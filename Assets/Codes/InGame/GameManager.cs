using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // 네임스페이스

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

    public float timeCount;
    public bool obstacleReady;
    public TextMeshProUGUI timeCountUI;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeCount = 60f;
        SetStageData();
        GameStart();
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

    public void GameEnd()
    {
        gameEnd = true;
        truckAni.SetTrigger("GameEnd");
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    void SelectStage()
    {
        stage = stageSelect;
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
        timeCount -= Time.deltaTime;
        timeCountUI.text = Mathf.Ceil(timeCount).ToString();
    }
}
