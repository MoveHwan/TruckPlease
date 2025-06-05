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
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 인터넷 연결 없음
            Debug.Log("인터넷 연결에 연결되지 않았습니다.");
            StartCoroutine(WaitLoadingSecond());    
        }
        else
        {
            // 인터넷 연결됨
            Debug.Log("인터넷 연결에 연결되어 있습니다.");
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
    }



    // 통상 로그인시 과정
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
        // 인터넷 연결됨
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
