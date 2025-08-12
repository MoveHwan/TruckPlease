using UnityEngine;
using System;
using TMPro;

public class DailyAdManager : MonoBehaviour
{
    const string defaultDateKey = "LastAdDate";
    const string defaultTicketKey = "AdTickets";

    string LastAdDateKey, AdTicketKey;

    int MaxTicketsPerDay = 2;

    public GameObject ADButton;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI NextTimeText;

    public string name;

    public bool isLobbyAd;

    void OnEnable()
    {
        this.enabled = !isLobbyAd;

        if (ADButton == null) ADButton = gameObject;
        if (name == "ShopGold") MaxTicketsPerDay = 3;

        LastAdDateKey = defaultDateKey + "_" + name;
        AdTicketKey = defaultTicketKey + "_" + name;

        if (!CanWatchAd())
        {
            ADButton.SetActive(false);
            return;
        }
    }

    void Update()
    {
        if (CanWatchAd() && !ADButton.activeSelf) 
        {
            ADButton.SetActive(true);
        }

        RefreshText();
    }

    // 광고 볼 수 있는지 확인
    public bool CanWatchAd()
    {
        RefreshTicketsIfNewDay();
        int tickets = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
        return tickets > 0;
    }

    // 광고 호출
    public void OnAdWatched()
    {
        if (!CanWatchAd())
        {
            Debug.Log("오늘은 더 이상 광고를 볼 수 없습니다.");
            return;
        }

        switch (name)
        {
            case "RewardCoin":
                GoogleAd.instance.ShowRewardedAdCoin();
                break;
            case "Save":
                GoogleAd.instance.ShowRewardedAd(this);
                break;
            case "Revert":
                GoogleAd.instance.ShowRewardedAd(this);
                break;
            case "Heart":
                GoogleAd.instance.ShowRewardedAdHeart(this);
                break;
            case "ShopGold":
                GoogleAd.instance.ShowRewardedAdShopCoin(this);
                break;
            default:
                return;
                    
        }

    }

    public void SubTicket()
    {
        int tickets = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
        tickets--;
        PlayerPrefs.SetInt(AdTicketKey, tickets);
        PlayerPrefs.Save();
        Debug.Log($"광고 시청 완료! 남은 티켓: {tickets}/{MaxTicketsPerDay}");

        if (name == "Heart") gameObject.SetActive(false);
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

    void RefreshText()
    {
        if (ADButton.activeSelf && CountText != null)
        {
            CountText.text = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay) + "/" + MaxTicketsPerDay;
        }

        if (!ADButton.activeSelf && NextTimeText != null)
        {
            // 남은 시간 계산
            TimeSpan remainingTime = DateTime.Now.Date.AddDays(1) - DateTime.Now;

            NextTimeText.text = $"{remainingTime.Hours:D2}h {remainingTime.Minutes:D2}m {remainingTime.Seconds:D2}s";
        }
    }
}
