using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables; // 네임스페이스
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Net.Mime;
using UnityEngine.UI;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Animator truckAni;

    public StageData[] stageData;

    public int stage;
    public int stageSelect;
    public GameObject stageTruck;
    public GameObject stageWall;
    public GameObject stageCheckBox;
    public GameObject stageObstacle;
    public GameObject[] boxes;

    public float firstStar;
    public float secondStar;
    public float thirdStar;

    public bool gameEnd;
    public bool gamePause;

    public float timeCount;
    public bool obstacleReady;
    public TextMeshProUGUI timeCountUI;

    public PlayableDirector playableDirector;

    public GameObject tuto;
    public bool tutoNeed;

    string unityLeaderboard = "StageClear";
    string unityLeaderboardWeight = "TotalWeight";

    public GameObject ADBack;
    public ReviewInGame reviewInGame;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeCount = 60f;
        SetStageData();
        GameStart();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBGMIngame();
        }

        GoogleAd.instance.LoadAd();

#if UNITY_EDITOR || UNITY_STANDALONE
#elif UNITY_ANDROID || UNITY_IOS
                if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            tuto.SetActive(true);
            tutoNeed = true;
                    if (Application.internetReachability == NetworkReachability.NotReachable)
        {
        }
        else
        {
            AddScore(unityLeaderboard, 0);
        }

        }

#endif

    }

    // Update is called once per frame
    void Update()
    {
        TimeControl();
    }


    void SetStageData()
    {
        SelectStage(); // 겜 출시시 주석요망
        stageTruck = stageData[stage - 1].truck;
        stageWall = stageData[stage - 1].stageWall;
        stageCheckBox = stageData[stage - 1].stageCheckBox;
        if(stageData[stage - 1].stageObstacle != null)
        {
            stageObstacle = stageData[stage - 1].stageObstacle;
            Instantiate(stageObstacle, BoxManager.Instance.gameObject.transform);
        }
        firstStar = stageData[stage - 1].firstStar;
        secondStar = stageData[stage - 1].secondStar;
        thirdStar = stageData[stage - 1].thirdStar;
        BoxManager.Instance.box = stageData[stage - 1].boxes;
        Instantiate(stageTruck);
        Instantiate(stageWall);
        Instantiate(stageCheckBox);
        BoxManager.Instance.CalcTotalWei();
        BoxManager.Instance.CalcBoxCount();

        truckAni = GameObject.FindWithTag("Truck").GetComponent<Animator>();

        // 바람 관련 설정
        if (stageData[stage - 1].useWind && !stageData[stage - 1].random)
        {
            WindManager.instance.SetFixedWind(stageData[stage - 1].windType, stageData[stage - 1].windSpeed);
        }       
        else if(stageData[stage - 1].random)
        {
            WindManager.instance.RandomWind();
        }
        else
        {
            // 바람을 사용하지 않음 → WindManager에 비활성화 지시
            WindManager.instance.DisableWind();
        }
    }

    public void GameStart()
    {
        BoxManager.Instance.NextBoxSpawn();
    }



    void SelectStage()
    {
    #if UNITY_EDITOR || UNITY_STANDALONE
        stage = stageSelect;
#elif UNITY_ANDROID || UNITY_IOS
        Debug.Log("모바일(Android 또는 iOS)에서 실행됨");
        stage = PlayerPrefs.GetInt("Stage", 1);
#endif
    }


    public void ReadyToStart()
    {
        if (Ready())
        {
            BoxManager.Instance.CalcBoxIn();
            Debug.Log("나 이거함");
        }
    }

    bool Ready()
    {
        if (obstacleReady)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void TimeControl()
    {
        if (!gamePause && timeCount > -0.2f)
        {
            timeCount -= Time.deltaTime;
            timeCountUI.text = Mathf.Ceil(timeCount).ToString();
        }
        if (!gameEnd && timeCount < 0)
        {
            GameEnd();
        }
    }

    public void GamePause()
    {
        gamePause = true;
        Time.timeScale = 0;
    }
    public void GameResume()
    {
        gamePause = false;
        Time.timeScale = 1;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("InGame");
    }
    public void GameEnd()
    {
        gameEnd = true;
        truckAni.SetTrigger("GameEnd");
        playableDirector.Play();
        if (BoxManager.Instance.inBoxWeight >= firstStar)
        {
            StageSave();
        }
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void PlayEndAni()
    {
        playableDirector.Play();
    }

    void StageSave()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
        }
        else
        {
            AddScore(unityLeaderboard, stage);
            AddWeight(unityLeaderboard);
        }
    }

    // 유니티 점수 전달
    public async void AddScore(string leaderboardId, int score)
    {
        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(leaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public async void AddWeight(string leaderboardId)
    {
        float totalWeight = PlayerPrefs.GetFloat("TotalWeight");
        totalWeight += BoxManager.Instance.inBoxWeight;
        PlayerPrefs.SetFloat("TotalWeight", totalWeight);
        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(unityLeaderboardWeight, totalWeight);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public void StackIntAdClear()
    {
        //if (BoxManager.Instance.inBoxWeight >= firstStar)
        //{
        //    StackIntAd.instance.stack++;
        //    Debug.Log("전면광고 스택");

        //    if (StackIntAd.instance.stack >= 3)
        //    {
        //        Debug.Log("전면광고 나옴");
        //        StackIntAd.instance.stack = 0;
        //        GoogleAd.instance.ShowInterstitialAd();
        //    }
        //}
    }

    public void ShowAdBack()
    {
        ADBack.SetActive(true);
    }

    public void HideAdBack()
    {
        ADBack.SetActive(false);
    }

    public void ShowCoinRewardAd()
    {
        GoogleAd.instance.ShowRewardedAdCoin();
    }
}
