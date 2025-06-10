using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class FatigueManager : MonoBehaviour
{
    public static FatigueManager instance;

    private const string FatigueKey = "Fatigue";
    private const string LastTimeKey = "LastFatigueTime";
    private const int MaxFatigue = 10;
    private const int RecoveryMinutes = 5;

    [SerializeField] int currentFatigue;

    bool CheckStageIn;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        LoadFatigue();
        InvokeRepeating(nameof(UpdateFatigue), 1f, 1f); // 1초마다 업데이트

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
            StageIn();
    }

    void Update()
    {
        if (CheckStageIn && StageCheck.Instance != null)
        {
            CheckStageIn = false;

            PlayerPrefs.SetInt("StageIn", 1);
            PlayerPrefs.Save();

            Destroy(StageCheck.Instance.gameObject);
        }

        if (SceneManager.GetActiveScene().name == "Lobby" && PlayerPrefs.GetInt("StageIn") != 0)
        {
            currentFatigue -= 1;
            PlayerPrefs.SetInt("StageIn", 0);

            SaveFatigue();
        }
    }

    void LoadFatigue()
    {
        currentFatigue = PlayerPrefs.GetInt(FatigueKey, MaxFatigue);

        string lastTimeStr = PlayerPrefs.GetString(LastTimeKey, "");
        if (!string.IsNullOrEmpty(lastTimeStr))
        {
            DateTime lastTime = DateTime.Parse(lastTimeStr);
            TimeSpan timePassed = DateTime.Now - lastTime;

            int recoverAmount = Mathf.FloorToInt((float)timePassed.TotalMinutes / RecoveryMinutes);
            if (recoverAmount > 0)
            {
                currentFatigue = Mathf.Min(currentFatigue + recoverAmount, MaxFatigue);

                if (PlayerPrefs.GetInt("StageIn") != 0)
                {
                    currentFatigue -= 1;
                    PlayerPrefs.SetInt("StageIn", 0);
                }

                SaveFatigue();
            }
        }
    }

    void UpdateFatigue()
    {
        if (currentFatigue < MaxFatigue)
        {
            DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey, DateTime.Now.ToString()));
            TimeSpan timePassed = DateTime.Now - lastTime;

            int recoverAmount = Mathf.FloorToInt((float)timePassed.TotalMinutes / RecoveryMinutes);

            if (recoverAmount > 0)
            {
                currentFatigue = Mathf.Min(currentFatigue + recoverAmount, MaxFatigue);

                // 새롭게 피로도가 회복된 시점을 기준으로 다시 저장
                DateTime newLastTime = lastTime.AddMinutes(recoverAmount * RecoveryMinutes);
                PlayerPrefs.SetString(LastTimeKey, newLastTime.ToString());

                SaveFatigue();
            }
        }
    }


    public void UpdateUI(TextMeshProUGUI fatigueText, TextMeshProUGUI timerText)
    {
        fatigueText.text = $"{currentFatigue}/{MaxFatigue}";

        if (timerText == null) return;

        if (currentFatigue < MaxFatigue)
        {
            DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString(LastTimeKey, DateTime.Now.ToString()));
            TimeSpan timePassed = DateTime.Now - lastTime;
            TimeSpan timeLeft = TimeSpan.FromMinutes(RecoveryMinutes) - timePassed;

            if (timeLeft.TotalSeconds < 0) timeLeft = TimeSpan.Zero;

            int minutes = timeLeft.Minutes;
            int seconds = timeLeft.Seconds;

            timerText.text = $"{minutes}m {seconds:D2}s";
        }
        else
        {
            timerText.text = "--m --s";
        }
    }

    void SaveFatigue()
    {
        int previousFatigue = PlayerPrefs.GetInt(FatigueKey, MaxFatigue);

        PlayerPrefs.SetInt(FatigueKey, currentFatigue);

        // 최대치에서 하나 줄었을 때만 타이머 시작
        if (previousFatigue == MaxFatigue && currentFatigue == MaxFatigue - 1)
        {
            PlayerPrefs.SetString(LastTimeKey, DateTime.Now.ToString());
        }

        PlayerPrefs.Save();
    }


    public bool CheckFatigue()
    {
        if (currentFatigue > 0) return true;
        
        return false;
    }

    public bool SubFatigue()
    {
        if (currentFatigue > 0) 
        {
            currentFatigue -= 1;
            PlayerPrefs.SetInt("StageIn", 0);

            SaveFatigue();

            return true; 
        }

        return false;
    }

    public void StageIn()
    {
        CheckStageIn = true;
    }
}
