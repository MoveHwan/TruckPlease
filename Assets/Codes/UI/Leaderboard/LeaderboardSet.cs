using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms.Impl;


public class LeaderboardSet : MonoBehaviour
{
    public static LeaderboardSet instance;

    public bool setComp;

    public LeaderboardEntry playerScoreResponse;
    public LeaderboardScoresPage topScoresResponse;

    async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        await TryInitializeUnityServices();
    }

    // 인터넷 체크 후 통과 
    async Task TryInitializeUnityServices()
    {
        await InitializeUnityServices();

#if UNITY_EDITOR
        await SignInAnonymouslyEditor(); // 에디터에서 익명 로그인
#else
    InitializeGooglePlayGames(); // 안드로이드에서 GPGS 로그인
                GetPlayerScore("StageClear");

#endif


    }

    async Task SignInAnonymouslyEditor()
    {
        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Editor: Signed in anonymously with PlayerID: " + AuthenticationService.Instance.PlayerId);

                // 닉네임 설정
                string nickname = "EditorPlayer";
                await AuthenticationService.Instance.UpdatePlayerNameAsync(nickname);
            }


            string myImage = PlayerPrefs.GetString("ProfileImage", "Human_1");

            var metadata = new Dictionary<string, object>
            {
                { "myImage", myImage },
            };

            // AddPlayerScoreOptions 객체 생성
            var options = new AddPlayerScoreOptions
            {
                Metadata = metadata, // 여기에 메타데이터 설정
            };

            Debug.Log("options" + options.ToString());
            await LeaderboardsService.Instance.AddPlayerScoreAsync("StageClear", 103, options);

            GetPlayerScore("StageClear");
            // 테스트용: 리더보드 점수 확인
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"Editor Anonymous Auth failed: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Editor Request failed: {ex.Message}");
        }
    }

    async Task InitializeUnityServices()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            Debug.Log("Unity Services already initialized.");
            //gdprSet.GDPRFormAvail();
            return;
        }

        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
            //gdprSet.GDPRFormAvail();


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
        //PlayGamesPlatform.Instance.Authenticate((success) =>
        //{
        //    if (success == SignInStatus.Success)
        //    {
        //        Debug.Log("Login with Google Play Games successful.");

        //        PlayGamesPlatform.Instance.RequestServerSideAccess(true, async (code) =>
        //        {
        //            Debug.Log("Authorization code: " + code);
        //            Token = code;

        //            // 비동기 실행을 위해 Task 사용
        //            await SignInWithGooglePlayGamesAsync(Token);
        //            StartCoroutine(loadingLogin.WaitLoadingSecond());
        //        });
        //    }
        //    else
        //    {
        //        Error = "Failed to retrieve Google Play Games authorization code";
        //        Debug.LogError("Login Unsuccessful");
        //    }
        //});
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

    async void GetPlayerScore(string leaderboardId)
    {
        var options = new GetPlayerScoreOptions
        {
            IncludeMetadata = true
        };

        playerScoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(leaderboardId, options);

        var optionsTop = new GetScoresOptions
        {
            IncludeMetadata = true,
            Limit = 100
        };

        topScoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(leaderboardId, optionsTop);

    }


}
