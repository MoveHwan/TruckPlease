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
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Animator truckAni;

    public CinemachineVirtualCamera GameEndCamera;    // 게임 종료 카메라
    public GameObject GameEndAim;                       // 게임 종료 에임
    public StageData[] stageData;
    public StageData eternalData;       // 무한모드 데이터
    public bool eternalMode;            // 무한모드 불

    public int stage;
    public int stageSelect;
    public GameObject stageTruck;
    public GameObject stageWall;
    public GameObject stageCheckBox;
    public GameObject stageObstacle;
    public GameObject[] boxes;
    public int life;
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
    string unityLeaderboardEternal = "EternalMode";
    string unityLeaderboardEternalDaily = "DailyEternal";
    string unityLeaderboardWeight = "TotalWeight";

    public GameObject ADBack;
    public ReviewInGame reviewInGame;
    public GameObject tutoThrowZone;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeCount = 5f;
        SetStageData();
        GameStart();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBGMIngame();
        }

        GoogleAd.instance.LoadAd();

        if (PlayerPrefs.GetInt("TutoThrowZone") <= 3 && PlayerPrefs.GetInt("Tutorial") != 0)
        {
            tutoThrowZone.SetActive(true);
        }

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
        //TimeControl();
    }


    void SetStageData()
    {
        SelectStage(); // 겜 출시시 주석요망

        if (stage == 999)
        {
            SetEternalMode();
        }
        else
        {
            //stageTruck = stageData[stage - 1].truck;
            stageWall = stageData[stage - 1].stageWall;
            stageCheckBox = stageData[stage - 1].stageCheckBox;
            life = stageData[stage - 1].life;

            if (stageData[stage - 1].stageObstacle != null)
            {
                stageObstacle = stageData[stage - 1].stageObstacle;
                Instantiate(stageObstacle, BoxManager.Instance.gameObject.transform);
            }
            firstStar = stageData[stage - 1].firstStar;
            secondStar = stageData[stage - 1].secondStar;
            thirdStar = stageData[stage - 1].thirdStar;
            BoxManager.Instance.box = stageData[stage - 1].boxes;
            //(stageTruck);
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
            else if (stageData[stage - 1].random)
            {
                WindManager.instance.RandomWind();
            }
            else
            {
                // 바람을 사용하지 않음 → WindManager에 비활성화 지시
                WindManager.instance.DisableWind();
            }
        }
    }

    void SetEternalMode()
    {
        eternalMode = true;
        //stageTruck = eternalData.truck;
        stageWall = eternalData.stageWall;
        stageCheckBox = eternalData.stageCheckBox;
        life = eternalData.life;
        if (eternalData.stageObstacle != null)
        {
            stageObstacle = eternalData.stageObstacle;
            Instantiate(stageObstacle, BoxManager.Instance.gameObject.transform);
        }
        firstStar = eternalData.firstStar;
        secondStar = eternalData.secondStar;
        thirdStar = eternalData.thirdStar;
        //BoxManager.Instance.box = stageData[stage - 1].boxes;
        BoxManager.Instance.AddOneRandomBox();
        BoxManager.Instance.AddOneRandomBox();
        //Instantiate(stageTruck);
        Instantiate(stageWall);
        Instantiate(stageCheckBox);
        BoxManager.Instance.CalcTotalWei();
        BoxManager.Instance.CalcBoxCount();

        truckAni = GameObject.FindWithTag("Truck").GetComponent<Animator>();

        // 바람 관련 설정
        if (eternalData.useWind && !eternalData.random)
        {
            WindManager.instance.SetFixedWind(eternalData.windType, eternalData.windSpeed);
        }
        else if (eternalData.random)
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
        GameEndCamera.Priority = 20;
        //GameEndAim.transform.DOMove(new Vector3(0, 0.807f, -1.77f), 3f); // 1.5초 동안 이동        
        truckAni.SetTrigger("GameEnd");
        if (!eternalMode) playableDirector.Play();
        
        if (eternalMode)
        {
            PlayerPrefs.SetInt("EternalMode", (int)Mathf.Round(BoxManager.Instance.inBoxWeight));
            PlayerPrefs.Save();
            AddScoreEternal((int)Mathf.Round(BoxManager.Instance.inBoxWeight));
        }
        else if (BoxManager.Instance.inBoxWeight >= firstStar)
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
        string myImage = PlayerPrefs.GetString("ProfileImage", "Human_1");

        var metadata = new Dictionary<string, object>
        {
            { "myImage", myImage },
        };

        // AddPlayerScoreOptions 객체 생성
        var options = new AddPlayerScoreOptions
        {
            Metadata = metadata, // 여기에 메타데이터 설정
        };

        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(leaderboardId, score, options);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public async void AddScoreEternal(int score)
    {
        string myImage = PlayerPrefs.GetString("ProfileImage", "Human_1");

        var metadata = new Dictionary<string, object>
        {
            { "myImage", myImage },
        };

        // AddPlayerScoreOptions 객체 생성
        var options = new AddPlayerScoreOptions
        {
            Metadata = metadata, // 여기에 메타데이터 설정
        };

        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(unityLeaderboardEternal, score, options);

        LeaderboardSet.instance.SetResultEternal();

        var playerEntryDaily = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(unityLeaderboardEternalDaily, score, options);

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
        if (BoxManager.Instance.inBoxWeight >= firstStar)
        {
            StackIntAd.instance.stack++;
            Debug.Log("전면광고 스택");

            if (StackIntAd.instance.stack >= 3)
            {
                Debug.Log("전면광고 나옴");
                StackIntAd.instance.stack = 0;
                GoogleAd.instance.ShowInterstitialAd();
            }
        }
    }

    public void EternalAd()
    {
        GoogleAd.instance.ShowInterstitialAd();
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

    public void CloseTutoZone()
    {
        tutoThrowZone.SetActive(false);

        int tutoZoneCount = PlayerPrefs.GetInt("TutoThrowZone");
        tutoZoneCount++;
        PlayerPrefs.SetInt("TutoThrowZone", tutoZoneCount);
        PlayerPrefs.Save();
    }

    public void CameraAimUp()
    {
        GameEndAim.transform.DOMove(new Vector3(0, 0.807f, -1.77f), 0.3f); // 1.5초 동안 이동  
    }
}
