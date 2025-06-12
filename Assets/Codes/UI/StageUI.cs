using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public GameObject StageLock;
    public TextMeshProUGUI StageText;

    public Transform StarGroup;

    public Image StageImage;
    public Sprite StageClearSprite;

    [Space]
    [SerializeField] int stageId;
    [SerializeField] int starCount;

    public int SetStage(int id)
    {
        stageId = id;

        int StageNum = int.Parse(StageText.text);

        if (StageNum == 1)
        {
            if (stageId != 1)
            {
                starCount = PlayerPrefs.GetInt("Stage" + (stageId - 4) + "_Star", 0);

                if (starCount == 0)
                    StageLock.SetActive(true);
                else
                    StageLock.SetActive(false);
            }
            else
                StageLock.SetActive(false);

        }
        else
        {
            starCount = PlayerPrefs.GetInt("Stage" + (stageId - 1) + "_Star", 0);

            if (starCount == 0)
                StageLock.SetActive(true);
            else
                StageLock.SetActive(false);
        }

        

        starCount = PlayerPrefs.GetInt("Stage" + stageId + "_Star", 0);

        for (int i = 0; i < starCount; i++)
        {
            if (i == 0)
                StageImage.sprite = StageClearSprite;

            StarGroup.GetChild(i).gameObject.SetActive(true);
        }

        return starCount;
    }

    public void LoadStage()
    {
        if (!FatigueManager.instance.CheckFatigue())
        {
            StageManager.instance.NoHeartPopUp.SetActive(true);
            return;
        }

        FatigueManager.instance.StageIn();

        PlayerPrefs.SetInt("Stage", stageId);

        AudioManager.instance.StopBGM();

        SceneManager.LoadScene("LoadingScene");
    }
}
