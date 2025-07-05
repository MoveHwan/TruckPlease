using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Analytics;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SocialPlatforms;
using TMPro;

public class UnityLogin : MonoBehaviour
{
    string Token;
    public string Error;
    string rankingId = "StageClear";
    public TextMeshProUGUI checkText;
    public LoadingLogin loadingLogin;

    public GDPRSet gdprSet;

    async void Awake()
    {
        await TryInitializeUnityServices();
    }

    // 인터넷 체크 후 통과 
    async Task TryInitializeUnityServices()
    {
        await InitializeUnityServices();
        InitializeGooglePlayGames(); // Google Play Games 활성화
    }

    async Task InitializeUnityServices()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            Debug.Log("Unity Services already initialized.");
            gdprSet.GDPRFormAvail();
            return;
        }

        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
            gdprSet.GDPRFormAvail();


        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unity Services Initialization Failed: {e.Message}");
        }
    }

    void InitializeGooglePlayGames()
    {
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play Games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, async (code) =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;

                    // 비동기 실행을 위해 Task 사용
                    await SignInWithGooglePlayGamesAsync(Token);
                    StartCoroutine(loadingLogin.WaitLoadingSecond());
                });
            }
            else
            {
                Error = "Failed to retrieve Google Play Games authorization code";
                Debug.LogError("Login Unsuccessful");
            }
        });
    }

    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log("Sign-in is successful.");

            // 구글 플레이 닉네임 가져오기
            //string googleNickname = GetGooglePlayNickname();
            string SetNickname = PlayerPrefs.GetString("nickname");
            Debug.Log($"Google Play Nickname: {SetNickname}");

            // Unity Authentication에 닉네임 업데이트
            await UpdatePlayerDisplayName(SetNickname);
            // 처음 들어갔을때 스코어 있는지 확인
            if (PlayerPrefs.GetInt("first") == 0)
            {
                GetPlayerScore(rankingId);
            }

        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"Authentication failed: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Request failed: {ex.Message}");
        }
    }

    async Task UpdatePlayerDisplayName(string nickname)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(nickname);
            Debug.Log($"Updated Player Display Name: {nickname}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to update player display name: {ex.Message}");
        }
    }

    string GetGooglePlayNickname()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            return PlayGamesPlatform.Instance.GetUserDisplayName();
        }
        return "UnknownPlayer"; // 로그인 실패 시 기본 닉네임
    }


    public async void GetPlayerScore(string leaderboardId)
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(leaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        int myScore = (int)scoreResponse.Score;
        //PlayerPrefs.SetInt("stage", myScore);
        PlayerPrefs.SetInt("first", 1);
        PlayerPrefs.Save();
    }

    // 인터넷 연결 확인하기
    void ShowInternetMessage()
    {
        checkText.gameObject.SetActive(true);
        checkText.color = new Color(checkText.color.r, checkText.color.g, checkText.color.b, 1); // 알파값 초기화
    }

    bool InternetConnect()
    {
        return Application.internetReachability == NetworkReachability.NotReachable;
    }


}
