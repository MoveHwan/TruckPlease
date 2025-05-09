using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInGoal : MonoBehaviour
{
    public GameObject[] boxes;

    void Start()
    {
        InGoalObstacleBox();
    }

    void InGoalObstacleBox()
    {
        foreach (var box in boxes) 
        { 
            BoxManager.Instance.GoaledBoxes.Add(box);
        }

        GameManager.Instance.obstacleReady = true;
        GameManager.Instance.ReadyToStart();
    }
}
