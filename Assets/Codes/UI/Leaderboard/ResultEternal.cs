using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultEternal : MonoBehaviour
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

    public GameObject LoadingPanel;

    void Awake()
    {
        SetRankData();
    }

    void Start()
    {
        StartCoroutine(WaitForLeaderboardData());
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
            playerIdText[i] = parent.GetChild(2).GetComponent<TextMeshProUGUI>();
            playerScoreText[i] = parent.GetChild(3).GetComponent<TextMeshProUGUI>();
            myRankFrame[i] = parent.GetChild(4).gameObject;
        }
        for (int i = 3; i < count; i++)
        {
            rankText[i].text = (i + 1).ToString();
        }
    }

    IEnumerator WaitForLeaderboardData()
    {
        float timeout = 10f; // 최대 5초까지 기다림 (원하면 무제한으로도 가능)
        float elapsedTime = 0f;

        while (LeaderboardSet.instance.playerRangeResponseEternal == null)
        {
            yield return new WaitForSeconds(0.2f); // 0.2초마다 체크
            elapsedTime += 0.2f;

            if (elapsedTime >= timeout)
            {
                Debug.LogWarning("Leaderboard 데이터 준비 시간 초과");
                yield break; // 또는 오류 처리 UI 띄우기
            }
        }

        GetTopPlayers(); // 데이터가 준비된 후 호출
    }

    // 유니티 나의 랭크 가져오기
    void GetTopPlayers()
    {
        LoadingPanel.SetActive(false);

        try
        {
            int index = 0;

            foreach (var playerScore in LeaderboardSet.instance.playerRangeResponseEternal.Results)
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
                
                rankText[index].text = playerScore.Rank.ToString();

                if (LeaderboardSet.instance.myRank == playerScore.Rank)
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

}
