using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollHelper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] ScrollHelper parentScrollHelper;
    public ScrollHelper OtherHelper;

    int size;
    float distance;

    float[] pos;

    [SerializeField] float scrollVelocity = 0f;
    [SerializeField] float targetPos;

    bool isDrag, passParent;

    public int targetIdx;

    public Scrollbar scrollbar;

    void Awake()
    {
        if (scrollbar.name == "Scrollbar Vertical")
            size = transform.GetChild(0).GetChild(0).childCount;
        else
            size = 2;

        pos = new float[size];

        distance = 1f / (size - 1);
        for (int i = 0; i < size; i++)
        {
            if (scrollbar.name == "Scrollbar Vertical")
            {
                pos[size - 1 - i] = distance * i;
            }
            else
                pos[i] = distance * i;
        }
            
           
        targetPos = pos[0];
    }

    private void OnEnable()
    {
        scrollbar.value = pos[0];
        
        parentScrollHelper = transform.parent.GetComponentInParent<ScrollHelper>();
        if (parentScrollHelper == this) parentScrollHelper = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 방향 체크하여 부모로 넘길지 여부 결정
        Vector2 delta = eventData.position - eventData.pressPosition;

        passParent = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);

        if (passParent && parentScrollHelper != null)
        {
            parentScrollHelper.OnBeginDrag(eventData);  // 가로로 드래그가 우선
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;

        if (passParent && parentScrollHelper != null)
        {
            // 부모 스크롤뷰로 이벤트 전파
            parentScrollHelper.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        if (passParent && parentScrollHelper != null)
        {
            // 부모 스크롤뷰로 이벤트 전파
            parentScrollHelper.OnEndDrag(eventData);
        }
        else
        {
            // 스크롤을 특정 화면으로 고정
            for (int i = 0; i < size; i++)
            {
                if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
                {
                    targetIdx = i;
                    targetPos = pos[i];
                }
            }
        }

        passParent = false;
    }

    void Update()
    {
        if (!isDrag)
        {
            

            if (scrollbar.value >= targetPos - 0.001f && scrollbar.value <= targetPos + 0.001f)
            {
                scrollbar.value = targetPos;
                scrollVelocity = 0;
            }
            else
                scrollbar.value = Mathf.SmoothDamp(scrollbar.value, targetPos, ref scrollVelocity, 0.15f);

        }
        else
        {
            if (scrollbar.name == "Scrollbar Vertical" && scrollbar.value > targetPos)
            {
                scrollbar.value = targetPos;
            }
        }
    }

    public void PrevTarget()
    {
        if (targetIdx - 1 < 0 || scrollbar.name == "Scrollbar Vertical") return;

        targetPos = pos[--targetIdx];
    }

    public void NextTarget()
    {
        if (targetIdx + 1 >= size) return;

        targetPos = pos[++targetIdx];
    }
}
