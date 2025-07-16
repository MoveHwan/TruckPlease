using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapScroller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public ScrollRect MapView;

    RectTransform target;
    Scrollbar HorBar, VerBar;

    float val, targetPosY;

    bool isDrag, isSetVal;

    Vector2 targetContentPosition;


    void Start()
    {
        HorBar = MapView.horizontalScrollbar;
        VerBar = MapView.verticalScrollbar;

        HorBar.value = 0.5f;
        VerBar.value = PlayerPrefs.GetFloat("MapScrollValue", val);
    }

    void Update()
    {
        if (target != null && MapView.content.anchoredPosition != targetContentPosition)
        {
            // 부드럽게 이동
            MapView.content.anchoredPosition = Vector2.Lerp(MapView.content.anchoredPosition, targetContentPosition, Time.deltaTime * 8);
        }

        else if (!isDrag)
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
        Canvas.ForceUpdateCanvases(); // UI 계산 강제 갱신

        RectTransform viewport = MapView.viewport;

        // 타겟의 중심을 월드 좌표로
        Vector3 worldTargetCenter = target.TransformPoint(target.rect.center);

        // Viewport 기준 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            viewport,
            worldTargetCenter,
            null,
            out Vector2 localPoint
        );

        // 타겟이 뷰포트 중앙에 오도록 content 위치 계산
        Vector2 offset = localPoint;
        targetContentPosition = MapView.content.anchoredPosition - offset;


        // 이동 가능한 최소/최대 위치 계산 (Content가 Viewport보다 클 경우만 유효)
        Vector2 minPos = Vector2.zero;
        Vector2 maxPos = new(Mathf.Max(0, MapView.content.rect.x - MapView.viewport.rect.x),Mathf.Max(0, MapView.content.rect.y - MapView.viewport.rect.y));

        // Content pivot이 (0,1)일 때: y좌표는 음수 방향으로 내려가야 함
        targetContentPosition.y = Mathf.Clamp(targetContentPosition.y, -maxPos.y, -minPos.y);
        targetContentPosition.x = Mathf.Clamp(targetContentPosition.x, -maxPos.x, -minPos.x);

        this.target = target;
    }
}
