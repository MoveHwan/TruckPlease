using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryBox : MonoBehaviour
{
    public RectTransform boxUI;

    static bool isMove;

    int dir, count;

    Vector2 dis;

    void OnEnable()
    {
        CanvasGroup boxCanvas = boxUI.GetComponent<CanvasGroup>();

        if (boxCanvas == null)
            boxCanvas = boxUI.AddComponent<CanvasGroup>();

        boxUI.localScale = Vector2.zero;
        boxCanvas.alpha = 0;

        boxUI.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
        boxCanvas.DOFade(1f, 0.3f)
            .OnComplete(() => 
            {
                Vector2 start = boxUI.anchoredPosition + Vector2.up * dis.y;
                Vector2 end = Vector2.right * (-dir * dis.x + start.x) + Vector2.up * start.y;

                // 포물선 이동 중간 점 구하기 (y축만 높임)
                Vector2 control = new((start.x - end.x) / 2f, start.y);

                // 포물선 이동을 위해 0~1까지 Lerp
                float t = 0f;

                // 포물선 이동
                DOTween.To(() => t, val =>
                {
                    t = val;
                    Vector2 pos = CalculateParabola(start, control, end, t);
                    boxUI.anchoredPosition = pos;

                }, 1f, 0.3f).OnComplete(() => {
                    // 바닥에 '쿵' 떨어지는 연출
                    boxUI.DOAnchorPosY(boxUI.anchoredPosition.y - 30f, 0.25f).SetEase(Ease.OutBounce)
                    .OnComplete(() => isMove = false);
                });
            }
       );
    }

    // 베지어 포물선 계산
    Vector2 CalculateParabola(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

    public void BoxMoveOn(int dir, Vector2 dis)
    {
        isMove = true;

        this.dir = dir;
        this.dis = dis;

        gameObject.SetActive(true);
    }

    public bool MoveCheck() => isMove;
}
