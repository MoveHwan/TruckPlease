using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

using UnityEngine.SceneManagement;
using Unity.Services.Core;

public class GoLobby : MonoBehaviour
{
    public Slider slider;
    public float fakeLoadingTime = 2f; // 슬라이더가 채워지는 시간 (초)
    bool eternalMode;

    void Start()
    {
        StartCoroutine(LoadSceneWithFakeProgress("Lobby"));
    }

    IEnumerator LoadSceneWithFakeProgress(string sceneName)
    {
        // 실제 씬 로딩 시작 (백그라운드)
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;

        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            yield return WaitForTask(LeaderboardSet.instance.SetLobby());
        }


        while (elapsed < fakeLoadingTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / fakeLoadingTime); // 0 ~ 1
            slider.value = progress;
            yield return null;
        }

        // 로딩 완료되었는지 확인 (혹시 덜 끝났으면 기다림)
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // 연출용 약간의 딜레이
        operation.allowSceneActivation = true;
    }

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
