using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Unity.Services.Core;

public class LoadingScene : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;

    [Header("Loading Settings")]
    public float fakeLoadingTime = 2f;

    private void Start()
    {
        StartCoroutine(LoadSceneWithFakeProgress("InGame"));
    }

    private IEnumerator LoadSceneWithFakeProgress(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // 필요한 경우 LeaderboardSet 초기화 대기
        if (ShouldWaitForLeaderboardInit())
        {
            yield return WaitForTask(LeaderboardSet.instance.SetIngame());
        }

        // 페이크 로딩 진행
        yield return RunFakeLoading();

        // 실제 씬 로딩이 90% 도달할 때까지 대기
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // 연출용 딜레이
        operation.allowSceneActivation = true;
    }

    private bool ShouldWaitForLeaderboardInit()
    {
        return PlayerPrefs.GetInt("Stage") == 999 &&
               UnityServices.State == ServicesInitializationState.Initialized;
    }

    private IEnumerator RunFakeLoading()
    {
        float elapsed = 0f;

        while (elapsed < fakeLoadingTime)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsed / fakeLoadingTime);
            yield return null;
        }
    }

    // Task -> Coroutine 변환 유틸리티
    private IEnumerator WaitForTask(Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            Debug.LogError("Task Exception: " + task.Exception);
        }
    }
}
