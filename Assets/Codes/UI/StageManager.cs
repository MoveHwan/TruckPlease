using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Transform Content;

    Transform StageGroup;
    int stageCount;

    void Start()
    {
        int topRatingStage = PlayerPrefs.GetInt("TopRatingStage", 0);

        for (int i = 0; i < Content.childCount; i++)
        {
            StageGroup = Content.GetChild(i).GetChild(0);

            for (int j = 0; j < StageGroup.childCount; j++)
            {
                stageCount += 1;

                StageGroup.GetChild(j).GetComponent<StageUI>().SetStage(stageCount, topRatingStage);
            }

        }


    }

}
