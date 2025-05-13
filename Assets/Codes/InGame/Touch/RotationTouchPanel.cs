using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationTouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static RotationTouchPanel instance;

    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    public RotationBox rotationBox;


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

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastTouchPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || rotationBox == null)
            return;

        Vector2 delta = eventData.position - lastTouchPosition;
        rotationBox.RotateObject(delta);
        lastTouchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
