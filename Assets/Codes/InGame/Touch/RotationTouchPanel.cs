using UnityEngine;
using UnityEngine.EventSystems;

public class RotateTouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static RotateTouchPanel instance;

    public RotationBox targetBox;
    private bool isDragging = false;
    private Vector2 lastTouchPos;

    [SerializeField] private float rotationSpeed = 0.2f;

    void Awake()
    {
        instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetBox == null || targetBox.throwDone)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out lastTouchPos
        );

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || targetBox == null || targetBox.throwDone)
            return;

        Vector2 currentPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out currentPos
        );

        Vector2 delta = currentPos - lastTouchPos;
        lastTouchPos = currentPos;

        // 회전 방향: X는 Y축 회전, Y는 X축 회전
        targetBox.RotateByInput(delta * rotationSpeed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
