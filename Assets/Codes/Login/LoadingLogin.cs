using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine.SceneManagement;
using TMPro;


public class LoadingLogin : MonoBehaviour
{
    public GameObject slider;
    public GameObject unityLogin;

    Slider loadingSlider;
    float duration = 3f; // 3�� ���� ä���
    float elapsed = 0f;  // ��� �ð�

    int loginGoogle = 1;
    int guest = 2;

    void Awake()
    {
        loadingSlider = slider.GetComponent<Slider>();
    }

    void Start()
    {
        //CheckNickName();
    }

    // �г����� �ִ��� Ȯ��
    public void CheckNickName()
    {
        if (PlayerPrefs.HasKey("nickname"))
        {
            StartCoroutine(WaitLoadingfirst());
        }
        else
        {
            SetRandomNickName();
        }
    }


    public void SignIn()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // ���ͳ� ���� ����
            Debug.Log("���ͳ� ���ῡ ������� �ʾҽ��ϴ�.");
            StartCoroutine(WaitLoadingSecond());    
        }
        else
        {
            // ���ͳ� �����
            Debug.Log("���ͳ� ���ῡ ����Ǿ� �ֽ��ϴ�.");
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
    }



    // ��� �α��ν� ����
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            UnityConnect();
            //StartCoroutine(WaitLoadingSecond());

        }
        else
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }

    // �ε� �����̴�
    IEnumerator WaitLoadingfirst()
    {
        while (loadingSlider.value < 0.7f)
        {
            elapsed += Time.deltaTime;
            loadingSlider.value = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;
        }

        SignIn();
    }

    void UnityConnect()
    {
        // ���ͳ� �����
        unityLogin.SetActive(true);
        
    }

    // �ε� �ι�°
    public IEnumerator WaitLoadingSecond()
    {
        while (loadingSlider.value < 1)
        {
            elapsed += Time.deltaTime;
            loadingSlider.value = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;
        }
        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            PlayerPrefs.SetInt("Stage", 1);
            SceneManager.LoadScene("InGame");
        }
        else
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    void SetRandomNickName()
    {
        int randomNum = Random.Range(0, 99999);
        string nickname = "player" + randomNum.ToString();
        PlayerPrefs.SetString("nickname",nickname);
        PlayerPrefs.Save();

        StartCoroutine(WaitLoadingfirst());
    }

}
