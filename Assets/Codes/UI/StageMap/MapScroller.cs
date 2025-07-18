using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapScroller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ScrollRect MapView;

    public bool isMove;

    RectTransform content;
    RectTransform viewport;

    Scrollbar HorBar, VerBar;

    float val;

    bool isDrag, isSetVal;


    void Start()
    {
        content = MapView.content;
        viewport = MapView.viewport;

        HorBar = MapView.horizontalScrollbar;
        VerBar = MapView.verticalScrollbar;

        HorBar.value = 0.5f;
        VerBar.value = PlayerPrefs.GetFloat("MapScrollValue", val);
    }

    void Update()
    {
        if (!isDrag)
        {
            if (!isSetVal && val == VerBar.value)
            {
                SetScrollValue();
            }
            else if (val != VerBar.value)
            {
                val = VerBar.value;
            }

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        isSetVal = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }

    void SetScrollValue()
    {
        isSetVal = true;

        val = VerBar.value;

        if (val <= 0) val = 0;

        PlayerPrefs.SetFloat("MapScrollValue", val);

        Debug.Log("MapScrollValue: " + val);
    }


    public void ScrollToTarget(RectTransform target)
    {
        // Ensure target is under the content
        target.SetParent(content);
        Debug.LogWarning($"[ScrollToTarget] Target: {target.name}");

        isMove = true;

        // Force layout update
        Canvas.ForceUpdateCanvases();

        // Get target center position (anchoredPosition ±âÁØ)
        Vector2 targetCenter = target.anchoredPosition + (target.rect.size * 0.5f);

        // Calculate offsets from viewport center
        Vector2 offset = targetCenter - (viewport.rect.size * 0.5f);

        // Calculate scrollable size
        Vector2 scrollable = content.rect.size - viewport.rect.size;

        // Clamp to [0,1] normalized range
        float verticalNormalized = Mathf.Clamp01(offset.y / scrollable.y);
        float horizontalNormalized = 0;

        float width = content.rect.size.x / 4;

        if (targetCenter.x >= width)
            horizontalNormalized = 1;
        else if (targetCenter.x <= -width)
            horizontalNormalized = 0;
        else
            horizontalNormalized = 0.5f;

        // Tween both directions
        DOTween.Sequence()
            .Append(DOTween.To(() => MapView.verticalNormalizedPosition,
                               v => MapView.verticalNormalizedPosition = v,
                               verticalNormalized, 0.4f).SetEase(Ease.OutCubic))
            .Join(DOTween.To(() => MapView.horizontalNormalizedPosition,
                             h => MapView.horizontalNormalizedPosition = h,
                             horizontalNormalized, 0.4f).SetEase(Ease.OutCubic))
            .AppendInterval(0.2f)
            .AppendCallback(() => isMove = false);
    }


}
