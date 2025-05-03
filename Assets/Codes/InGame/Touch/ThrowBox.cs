using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBox : MonoBehaviour
{
    RotationBox rotationBox;

    public BoxData boxData;
    public float weight;   //  상자 무게

    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private float dragStartTime;
    private float dragEndTime;
    bool dragStarted;
    bool throwDone;

    private Rigidbody rb;
    public float forceMultiplier = 5f;  // 전체 힘 조정
    public float zDirection=1f;
    private float screenHeight;
    private float screenWidth;

    void Awake()
    {
        rotationBox = GetComponent<RotationBox>();
    }

    void Start()
    {
        SetWeight();
        screenWidth = Screen.width;
        screenHeight = Screen.height; // 화면 높이
    }

    void Update()
    {
        if (throwDone)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    // 마우스 입력 처리
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시작
        {
            if (IsInTopScreen(Input.mousePosition)) // 화면 위쪽 70% 내에서만
            {
                dragStarted = true;
                dragStartPos = Input.mousePosition;
                dragStartTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제
        {
            if (IsInTopScreen(Input.mousePosition) && dragStarted) // 화면 위쪽 70% 내에서만
            {
                dragEndPos = Input.mousePosition;
                dragEndTime = Time.time;

                ThrowObject();
            }
        }
    }

    // 터치 입력 처리
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsInTopScreen(touch.position)) // 화면 위쪽 70% 내에서만
                {
                    dragStartPos = touch.position;
                    dragStartTime = Time.time;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (IsInTopScreen(touch.position)) // 화면 위쪽 70% 내에서만
                {
                    dragEndPos = touch.position;
                    dragEndTime = Time.time;

                    ThrowObject();
                }
            }
        }
    }

    // 화면 위쪽 70% 안에 있는지 확인하는 함수
    bool IsInTopScreen(Vector2 position)
    {
        return position.y >= screenHeight * 0.3f; // 화면 높이의 30% 이상, 즉 위쪽 70% 내
    }

    // 물체 던지기
    void ThrowObject()
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        Vector2 dragVector = dragEndPos - dragStartPos;
        float dragDuration = dragEndTime - dragStartTime;
        Debug.Log(dragDuration);
        Debug.Log(dragVector);
        float clampedDuration = Mathf.Clamp(dragDuration, 0.5f, 3f);
        float mappedDuration = Mathf.Lerp(1f, 3f, (clampedDuration - 0.5f) / (3f - 0.5f));

        // 1. 드래그 거리 정규화 (해상도 기준 비율)
        float normalizedDragY = dragVector.y / (screenHeight * 0.7f);
        // 3. 힘 계산: 정규화된 드래그 × 보정값
        float throwForce = normalizedDragY;
        throwForce = Mathf.Lerp(0.3f, 1f, throwForce);
        normalizedDragY = Mathf.Lerp(1f, 3f, Mathf.Clamp01(normalizedDragY));

        // x방향 설정
        float normalizedDragX = dragVector.x / screenWidth;
        Debug.Log("normalizedDragY: " + normalizedDragY);
        Debug.Log("normalizedDragX : " + normalizedDragX);
        
        // 2. 방향 계산
        Vector3 throwDirection = new Vector3(normalizedDragX, mappedDuration, zDirection).normalized;
        //throwDirection.y += 0.5f;
        Debug.Log(throwForce);

        // 4. 힘 적용
        rb.AddForce(throwDirection * throwForce * forceMultiplier, ForceMode.Impulse);
        dragStarted = false;


        StartCoroutine(ThrowFinish());
    }

    IEnumerator ThrowFinish()
    {
        throwDone = true;
        rotationBox.throwDone = true;
        BoxManager.Instance.RemainBoxCal(gameObject);
        yield return new WaitForSeconds(0.5f);
        
        BoxManager.Instance.NextBoxSpawn();
    }

    void SetWeight()
    {
        weight = boxData.Weight;
    }
}
