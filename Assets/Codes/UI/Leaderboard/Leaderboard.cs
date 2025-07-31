using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using DG.Tweening;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using System.Xml.Linq;
using UnityEngine.SocialPlatforms.Impl;
using System;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Core;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class LeaderBoard : MonoBehaviour
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

    void Awake()
    {
        SetRankData();
    }

    void OnEnable()
    {
        LoadingPanel.SetActive(true);
        // 다음 프레임에서 스크롤을 맨 위로 설정
        StartCoroutine(ResetScrollPosition());

        StartCoroutine(WaitForLeaderboardData());
    }

    IEnumerator WaitForLeaderboardData()
    {
        float timeout = 10f; // 최대 5초까지 기다림 (원하면 무제한으로도 가능)
        float elapsedTime = 0f;

        while (LeaderboardSet.instance.topScoresResponse == null)
        {
            yield return new WaitForSeconds(0.2f); // 0.2초마다 체크
            elapsedTime += 0.2f;

            if (elapsedTime >= timeout)
            {
                Debug.LogWarning("Leaderboard 데이터 준비 시간 초과");
                yield break; // 또는 오류 처리 UI 띄우기
            }
        }

        GetPlayerScore(); // 데이터가 준비된 후 호출
        LoadingPanel.SetActive(false);
    }

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
            chaImages[i].sprite = null;
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
    void GetPlayerScore()
    {
        var scoreResponse = LeaderboardSet.instance.playerScoreResponse;

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

    void GetTopPlayers(string leaderboardId)
    {
        try
        {
            //var options = new GetScoresOptions
            //{
            //    IncludeMetadata = true,
            //    Limit = 100
            //};


            //var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, options);

            int index = 0;

            foreach (var playerScore in LeaderboardSet.instance.topScoresResponse.Results)
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
