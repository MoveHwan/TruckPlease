using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBox : MonoBehaviour
{
    public float rotationSpeed = 100f; // ȸ�� �ӵ� ����
    private Vector2 lastTouchPosition; // Vector2�� ����
    private bool isDragging = false;
    public bool throwDone;

    void Start()
    {
        if (!throwDone)
        {
            RotationTouchPanel.instance.rotationBox = this;
        }
    }

    public void RotateObject(Vector2 delta)
    {
        float rotationX = delta.x * rotationSpeed * Time.deltaTime;
        float rotationY = delta.y * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, -rotationX, Space.World);
        transform.Rotate(Vector3.right, rotationY, Space.World);
    }
}
