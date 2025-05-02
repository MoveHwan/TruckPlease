using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Animator truckAni;

    public StageData[] stageData;

    public int stage;
    public GameObject stageTruck;
    public GameObject stageWall;
    public GameObject[] boxes;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetStageData();
        truckAni = GameObject.FindWithTag("Truck").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameEnd()
    {
        truckAni.SetTrigger("GameEnd");
    }

    void SetStageData()
    {
        stageTruck = stageData[stage - 1].truck;
        stageWall = stageData[stage - 1].stageWall;
        boxes = stageData[stage - 1].boxes;
        Instantiate(stageTruck);
        Instantiate(stageWall);
    }
}
