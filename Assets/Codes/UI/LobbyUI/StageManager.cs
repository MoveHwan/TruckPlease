using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public GameObject StagePopUp;
    public GameObject ChapterPopUp;
    public GameObject NoHeartPopUp;
    public Transform[] ChapterLandMarks;
    public Transform[] Stages;

    int stageCount, totalStarCount;

    [SerializeField] int totalClearStage;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        float starCount;

        for (int i = 0; i < ChapterLandMarks.Length; i++)
        {
            starCount = 0;

            for (int j = 0; j < Stages[i].childCount; j++)
            {
                int star = Stages[i].GetChild(j).GetComponent<StageUI>().SetStage(++stageCount);

                starCount += star;

                if (star > 0)
                    totalClearStage++;
            }

            totalStarCount += (int)starCount;

            for (int j = 0;j < ChapterLandMarks[i].childCount; j++)
            {
                if (starCount >= 6)
                {
                    ChapterLandMarks[i].GetChild(j).GetChild(0).gameObject.SetActive(false);
                    starCount -= 6;
                }
                else
                {
                    ChapterLandMarks[i].GetChild(j).GetChild(0).GetChild(0).GetComponent<Slider>().value = starCount / 6;
                    ChapterLandMarks[i].GetChild(j).GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = starCount + "/6";
                    break;
                }
                   
            }
        }

        /*if (PlayerPrefs.GetInt("NewChapter", 0) == 1)
        {
            PlayerPrefs.SetInt("NewChapter", 0);
            ChapterPopUp.SetActive(true);
        }*/

        Profile.Instance.SetTotalStat(totalStarCount, PlayerPrefs.GetInt("TopStage", 0));
    }



    /*public void OpenChapter(int chapter)
   {
       for (int i = 0; i < Stages.Length; i++)
       {
           Stages[i].gameObject.SetActive( chapter - 1 == i );
       }

       StagePopUp.SetActive(true);

       ScrollHelper.instance.ChapterClick(chapter);
   }*/
}
