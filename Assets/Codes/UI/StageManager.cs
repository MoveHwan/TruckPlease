using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public GameObject StagePopUp;
    public GameObject ChapterPopUp;
    public GameObject NoHeartPopUp;
    public Transform[] Chapters;
    public Transform[] Stages;

    int stageCount, totalStarCount;

    [SerializeField] int totalClearStage;

    void Awake()
    {
        instance = this;

        //PlayerPrefs.SetInt("Chapter 2_new", 0);
    }

    void Start()
    {
        string topRatingStage = PlayerPrefs.GetString("TopRatingStage", "1_0");
        string[] ratingStr = topRatingStage.Split('_');

        int topChapter = int.Parse(ratingStr[0]);
        float topStage = int.Parse(ratingStr[1]);

        float starCount;

        for (int i = 0; i < Chapters.Length; i++)
        {
            starCount = 0;

            if (i != 0)
            {
                if (topChapter + 1 <= i || (topChapter == i && topStage < 9))
                {
                    Chapters[i].GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    Chapters[i].GetChild(4).GetComponent<NewChapter>().NewOn();
                }
                
            }

            Transform stagesPar = Stages[i].GetChild(0);

            for (int j = 0; j < stagesPar.childCount; j++)
            {
                int star = stagesPar.GetChild(j).GetComponent<StageUI>().SetStage(++stageCount);

                starCount += star;

                if (star > 0)
                    totalClearStage++;
            }

            Chapters[i].GetChild(2).GetComponent<Slider>().value = starCount / (stagesPar.childCount * 3);
            Chapters[i].GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = starCount + "/" + stagesPar.childCount * 3;

            totalStarCount += (int)starCount;
        }

        if (PlayerPrefs.GetInt("NewChapter", 0) == 1)
        {
            PlayerPrefs.SetInt("NewChapter", 0);
            ChapterPopUp.SetActive(true);
        }

        Profile.Instance.SetTotalStat(totalStarCount, totalClearStage);
    }

    public void OpenChapter(int chapter)
    {
        for (int i = 0; i < Stages.Length; i++)
        {
            Stages[i].gameObject.SetActive( chapter - 1 == i );
        }

        StagePopUp.SetActive(true);

        ScrollHelper.instance.ChapterClick(chapter);
    }

    
}
