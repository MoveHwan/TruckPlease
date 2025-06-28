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
        // (0, 0): �� �Ʒ�
        // (0, 1): �� ��
        // (1, 1): ������ �� (����/���� �� �� ���� ��)
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
                // ���⿡ ������ �ڵ� �ۼ�
                Debug.Log("������ �� �Ϸ�!");
                StartCoroutine(ScrollToTop());
            });

    }
}
