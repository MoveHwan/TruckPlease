using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelUpPopUp : MonoBehaviour
{
    public static LevelUpPopUp Instance;

    public GameObject LevelUpView;
    public CanvasGroup PopUp;
    public CanvasGroup LevelCard;
    public TextMeshProUGUI LevelText;
    public GameObject OkButton;


    Vector3 cardDefaultVec;

    Sequence seq;

    int ADCount;

    void Awake()
    {
        Instance = this;

        cardDefaultVec = LevelCard.transform.position;
    }

    public void PopUpOn(int level)
    {
        if (LevelUpView.activeSelf)
        {
            LevelText.text = level.ToString();
            return;
        }

        ADCount++;

        LevelCard.transform.position = cardDefaultVec + Vector3.up * -200;
        LevelCard.alpha = 0;

        PopUp.transform.localScale = Vector3.zero;
        PopUp.alpha = 0;

        LevelText.text = level.ToString();

        OkButton.SetActive(false);
        LevelUpView.SetActive(true);
        
        seq = DOTween.Sequence();

        seq.Append(PopUp.transform.DOScale(1.3f, 0.4f)).SetEase(Ease.OutQuad)
            .Join(PopUp.DOFade(1, 0.4f))
            .Append(PopUp.transform.DOScale(1, 0.2f)).SetEase(Ease.InOutQuad)
            .Append(LevelCard.transform.DOMove(cardDefaultVec, 0.5f)).SetEase(Ease.OutSine)
            .Join(LevelCard.DOFade(1, 0.5f))
            .InsertCallback(0.5f, () => OkButton.SetActive(true));
    }

    public void PopUpClose()
    {
        if (ADCount >= 2)
        {
            ADCount = 0;

            GameManager.Instance.EternalAd();

            InGameRemoveAds.instance.RemoveAdsOn();
        }

        LevelUpView.SetActive(false);
    }

}
