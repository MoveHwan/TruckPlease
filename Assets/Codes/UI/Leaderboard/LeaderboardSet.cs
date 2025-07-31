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
using static UnityEditor.Progress;


public class LeaderboardSet : MonoBehaviour
{
    public static LeaderboardSet instance;

    public bool setComp;

    public int myRank;          // 무한모드 등수

    public LeaderboardEntry playerScoreResponse;
    public LeaderboardScoresPage topScoresResponse;
    public LeaderboardScoresPage topScoresResponseEternal;
    public LeaderboardScoresPage topScoresResponseEternalDaily;
    public LeaderboardScoresPage playerRangeResponseEternal;
    public LeaderboardEntry playerScoreResponseEternal;

    int rangeLimit = 5;

    public bool readyResult;

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
                SetLobby();
            SetIngame();
            SetResultEternal();

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
            await LeaderboardsService.Instance.AddPlayerScoreAsync("StageClear", 32, options);

            SetLobby();
            SetIngame();
            SetResultEternal();
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

    // 로비 리더보드 준비
    public async void SetLobby()
    {
        var options = new GetPlayerScoreOptions
        {
            IncludeMetadata = true
        };

        playerScoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync("StageClear", options);

        var optionsTop = new GetScoresOptions
        {
            IncludeMetadata = true,
            Limit = 100
        };

        topScoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync("StageClear", optionsTop);

        playerScoreResponseEternal = await LeaderboardsService.Instance
            .GetPlayerScoreAsync("EternalMode");            // 무한모드 내점수

        myRank = playerScoreResponseEternal.Rank;

        var optionsTopEternal = new GetScoresOptions
        {
            IncludeMetadata = true,
            Limit = 100
        };

        topScoresResponseEternal = await LeaderboardsService.Instance
            .GetScoresAsync("EternalMode", optionsTopEternal);
    }

    // 인게임 리더보드 준비
    public async void SetIngame()
    {
        playerScoreResponseEternal = await LeaderboardsService.Instance
            .GetPlayerScoreAsync("EternalMode");            // 무한모드 내점수

        myRank = playerScoreResponseEternal.Rank;

        var optionsTopEternalDaily = new GetScoresOptions
        {
            IncludeMetadata = true,
            Limit = 3
        };

        topScoresResponseEternalDaily = await LeaderboardsService.Instance
            .GetScoresAsync("EternalMode", optionsTopEternalDaily);

    }

    // 인게임 결과창 준비
    public async void SetResultEternal()
    {
        playerScoreResponseEternal = await LeaderboardsService.Instance
            .GetPlayerScoreAsync("EternalMode");            // 무한모드 내점수

        myRank = playerScoreResponseEternal.Rank;

        int startRank = Mathf.Max(myRank - 2, 0);
        playerRangeResponseEternal = await LeaderboardsService.Instance.GetScoresAsync(
            "EternalMode",
            new GetScoresOptions
            {
                IncludeMetadata = true,
                Offset = startRank,
                Limit = rangeLimit
            });

        readyResult = true;
    }
}
