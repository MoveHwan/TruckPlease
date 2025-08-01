using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameDailyRank : MonoBehaviour
{
    // 색상 설정
    string nicknameColor = "#3A8DFF"; // 선명한 파란색
    string tagColor = "#555555";     // 중간 회색
    string coloredText;

    public TextMeshProUGUI[] playerIdText;
    public TextMeshProUGUI[] playerScoreText;

    string rankingId = "StageClear";

    void Start()
    {
        StartCoroutine(WaitForLeaderboardData());
    }

    IEnumerator WaitForLeaderboardData()
    {
        while (LeaderboardSet.instance == null)
        {
            yield return new WaitForSeconds(0.2f); // 0.2초마다 체크
        }

        float timeout = 10f; // 최대 5초까지 기다림 (원하면 무제한으로도 가능)
        float elapsedTime = 0f;

        while (LeaderboardSet.instance.topScoresResponseEternalDaily == null)
        {
            yield return new WaitForSeconds(0.2f); // 0.2초마다 체크
            elapsedTime += 0.2f;

            if (elapsedTime >= timeout)
            {
                Debug.LogWarning("Leaderboard 데이터 준비 시간 초과");
                yield break; // 또는 오류 처리 UI 띄우기
            }
        }

        GetTopPlayers(rankingId);

    }


    void GetTopPlayers(string leaderboardId)
    {
        if (LeaderboardSet.instance.topScoresResponseEternalDaily == null)
            return;

        try
        {
            int index = 0;

            foreach (var playerScore in LeaderboardSet.instance.topScoresResponseEternalDaily.Results)
            {
                if (index >= 3)
                    return;

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

                playerIdText[index].text = coloredNickname;
                playerScoreText[index].text = playerScore.Score.ToString();
                index++;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching leaderboard scores: {e.Message}");
        }
    }
}
