using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapScroller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Scrollbar MapScrollbar;

    float val;

    bool isDrag, isSetVal;

    
    void Start()
    {
        MapScrollbar.value = PlayerPrefs.GetFloat("MapScrollValue", val);
    }

    void Update()
    {
        if (!isDrag)
        {
            if (!isSetVal && val == MapScrollbar.value)
            {
                SetScrollValue();
            }
            else if (val != MapScrollbar.value)
            {
                val = MapScrollbar.value;
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

        val = MapScrollbar.value;

        if (val <= 0) val = 0;

        PlayerPrefs.SetFloat("MapScrollValue", val);

        Debug.Log("MapScrollValue: " + val);
    }

}
