using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    public GameObject StageLock;
    public TextMeshProUGUI StageText;
    public Transform Content;
    public Transform StarGroup;
    public StageUI NextStage;

    public bool isLeft;

    [Space]
    public int stageId;
    public int starCount;

    bool isPrevActive;

    public int SetStage(int id)
    {
        stageId = id;
        starCount = PlayerPrefs.GetInt("Stage" + stageId + "_star", 0);
        isPrevActive = PlayerPrefs.GetInt("Stage" + (stageId - 1) + "_star", 0) > 0 || stageId == 1;

        StageText.text = stageId.ToString();
        StageLock.SetActive(stageId != 1 && !isPrevActive);

        for (int i = 0; i < starCount; i++)
        {
            StarGroup.GetChild(i).gameObject.SetActive(true);
        }

        if (isPrevActive && starCount <= 0 || id == 1 && starCount <= 0)
        {
            PlayerTruck.Instance.SetPlayerTruck(isLeft, gameObject.GetComponent<RectTransform>());
        }

        return starCount;
    }

    public void ClearStage(int star)
    {
        StartCoroutine(ClearStageCoroutine(star));
    }

    IEnumerator ClearStageCoroutine(int star)
    {
        StageManager.instance.MapScroller.ScrollToTarget(gameObject.GetComponent<RectTransform>());

        yield return new WaitUntil(() => StageManager.instance.MapScroller.isMove == false);
        yield return new WaitForSeconds(0.3f);


        CanvasGroup stageCvg = StageText.transform.parent.GetComponent<CanvasGroup>();

        if (stageCvg == null)
            stageCvg = StageText.transform.parent.AddComponent<CanvasGroup>();

        stageCvg.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack);
        stageCvg.DOFade(0, 0.3f);

        yield return new WaitForSeconds(0.2f);

        PlayerTruck.Instance.DeliveryBoxOn();

        yield return new WaitUntil(() => PlayerTruck.Instance.moveEnd);


        for (int i = 0; i < star; i++)
        {
            if (StarGroup.GetChild(i).gameObject.activeSelf) continue;

            StarGroup.GetChild(i).transform.localScale = Vector3.zero;
            StarGroup.GetChild(i).gameObject.SetActive(true);
            StarGroup.GetChild(i).transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(0.15f);
        }

        StageManager.instance.stageShowEnd = true;
    }

    public void NextOpen()
    {
        StartCoroutine(NextOpenCoroutine());
    }

    IEnumerator NextOpenCoroutine()
    {
        StageManager.instance.MapScroller.ScrollToTarget(gameObject.GetComponent<RectTransform>());

        yield return new WaitUntil(() => StageManager.instance.MapScroller.isMove == false);
        yield return new WaitForSeconds(0.3f);


        CanvasGroup lockCvg = StageLock.GetComponent<CanvasGroup>();

        if (lockCvg == null)
            lockCvg = StageLock.AddComponent<CanvasGroup>();

        lockCvg.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        lockCvg.DOFade(0, 0.3f);

        yield return new WaitForSeconds(0.3f);

        StageManager.instance.nextShowEnd = true;
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
        Debug.LogWarning("Stage: " + stageId);

        AudioManager.instance.StopBGM();

        SceneManager.LoadScene("LoadingScene");
    }
}
