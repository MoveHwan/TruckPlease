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
    float duration = 3f; // 3초 동안 채우기
    float elapsed = 0f;  // 경과 시간

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

    // 닉네임이 있는지 확인
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



    // 통상 로그인시 과정
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
            //클라우드 세이브 오류나서 일단 꺼둠
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

    // 로딩 슬라이더
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

    // 로딩 두번째
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
