using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public MapScroller MapScroller;
    public GameObject NoHeartPopUp;
    public Transform[] ChapterLandMarks;
    public Transform[] Stages;

    [SerializeField] Landmark[] landmark;
    [SerializeField] StageUI targetStage;
    [SerializeField] int totalClearStage;

    public bool stageShowEnd, landShowEnd;

    int stageCount, totalStarCount, landIdx;

    bool setStage;

    static bool stageCheck;

    void Awake()
    {
        instance = this;

#if UNITY_EDITOR
        if (!stageCheck && SceneManager.GetActiveScene().name == "Lobby")
            stageCheck = true;
#else
        if (!stageCheck)
            stageCheck = true;
#endif
    }


    void Start()
    {
        landmark = new Landmark[ChapterLandMarks.Length];

        float starCount;

        for (int i = 0; i < ChapterLandMarks.Length; i++)
        {
            StageUI stage;
            starCount = 0;

            for (int j = 0; j < Stages[i].childCount; j++)
            {
                stage = Stages[i].GetChild(j).GetComponent<StageUI>();

                int star = stage.SetStage(++stageCount);

                starCount += star;

                if (star > 0)
                    totalClearStage++;
                else if (!setStage)
                {
                    setStage = true;
                    targetStage = stage;
                }

            }

            totalStarCount += (int)starCount;

            for (int j = 0;j < ChapterLandMarks[i].childCount; j++)
            {
                landmark[i] = ChapterLandMarks[i].GetChild(j).GetComponent<Landmark>();

                if (starCount >= 6)
                {
                    landmark[i].Lock.SetActive(false);
                    starCount -= 6;
                }
                else
                {
                    if (j + 1 < ChapterLandMarks[i].childCount)
                        landmark[i].NextLandmark = ChapterLandMarks[i].GetChild(j+1).GetComponent<Landmark>();

                    landmark[i].SetStarSlider(starCount);
                    break;
                }
                   
            }
        }

        if (Profile.Instance != null)
            Profile.Instance.SetTotalStat(totalStarCount, PlayerPrefs.GetInt("TopStage", 0));
        else
            transform.parent.parent.gameObject.SetActive(false);

    }

    public void SetLandmark()
    {
        int nowStage = PlayerPrefs.GetInt("Stage", 1);
        
        if (stageCheck)
        {
            StageUI stage = null;

            for (int i = 0; i<Stages.Length; i++)
            {
                for (int j = 0; j < Stages[i].childCount; j++)
                {
                    stage = Stages[i].GetChild(j).GetComponent<StageUI>();

                    if (stage.stageId == nowStage)
                    {
                        targetStage = stage;
                        break;
                    }
                }

                if (stage != null && stage.stageId == nowStage)
                    break;
            }
        }

        landIdx = targetStage.stageId;

        if (landIdx % 6 == 0)
            landIdx = landIdx / 6 - 1;
        else
            landIdx = landIdx / 6;

        PlayerTruck.Instance.SetPlayerTruck(targetStage.isLeft, targetStage.GetComponent<RectTransform>());

        Debug.LogWarning("<StageManager> Stage: " + targetStage.stageId + ", landIdx: " + landIdx);
    }

    public void StageMapView(int clearStar)
    {
        if (clearStar <= targetStage.starCount)
        {
            StageTruckCanvas.Instance.ResultSeqPlay();
            return;
        }
        
        StartCoroutine(MapViewCoroutine(clearStar));
    }

    IEnumerator MapViewCoroutine(int clearStar)
    {
        targetStage.ClearStage(clearStar);

        yield return new WaitUntil(() => stageShowEnd);
        yield return new WaitForSeconds(0.2f);

        landmark[landIdx].StarSliderShow(clearStar - targetStage.starCount);

        yield return new WaitUntil(() => landShowEnd);
        yield return new WaitForSeconds(0.2f);

        StageTruckCanvas.Instance.ResultSeqPlay();

        yield break;
    }

    public bool EditorStageCheck() => stageCheck;
}
