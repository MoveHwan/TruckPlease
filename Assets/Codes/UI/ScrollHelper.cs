using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollHelper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform TargetGroup;
    public Transform PageNavi;

    [SerializeField] float scrollVelocity = 0f;
    [SerializeField] float targetPos;

    public int targetIdx;


    float[] pos;

    int size;
    float distance;

    bool isDrag;


    void Awake()
    {
        size = TargetGroup.childCount;

        pos = new float[size];

        distance = 1f / (size - 1);
        for (int i = 0; i < size; i++) pos[i] = distance * i;
        
        targetPos = pos[0];
    }

    private void OnEnable()
    {
        scrollbar.value = pos[0];
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        if (targetIdx < size - 1 && scrollbar.value - pos[targetIdx] > distance * 0.2f)
        {
            targetPos = pos[++targetIdx];

            SetPageNavi();
        }
        else if (targetIdx > 0 && scrollbar.value - pos[targetIdx] < -distance * 0.2f)
        {
            targetPos = pos[--targetIdx];

            SetPageNavi();
        }


        /*// 스크롤을 특정 화면으로 고정
        for (int i = 0; i < size; i++)
        {
            if (scrollbar.value - pos[i] > 0)
            {
                targetIdx = i;
                targetPos = pos[i];

                SetPageNavi();
            }
            else if (scrollbar.value - pos[i] < 0 && scrollbar.value < pos[i] + distance * 0.8f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIdx = i;
                targetPos = pos[i];

                SetPageNavi();
            }
        }*/

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
    }

    void SetPageNavi()
    {
        for (int i = 0; i < size; i++)
        {
            PageNavi.GetChild(i).GetChild(0).gameObject.SetActive(i == targetIdx);
        }
    }

    public void PrevTarget()
    {
        if (targetIdx - 1 < 0) return;

        targetPos = pos[--targetIdx];
    }

    public void NextTarget()
    {
        if (targetIdx + 1 >= size) return;

        targetPos = pos[++targetIdx];
    }
}
