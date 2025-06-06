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

    // ���� �� �� �ִ��� Ȯ��
    public bool CanWatchAd()
    {
        RefreshTicketsIfNewDay();
        int tickets = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
        return tickets > 0;
    }

    // ���� �� �� ȣ��
    public void OnAdWatched()
    {
        if (!CanWatchAd())
        {
            Debug.Log("������ �� �̻� ���� �� �� �����ϴ�.");
            return;
        }

        int tickets = PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
        tickets--;
        PlayerPrefs.SetInt(AdTicketKey, tickets);
        PlayerPrefs.Save();
        Debug.Log($"���� ��û �Ϸ�! ���� Ƽ��: {tickets}/{MaxTicketsPerDay}");

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

    // ��¥�� �ٲ�� Ƽ���� ����
    private void RefreshTicketsIfNewDay()
    {
        string lastDate = PlayerPrefs.GetString(LastAdDateKey, "");
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastDate != today)
        {
            PlayerPrefs.SetString(LastAdDateKey, today);
            PlayerPrefs.SetInt(AdTicketKey, MaxTicketsPerDay);
            PlayerPrefs.Save();
            Debug.Log("���ο� ���Դϴ�! ���� Ƽ���� ��� �����Ǿ����ϴ�.");
        }
    }

    // ���� Ƽ�� �� �������� (UI ǥ�ÿ�)
    public int GetRemainingTickets()
    {
        RefreshTicketsIfNewDay();
        return PlayerPrefs.GetInt(AdTicketKey, MaxTicketsPerDay);
    }
}
