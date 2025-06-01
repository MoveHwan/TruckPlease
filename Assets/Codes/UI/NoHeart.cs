using UnityEngine;
using DG.Tweening;

public class NoHeart : MonoBehaviour
{
    Sequence noHeartSeq;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false); // 초기에는 꺼져 있음
    }

    public void Show(Vector2 anchoredPosition)
    {
        noHeartSeq?.Kill();
        gameObject.SetActive(true);

        rectTransform.anchoredPosition = anchoredPosition;
        canvasGroup.alpha = 1f;

        noHeartSeq = DOTween.Sequence();
        noHeartSeq.Append(rectTransform.DOAnchorPosY(anchoredPosition.y + 100f, 0.4f).SetEase(Ease.OutCubic))
                  .Join(canvasGroup.DOFade(0f, 0.4f))
                  .OnComplete(() => gameObject.SetActive(false));
    }

    public void ShowOverTarget(GameObject target)
    {
        if (target.TryGetComponent<RectTransform>(out RectTransform targetRect))
        {
            transform.SetParent(target.transform.parent, false);
            Show(targetRect.anchoredPosition);
        }
    }
}
