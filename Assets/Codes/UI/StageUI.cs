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

    public void SetStage(int Id,int topClearStage)
    {
        stageId = Id;
        StageText.text = stageId.ToString();

        if (stageId < topClearStage + 1) 
        {
            StageImage.sprite = StageClearSprite;

            starCount = PlayerPrefs.GetInt("Stage" + Id + "_Star", 0);

            for (int i = 0; i < starCount; i++)
            {
                StarGroup.GetChild(i).gameObject.SetActive(true);
            }

            StageLock.SetActive(false);
        }
        else if (stageId == topClearStage + 1)
        {
            StageLock.SetActive(false);
        }
        else
        {
            StageLock.SetActive(true);
        }
    }

    public void LoadStage()
    {
        PlayerPrefs.SetInt("Stage", stageId);

        SceneManager.LoadScene("InGame");
    }
}
