using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        truckAni.SetTrigger("GameEnd");
    }

    public void GoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
