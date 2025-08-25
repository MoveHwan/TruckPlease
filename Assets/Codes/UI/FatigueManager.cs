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
    private const int MaxFatigue = 5;
    private const int RecoveryMinutes = 15;

    [SerializeField] int currentFatigue, stage;

    bool CheckStageIn, unlimited;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadFatigue();
        InvokeRepeating(nameof(UpdateFatigue), 1f, 1f); // 1초마다 업데이트

#if !UNITY_EDITOR

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            StageIn();
            PlayerPrefs.SetInt("Fatigue", PlayerPrefs.GetInt("Fatigue", 5) + 1);
            PlayerPrefs.Save();
        }

#endif


    }

    void Update()
    {
        if (CheckStageIn && StageCheck.Instance != null)
        {
            CheckStageIn = false;

            Destroy(StageCheck.Instance.gameObject);

            if (StageManager.instance != null) 
            {
                if (StageManager.instance.EditorStageCheck())
                    stage = PlayerPrefs.GetInt("Stage", 1);
                else
                    stage = GameManager.Instance.stageSelect;
            }

            if (stage == 999)
            {
                SubFatigue();
                CheckStageIn = true;
            }
            else
            {
                PlayerPrefs.SetInt("StageIn", 1);
                PlayerPrefs.Save();
            }

            StageManager.instance.SetLandmark();
        }

        if (SceneManager.GetActiveScene().name == "Lobby" && PlayerPrefs.GetInt("StageIn") != 0)
        {
            currentFatigue -= 1;
            PlayerPrefs.SetInt("StageIn", 0);

            SaveFatigue();


            if (Application.internetReachability != NetworkReachability.NotReachable && GameDatas.instance)
            {
                GameDatas.instance.CloudSave();
            }

        }

        CheckRewardFatiuge();
    }

    void CheckRewardFatiuge()
    {
        if (PlayerPrefs.GetInt("isHeartReward", 0) == 1)
        {
            PlayerPrefs.SetInt("isHeartReward", 0);

            currentFatigue += 5;

            SaveFatigue();
        }
    }

    void LoadFatigue()
    {
        unlimited = PlayerPrefs.GetInt("Unlimited_Heart", 0) == 1;
        if (unlimited)
        {
            currentFatigue = 999;
            return;
        }

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
        unlimited = PlayerPrefs.GetInt("Unlimited_Heart", 0) == 1;
        if (unlimited)
        {
            currentFatigue = 999;
            return;
        }

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
        else
        {
            PlayerPrefs.SetString(LastTimeKey, DateTime.Now.ToString());
        }
    }


    public void UpdateUI(TextMeshProUGUI fatigueText, TextMeshProUGUI timerText)
    {
        if (unlimited)
        {
            fatigueText.text = "<size=350%>∞</size>";
            timerText.text = "--m --s";
            return;
        }

        currentFatigue = PlayerPrefs.GetInt("Fatigue", 5);

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

            timerText.text = $"{minutes:D2}m {seconds:D2}s";
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
        if (unlimited) return true;
        if (currentFatigue > 0) return true;
        
        return false;
    }

    public bool CheckRetryFatigue()
    {
        if (unlimited) return true;
        if (currentFatigue > 1) return true;

        return false;
    }

    public bool SubFatigue()
    {
        if (unlimited) return true;

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
