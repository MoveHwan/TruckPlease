using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBox : MonoBehaviour
{
    float rotationSpeed = 10f; // 회전 속도 설정
    private Vector2 lastTouchPosition; // Vector2로 수정
    private bool isDragging = false;
    public bool throwDone;
    public bool vfxDone;

    void Update()
    {
        if (throwDone)
            return;
        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y < Screen.height * 0.3f)
            {
                lastTouchPosition = Input.mousePosition;
                isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            
            Vector2 deltaPosition = (Vector2)Input.mousePosition - lastTouchPosition;
            RotateObject(deltaPosition);
            lastTouchPosition = Input.mousePosition;
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.y < Screen.height * 0.3f)
                {
                    lastTouchPosition = touch.position;
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                
                Vector2 deltaPosition = touch.position - lastTouchPosition;
                RotateObject(deltaPosition);
                lastTouchPosition = touch.position;
                
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (Input.touchCount == 2)
            {
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);

                // 두 터치 간의 벡터
                Vector2 prevPos0 = t0.position - t0.deltaPosition;
                Vector2 prevPos1 = t1.position - t1.deltaPosition;

                Vector2 prevVector = prevPos1 - prevPos0;
                Vector2 currVector = t1.position - t0.position;

                // 이전 프레임과 현재 프레임의 각도 차이를 계산
                float angle = Vector2.SignedAngle(prevVector, currVector);

                // 오브젝트 회전 (Y축 기준으로 회전 예시)
                transform.Rotate(Vector3.forward, angle * rotationSpeed * rotationSpeed * Time.deltaTime, Space.World);

            }

        }
    }

    void RotateObject(Vector2 delta)
    {
        float rotationX = delta.x * rotationSpeed * Time.deltaTime;
        float rotationY = delta.y * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, -rotationX, Space.World);
        transform.Rotate(Vector3.right, rotationY, Space.World);
    }

}
