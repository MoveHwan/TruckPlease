using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndRankCon : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform myFrame;

    void Start()
    {
        // (0, 0): 맨 아래
        // (0, 1): 맨 위
        // (1, 1): 오른쪽 위 (수평/수직 둘 다 있을 때)
        StartCoroutine(MyFrameScaleUp());
        //StartCoroutine(ScrollToTop());
    }

    IEnumerator ScrollToTop()
    {
        yield return new WaitForSeconds(0.1f);
        scrollRect.DONormalizedPos(new Vector2(0, 1), 2f).SetEase(Ease.OutCubic);
    }

    IEnumerator MyFrameScaleUp()
    {
        yield return new WaitForSeconds(1f);
        myFrame.DOScale(1.07f,2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                // 여기에 마무리 코드 작성
                Debug.Log("스케일 업 완료!");
                StartCoroutine(ScrollToTop());
            });

    }
}
