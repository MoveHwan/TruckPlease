using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables; // 네임스페이스

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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeCount = 60f;
        SetStageData();
        GameStart();

#if UNITY_EDITOR || UNITY_STANDALONE
#elif UNITY_ANDROID || UNITY_IOS
                if (!tutoNeed)
        {
            tuto.SetActive(true);
            tutoNeed = true;
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
            Instantiate(stageObstacle);
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
        stage = PlayerPrefs.GetInt("Stage");
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
        if (!gamePause)
        {
            timeCount -= Time.deltaTime;
            timeCountUI.text = Mathf.Ceil(timeCount).ToString();
        }
    }

    public void GamePause()
    {
        gamePause = true;
    }
    public void GameResume()
    {
        gamePause = false;
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
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void PlayEndAni()
    {
        playableDirector.Play();
    }
}
