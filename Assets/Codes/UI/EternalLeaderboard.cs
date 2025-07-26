using GooglePlayGames;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.EventSystems.EventTrigger;

public class EternalLeaderboard : MonoBehaviour
{
    // 색상 설정
    string nicknameColor = "#3A8DFF"; // 선명한 파란색
    string tagColor = "#555555";     // 중간 회색
    string coloredText;

    //public Text score;
    public GameObject[] rankSet;
    TextMeshProUGUI[] rankText;
    Image[] chaImages;
    TextMeshProUGUI[] playerIdText;
    TextMeshProUGUI[] playerScoreText;
    GameObject[] myRankFrame;

    public TextMeshProUGUI myPlayerIdText;
    public TextMeshProUGUI myPlayerScore;
    public TextMeshProUGUI myPlayerRankText;
    //public GameObject setting;
    public TextMeshProUGUI messageText; // 로그인 필요 메시지
    public GameObject LoadingPanel;

    string rankingId = "StageClear";

    public ScrollRect scrollRect;

    [Header("[MyScore]")]
    string myNickname;
    int myScore;
    int myRank;

    private bool isAuthenticated = false; // 인증 여부 저장

    async void Awake()
    {
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

            SetRankData();
            // 다음 프레임에서 스크롤을 맨 위로 설정
            StartCoroutine(ResetScrollPosition());

            GetPlayerScore(rankingId);

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




    //void Awake()
    //{
    //    SetRankData();
    //}

    //void OnEnable()
    //{
    //    // 다음 프레임에서 스크롤을 맨 위로 설정
    //    StartCoroutine(ResetScrollPosition());

    //    GetPlayerScore(rankingId);
    //}

    void SetRankData()
    {
        int count = rankSet.Length;

        // 배열 초기화
        rankText = new TextMeshProUGUI[count];
        chaImages = new Image[count];
        playerIdText = new TextMeshProUGUI[count];
        playerScoreText = new TextMeshProUGUI[count];
        myRankFrame = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            Transform parent = rankSet[i].transform;

            // 자식 인덱스: 1, 3, 4, 6
            rankText[i] = parent.GetChild(0).GetComponent<TextMeshProUGUI>();
            chaImages[i] = parent.GetChild(1).GetChild(0).GetComponent<Image>();
            playerIdText[i] = parent.GetChild(2).GetComponent<TextMeshProUGUI>();
            playerScoreText[i] = parent.GetChild(3).GetComponent<TextMeshProUGUI>();
            myRankFrame[i] = parent.GetChild(5).gameObject;
        }
        for (int i = 3; i < count; i++)
        {
            rankText[i].text = (i + 1).ToString();
        }
    }

    // 유니티 나의 랭크 가져오기
    public async void GetPlayerScore(string leaderboardId)
    {
        var options = new GetPlayerScoreOptions
        {
            IncludeMetadata = true
        };

        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(leaderboardId, options);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        myScore = (int)scoreResponse.Score;
        myNickname = scoreResponse.PlayerName;
        myRank = scoreResponse.Rank;

        Debug.Log(myScore.ToString());
        Debug.Log(myNickname);

        // 닉네임과 태그 분리
        string[] parts = myNickname.Split('#');
        string nameOnly = parts.Length > 0 ? parts[0] : myNickname;
        string tagOnly = parts.Length > 1 ? "#" + parts[1] : "";

        // 색상 + 크기 조합 (태그는 70% 사이즈)
        string coloredNickname = $"<b><color={nicknameColor}>{nameOnly}</color></b>";


        //myPlayerIdText.text = coloredNickname;
        myPlayerScore.text = myScore.ToString();
        myPlayerRankText.text = (myRank + 1).ToString(); // 등수 UI에 표시

        GetTopPlayers(rankingId);
    }

    public async void GetTopPlayers(string leaderboardId)
    {
        try
        {
            var options = new GetScoresOptions
            {
                IncludeMetadata = true,
                Limit = 100
            };


            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, options);

            if (scoresResponse == null || scoresResponse.Results == null)
            {
                Debug.LogError("Failed to fetch leaderboard scores.");
                return;
            }

            int index = 0;

            foreach (var playerScore in scoresResponse.Results)
            {
                string playername = playerScore.PlayerName;
                string[] parts = playername.Split('#');
                string nameOnly = parts.Length > 0 ? parts[0] : playername;
                string tagOnly = parts.Length > 1 ? "#" + parts[1] : "";

                string chaImage;

                if (!string.IsNullOrEmpty(playerScore.Metadata))
                {
                    try
                    {
                        var metadataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(playerScore.Metadata);

                        if (metadataDict != null && metadataDict.TryGetValue("myImage", out var myImage))
                        {
                            Debug.Log("myImage: " + myImage); // 출력: Human_1
                            chaImage = myImage;
                        }
                        else
                        {
                            Debug.LogWarning("myImage 키가 metadata에 없습니다.");
                            chaImage = "Human_1";
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Metadata JSON 파싱 실패: " + ex.Message);
                        chaImage = "Human_1";

                    }
                }
                else
                {
                    chaImage = "Human_1";

                    Debug.LogWarning("Metadata가 비어 있거나 null입니다.");
                }

                // 색상 + 크기 조합 (태그는 70% 사이즈)
                string coloredNickname = $"<b><color={nicknameColor}>{nameOnly}</color></b>";

                if (myNickname == playername)
                {
                    myRankFrame[index].SetActive(true);
                }
                playerIdText[index].text = coloredNickname;
                playerScoreText[index].text = playerScore.Score.ToString();
                chaImages[index].sprite = ProfilImageList.Instance.GetSprite(chaImage);
                index++;
            }

            Debug.Log(chaImages[0].sprite);
            foreach (var playerScore in scoresResponse.Results)
            {
                Debug.Log($"Rank {playerScore.Rank}: {playerScore.PlayerName} - {playerScore.Score}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching leaderboard scores: {e.Message}");
        }
    }


    IEnumerator ResetScrollPosition()
    {
        yield return null; // 1프레임 대기 (UI 레이아웃이 잡힌 후 실행)
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
