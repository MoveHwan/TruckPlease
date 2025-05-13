using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrowBox : MonoBehaviour
{
    RotationBox rotationBox;
    RectTransform touchAreaUI;

    public BoxData boxData;
    public float weight;   //  상자 무게
    float rbWeight;

    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private float dragStartTime;
    private float dragEndTime;
    bool dragStarted;
    public bool throwDone;
    public Slider throwHeightSli;
    float speed = 0.5f;   // 슬라이더가 움직이는 속도
    private bool goingRight = true;

    private Rigidbody rb;
    public float forceMultiplier;  // 전체 힘 조정
    public float zDirection=1f;
    private float screenHeight;
    private float screenWidth;

    void Awake()
    {
        rotationBox = GetComponent<RotationBox>();
        //throwHeightSli = GameObject.FindGameObjectWithTag("HeightSlider").GetComponent<Slider>();
        touchAreaUI = GameObject.FindGameObjectWithTag("ThrowPanel").GetComponent<RectTransform>();
        SetWeight();
    }

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height; // 화면 높이
    }

    void Update()
    {
        if (throwDone)
            return;
        if (ThrowTouchPanel.Instance.throwAble)
        {
            ThrowObject();
        }
//#if UNITY_EDITOR || UNITY_STANDALONE
//        HandleMouseInput();
//#elif UNITY_ANDROID || UNITY_IOS
//        HandleTouchInput();
//#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsInTouchArea(Input.mousePosition))
            {
                if (IsInTopScreen(Input.mousePosition))
                {
                    dragStarted = true;
                    dragStartPos = Input.mousePosition;
                    dragStartTime = Time.time;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (IsInTopScreen(Input.mousePosition) && dragStarted)
            {
                dragEndPos = Input.mousePosition;
                dragEndTime = Time.time;
                ThrowObject();
                throwHeightSli.value = 0f;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (IsInTopScreen(Input.mousePosition))
            {
                MoveSlider();
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
                    dragStarted = true;
                    dragStartPos = touch.position;
                    dragStartTime = Time.time;

                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (IsInTopScreen(touch.position) && dragStarted) // 화면 위쪽 70% 내에서만
                {
                    dragEndPos = touch.position;
                    dragEndTime = Time.time;

                    ThrowObject();
                    throwHeightSli.value = 0f;          // 슬라이더 값 초기화해주기

                }
            }

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                MoveSlider();
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
        dragStartPos = ThrowTouchPanel.Instance.dragStartPos;
        dragEndPos = ThrowTouchPanel.Instance.dragEndPos;

        Vector2 dragVector = dragEndPos - dragStartPos;
        if (dragVector.y < 0)
            return;

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = rbWeight;
        }

        // 홀드 시간으로 높이 정하기
        //float dragDuration = dragEndTime - dragStartTime;
        //Debug.Log(dragDuration);
        //Debug.Log("dragVector"+ dragVector);
        //float clampedDuration = Mathf.Clamp(dragDuration, 0.5f, 3f);
        //float mappedDuration = Mathf.Lerp(1f, 3f, (clampedDuration - 0.5f) / (3f - 0.5f));
        
        // 홀드 시간 슬라이더로 높이 정하기
        float mappedDuration = Mathf.Lerp(0.5f, 3f, ThrowTouchPanel.Instance.throwHeightSli.value);
        Debug.Log("mappedDuration : "+ mappedDuration);
        // 1. 드래그 거리 정규화 (해상도 기준 비율)
        float normalizedDragY = dragVector.y / ThrowTouchPanel.Instance.height;
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

        ThrowFinish();
    }

    void ThrowFinish()
    {
        throwDone = true;
        rotationBox.throwDone = true;
        ThrowTouchPanel.Instance.throwAble = false;
        ThrowTouchPanel.Instance.throwHeightSli.value = 0f;
        BoxManager.Instance.RemainBoxCal(gameObject);
        BoxManager.Instance.NextBoxSpawnWait();
    }

    void MoveSlider()
    {
        // 슬라이더 값 증가 또는 감소
        if (goingRight)
        {
            throwHeightSli.value += speed * Time.deltaTime;
            if (throwHeightSli.value >= throwHeightSli.maxValue)
                goingRight = false;
        }
        else
        {
            throwHeightSli.value -= speed * Time.deltaTime;
            if (throwHeightSli.value <= throwHeightSli.minValue)
                goingRight = true;
        }
    }

    void SetWeight()
    {
        weight = boxData.Weight;
        rbWeight = boxData.rbWeight;
        forceMultiplier = boxData.forceMultiplier;

        rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.mass = rbWeight;
        }
    }

    bool IsInTouchArea(Vector2 screenPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(touchAreaUI, screenPos, null);
    }

}
