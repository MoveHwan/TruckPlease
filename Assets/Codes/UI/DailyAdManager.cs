using UnityEngine;
using System;

public class DailyAdManager : MonoBehaviour
{
    const string defaultDateKey = "LastAdDate";
    const string defaultTicketKey = "AdTickets";

    string LastAdDateKey, AdTicketKey;

    int MaxTicketsPerDay = 2;

    public string name;

    void OnEnable()
    {
        LastAdDateKey = defaultDateKey + "_" + name;
        AdTicketKey = defaultTicketKey + "_" + name;

        if (!CanWatchAd())
        {
            gameObject.SetActive(false);
        }
    }

    // 광고 볼 수 있는지 확인
    public bool CanWatchAd()
    {
        RefreshTicketsIfNewDay();
        int tickets = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
        return tickets > 0;
    }

    // 광고 본 후 호출
    public void OnAdWatched()
    {
        if (!CanWatchAd())
        {
            Debug.Log("오늘은 더 이상 광고를 볼 수 없습니다.");
            return;
        }

        int tickets = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
        tickets--;
        PlayerPrefs.SetInt(AdTicketKey, tickets);
        PlayerPrefs.Save();
        Debug.Log($"광고 시청 완료! 남은 티켓: {tickets}/{MaxTicketsPerDay}");

        switch (name)
        {
            case "RewardCoin":
                GoogleAd.instance.ShowRewardedAdCoin();
                break;
            case "Save":
                GoogleAd.instance.ShowRewardedAd();
                break;
            case "Revert":
                GoogleAd.instance.ShowRewardedAd();
                break;
            default:
                return;
                    
        }
    }

    // 날짜가 바뀌면 티켓을 리셋
    private void RefreshTicketsIfNewDay()
    {
        string lastDate = PlayerPrefs.GetString(LastAdDateKey, "");
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastDate != today)
        {
            PlayerPrefs.SetString(LastAdDateKey, today);
            PlayerPrefs.SetInt(AdTicketKey, MaxTicketsPerDay);
            PlayerPrefs.Save();
            Debug.Log("새로운 날입니다! 광고 티켓이 모두 충전되었습니다.");
        }
    }

    // 남은 티켓 수 가져오기 (UI 표시용)
    public int GetRemainingTickets()
    {
        RefreshTicketsIfNewDay();
        return PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
    }
}
