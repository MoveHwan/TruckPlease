using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public Slider slider;
    public float fakeLoadingTime = 2f; // �����̴��� ä������ �ð� (��)

    void Start()
    {
        StartCoroutine(LoadSceneWithFakeProgress("InGame"));
    }

    IEnumerator LoadSceneWithFakeProgress(string sceneName)
    {
        // ���� �� �ε� ���� (��׶���)
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;
        while (elapsed < fakeLoadingTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / fakeLoadingTime); // 0 ~ 1
            slider.value = progress;
            yield return null;
        }

        // �ε� �Ϸ�Ǿ����� Ȯ�� (Ȥ�� �� �������� ��ٸ�)
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // ����� �ణ�� ������
        operation.allowSceneActivation = true;
    }
}
