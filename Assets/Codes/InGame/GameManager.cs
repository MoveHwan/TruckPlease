using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Animator truckAni;

    public StageData[] stageData;

    public int stage;
    public GameObject stageTruck;
    public GameObject stageWall;
    public GameObject stageCheckBox;
    public GameObject[] boxes;

    public float firstStar;
    public float secondStar;
    public float thirdStar;

    public bool gameEnd;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetStageData();
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SetStageData()
    {
        stageTruck = stageData[stage - 1].truck;
        stageWall = stageData[stage - 1].stageWall;
        stageCheckBox = stageData[stage - 1].stageCheckBox;
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
}
