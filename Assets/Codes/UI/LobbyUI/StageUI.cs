using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    public GameObject StageLock;
    public TextMeshProUGUI StageText;
    public Transform Content;
    public Transform StarGroup;

    public bool isSymmetry;
    public Vector2 truckVec;

    [Space]
    [SerializeField] int stageId;
    [SerializeField] int starCount;


    void Start()
    {
        //truckVec = 
    }

    public int SetStage(int id)
    {
        stageId = id;

        StageText.text = stageId.ToString();

        StageLock.SetActive(stageId != 1 && PlayerPrefs.GetInt("Stage" + (stageId - 1) + "_Star", 0) <= 0);

        starCount = PlayerPrefs.GetInt("Stage" + stageId + "_Star", 0);

        for (int i = 0; i < starCount; i++)
        {
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
