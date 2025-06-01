using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollHelper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public static ScrollHelper instance;

    public Scrollbar scrollbar;
    public Transform TargetGroup;
    public NewChapter NewChapter;

    [SerializeField] float targetPos;

    public int targetIdx;

    Coroutine coroutine;

    float[] pos;

    int size, newChapIdx;
    float distance;

    bool isDrag, isUnlock;


    void Awake()
    {
        instance = this;

        size = TargetGroup.childCount;

        pos = new float[size];

        distance = 1f / (size - 1);
        for (int i = 0; i < size; i++) pos[i] = distance * i;
    }

    void Start()
    {
        int idx = PlayerPrefs.GetInt("Chapter_Idx", 0);

        targetPos = pos[idx];
        scrollbar.value = pos[idx];
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (targetIdx < size - 1 && scrollbar.value - pos[targetIdx] > distance * 0.2f)
        {
            targetPos = pos[++targetIdx];
        }
        else if (targetIdx > 0 && scrollbar.value - pos[targetIdx] < -distance * 0.2f)
        {
            targetPos = pos[--targetIdx];
        }

        PlayerPrefs.SetInt("Chapter_Idx", targetIdx);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DragEndDelay());
    }

    void Update()
    {
        if (!isUnlock && NewChapter)
        {
            isUnlock = true;

            newChapIdx = ++targetIdx;

            targetPos = pos[newChapIdx];
        }

        if (!isDrag)
        {
            if (scrollbar.value >= targetPos - 0.001f && scrollbar.value <= targetPos + 0.001f)
            {
                scrollbar.value = targetPos;

                if (isUnlock && targetPos == pos[newChapIdx])
                {
                    NewChapter.PlayUnlockSeq();
                    NewChapter = null;
                    isUnlock = false;
                }
                    
            }
            else
                scrollbar.value = Mathf.MoveTowards(scrollbar.value, targetPos, Time.deltaTime * 0.6f);

        }

        
    }

    IEnumerator DragEndDelay()
    {
        yield return new WaitForSeconds(0.15f);

        isDrag = false;
    }
    
    public void ChapterClick(int chap)
    {
        targetPos = pos[chap-1];
        scrollbar.value = targetPos;
    }
}
