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
    public float weight;   //  ���� ����
    float rbWeight;

    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private float dragStartTime;
    private float dragEndTime;
    bool dragStarted;
    public bool throwDone;
    public Slider throwHeightSli;
    float speed = 0.5f;   // �����̴��� �����̴� �ӵ�
    private bool goingRight = true;

    private Rigidbody rb;
    public float forceMultiplier;  // ��ü �� ����
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
        screenHeight = Screen.height; // ȭ�� ����
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

    // ��ġ �Է� ó��
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsInTopScreen(touch.position)) // ȭ�� ���� 70% ��������
                {
                    dragStarted = true;
                    dragStartPos = touch.position;
                    dragStartTime = Time.time;

                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (IsInTopScreen(touch.position) && dragStarted) // ȭ�� ���� 70% ��������
                {
                    dragEndPos = touch.position;
                    dragEndTime = Time.time;

                    ThrowObject();
                    throwHeightSli.value = 0f;          // �����̴� �� �ʱ�ȭ���ֱ�

                }
            }

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                MoveSlider();
            }
        }
    }

    // ȭ�� ���� 70% �ȿ� �ִ��� Ȯ���ϴ� �Լ�
    bool IsInTopScreen(Vector2 position)
    {
        return position.y >= screenHeight * 0.3f; // ȭ�� ������ 30% �̻�, �� ���� 70% ��
    }

    // ��ü ������
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

        // Ȧ�� �ð����� ���� ���ϱ�
        //float dragDuration = dragEndTime - dragStartTime;
        //Debug.Log(dragDuration);
        //Debug.Log("dragVector"+ dragVector);
        //float clampedDuration = Mathf.Clamp(dragDuration, 0.5f, 3f);
        //float mappedDuration = Mathf.Lerp(1f, 3f, (clampedDuration - 0.5f) / (3f - 0.5f));
        
        // Ȧ�� �ð� �����̴��� ���� ���ϱ�
        float mappedDuration = Mathf.Lerp(0.5f, 3f, ThrowTouchPanel.Instance.throwHeightSli.value);
        Debug.Log("mappedDuration : "+ mappedDuration);
        // 1. �巡�� �Ÿ� ����ȭ (�ػ� ���� ����)
        float normalizedDragY = dragVector.y / ThrowTouchPanel.Instance.height;
        // 3. �� ���: ����ȭ�� �巡�� �� ������
        float throwForce = normalizedDragY;
        throwForce = Mathf.Lerp(0.3f, 1f, throwForce);
        normalizedDragY = Mathf.Lerp(1f, 3f, Mathf.Clamp01(normalizedDragY));

        // x���� ����
        float normalizedDragX = dragVector.x / screenWidth;
        Debug.Log("normalizedDragY: " + normalizedDragY);
        Debug.Log("normalizedDragX : " + normalizedDragX);
        
        // 2. ���� ���
        Vector3 throwDirection = new Vector3(normalizedDragX, mappedDuration, zDirection).normalized;
        //throwDirection.y += 0.5f;
        Debug.Log(throwForce);

        // 4. �� ����
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
        // �����̴� �� ���� �Ǵ� ����
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
