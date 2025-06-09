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


public class LeaderBoard : MonoBehaviour
{
    // 색상 설정
    string nicknameColor = "#3A8DFF"; // 선명한 파란색
    string tagColor = "#555555";     // 중간 회색
    string coloredText;

    //public Text score;
    public TextMeshProUGUI[] playerIdText;
    public TextMeshProUGUI[] playerScoreText;
    public TextMeshProUGUI myPlayerIdText;
    public TextMeshProUGUI myPlayerScore;
    public TextMeshProUGUI myPlayerRankText;
    //public GameObject setting;
    public TextMeshProUGUI messageText; // 로그인 필요 메시지
    public GameObject LoadingPanel;

    string rankingId = "StageClear";

    [Header("[MyScore]")]
    string myNickname;
    int myScore;
    int myRank;

    private bool isAuthenticated = false; // 인증 여부 저장

    void Awake()
    {
    }

    void OnEnable()
    {
        LoadingPanel.SetActive(true);
        GetPlayerScore(rankingId);
        GetTopPlayers(rankingId);
    }

    // 로그인을 안했을시 로그인을 하라는 메시지
    public void ShowLoginMessage()
    {
        float duration = 1.5f; // 애니메이션 지속 시간
        Vector3 moveOffset = new Vector3(0, 100, 0); // 이동 거리

        messageText.gameObject.SetActive(true);
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1); // 알파값 초기화

        // 현재 위치에서 moveOffset 만큼 위로 이동
        messageText.rectTransform.anchoredPosition += new Vector2(0, -moveOffset.y);

        // DOTween 애니메이션 실행
        messageText.rectTransform.DOAnchorPosY(messageText.rectTransform.anchoredPosition.y + moveOffset.y, duration);
        messageText.DOFade(0, duration).OnComplete(() =>
        {
            messageText.gameObject.SetActive(false); // 애니메이션 완료 후 비활성화
        });
    }

    // 유니티 나의 랭크 가져오기
    public async void GetPlayerScore(string leaderboardId)
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(leaderboardId);
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
        string coloredNickname = $"<b><color={nicknameColor}>{nameOnly}</color></b><size=70%><color={tagColor}>{tagOnly}</color></size>";


        myPlayerIdText.text = coloredNickname;
        myPlayerScore.text = myScore.ToString();
        myPlayerRankText.text = (myRank + 1).ToString(); // 등수 UI에 표시
    }

    public async void GetTopPlayers(string leaderboardId)
    {
        try
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, new GetScoresOptions { Limit = 12 });

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

                // 색상 + 크기 조합 (태그는 70% 사이즈)
                string coloredNickname = $"<b><color={nicknameColor}>{nameOnly}</color></b><size=70%><color={tagColor}>{tagOnly}</color></size>";


                playerIdText[index].text = coloredNickname;
                playerScoreText[index].text = playerScore.Score.ToString();
                index++;
            }
            LoadingPanel.SetActive(false);

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
}
