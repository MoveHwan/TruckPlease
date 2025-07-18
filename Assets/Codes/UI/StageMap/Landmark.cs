using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Landmark : MonoBehaviour
{
    public GameObject Lock;
    public Slider StarSlider;
    public TextMeshProUGUI SliderText;
    public Landmark NextLandmark;

    CanvasGroup LockCvG;
    Sequence UnlockSeq;

    [SerializeField] int totalStar, newStar;
    [SerializeField] float targetVal;
    [SerializeField] bool sliderOn;

    void Update()
    {
        if (sliderOn)
        {
            if (StarSlider.value >= targetVal - 0.005f)
            {
                sliderOn = false;

                if (targetVal >= 1)
                    Unlock();
                else
                    StageManager.instance.landShowEnd = true;
            }

            StarSlider.value = Mathf.Lerp(StarSlider.value, targetVal, Time.deltaTime * 8);
        }
    }

    public void SetStarSlider(float star)
    {
        totalStar = (int)star;

        StarSlider.value = star / 6;
        SliderText.text = star + "/6";
    }

    public void StarSliderShow(float getStar)
    {
        newStar = (int)getStar;

        targetVal = Mathf.Clamp01((totalStar + getStar) / 6);

        StartCoroutine(WaitMapMove());
    }


    void Unlock()
    {
        LockCvG = Lock.GetComponent<CanvasGroup>();

        if (LockCvG == null)
            LockCvG = Lock.AddComponent<CanvasGroup>();

        UnlockSeq = DOTween.Sequence();

        UnlockSeq.AppendCallback(() =>
        {
            Lock.SetActive(true);
            LockCvG.alpha = 1;
        });

        UnlockSeq
            .Append(LockCvG.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutBack))
            .Append(LockCvG.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
            .Join(LockCvG.DOFade(0, 0.5f));

        UnlockSeq.AppendCallback(() =>
        {
            Lock.SetActive(false);

            if (totalStar > 0)
                NextLandmark.StarSliderShow(totalStar);
            else
                StageManager.instance.landShowEnd = true;

            DOTween.Kill(UnlockSeq);
        });
    }

    IEnumerator WaitMapMove()
    {
        //transform.SetParent(transform.parent.parent.parent);

        StageManager.instance.MapScroller.ScrollToTarget(gameObject.GetComponent<RectTransform>());

        yield return new WaitUntil(() => StageManager.instance.MapScroller.isMove == false);
        yield return new WaitForSeconds(0.3f);

        sliderOn = true;

        int targetValue = totalStar + newStar >= 6 ? 6 : totalStar + newStar;

        DOVirtual.Int(totalStar, targetValue, 0.3f, value =>
        {
            SliderText.text = value + "/6";
        });

        totalStar = totalStar + newStar - 6;

        yield break;
    }
}
