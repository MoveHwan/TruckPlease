using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationTouchPanel : MonoBehaviour
{
    public static RotationTouchPanel instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    dragStarted = true;
    //    isPressing = true;
    //    dragStartPos = eventData.position;
    //    dragStartTime = Time.time;
    //    throwHeightSli.value = 0f;
    //    Debug.Log("Pointer Down: " + dragStartPos);

    //    isDragging = true;
    //}


}
