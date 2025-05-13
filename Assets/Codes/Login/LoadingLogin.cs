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
    public GameObject nicknamePanel;
    public TextMeshProUGUI nickName;

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
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }



    // ��� �α��ν� ����
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            // Perfectly login success

            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            //logText.text = "Success \n" + name;
            UnityConnect();
            //Ŭ���� ���̺� �������� �ϴ� ����
            //DataConnectGP dataConnectGP = GetComponent<DataConnectGP>();
            //dataConnectGP.LoadData();
        }
        else
        {
            //logText.text = "Sign in Failed!";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
            // Login failed
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
        SceneManager.LoadScene("Lobby");
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
