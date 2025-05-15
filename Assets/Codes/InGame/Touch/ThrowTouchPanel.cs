using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ThrowTouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler , IDragHandler
{
    public static ThrowTouchPanel Instance;

    RectTransform rect;
    public RectTransform CircleIn;
    public float height;
    public Slider throwHeightSli;

    public Vector2 dragStartPos;
    private float dragStartTime;

    public Vector2 dragEndPos;
    private float dragEndTime;

    private bool dragStarted = false;
    public bool throwAble;

    private bool goingRight = true;
    float speed = 0.5f;   // �����̴��� �����̴� �ӵ�

    private bool isPressing = false;

    public GameObject startCirclePrefab;
    public GameObject endCirclePrefab;
    public List<GameObject> circlePrefabs;

    private const int maxCircleCount = 8;

    private GameObject startCircle;
    private GameObject endCircle;
    private List<GameObject> circlePool = new List<GameObject>();

    private bool isDragging = false;
    private Vector2 startPos;
    private float screenHeight;

    public float maxDistance = 800f;

    private Canvas canvas;

    public ThrowBox controlBox;


    void Awake()
    {
        Instance = this;
        
        rect = GetComponent<RectTransform>();
        canvas = CircleIn.GetComponent<Canvas>();
        height = rect.rect.height;
    }

    void Start()
    {
        screenHeight = Screen.height;

        for (int i = 0; i < maxCircleCount; i++)
        {
            GameObject circle = Instantiate(circlePrefabs[i], rect);
            circle.SetActive(false);
            circlePool.Add(circle);
        }
    }
    void Update()
    {
        if (isPressing)
        {
            MoveSlider();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (controlBox == null)
            return;
        dragStarted = true;
        isPressing = true;


        //dragStartPos = eventData.position;
        dragStartTime = Time.time;
        throwHeightSli.value = 0f;
        Debug.Log("Pointer Down: " + dragStartPos);

        isDragging = true;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            CircleIn,
            eventData.position,
            null, // Overlay ���ϱ� ������ null!
            out localPoint
        );

        // 2. Clamp (rect ���� ����)
        Rect rect = CircleIn.rect;
        localPoint.x = Mathf.Clamp(localPoint.x, rect.xMin, rect.xMax);
        localPoint.y = Mathf.Clamp(localPoint.y, rect.yMin, rect.yMax);

        // 3. Clamp�� localPoint �� �ٽ� screen ��ǥ��� ��ȯ�ϰ� �ʹٸ� (���û���)
        // Vector2 worldPoint = CircleIn.TransformPoint(localPoint);
        // dragEndPos = RectTransformUtility.WorldToScreenPoint(null, worldPoint);

        // 4. ����
        startPos = localPoint; // �Ǵ� worldPoint / anchoredPosition, ��Ȳ�� ����
        dragStartPos = localPoint;

        startCircle = Instantiate(startCirclePrefab, CircleIn);
        startCircle.GetComponent<RectTransform>().anchoredPosition = localPoint;

        endCircle = Instantiate(endCirclePrefab, CircleIn);
        endCircle.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragStarted)
        {
            // 1. Screen �� LocalPoint�� ��ȯ
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                CircleIn,
                eventData.position,
                null,
                out localPoint
            );

            // 2. Clamp (rect ���� ����)
            Rect rect = CircleIn.rect;
            localPoint.x = Mathf.Clamp(localPoint.x, rect.xMin, rect.xMax);
            localPoint.y = Mathf.Clamp(localPoint.y, rect.yMin, rect.yMax);

            // 3. Clamp�� localPoint �� �ٽ� screen ��ǥ��� ��ȯ�ϰ� �ʹٸ� (���û���)
            // Vector2 worldPoint = CircleIn.TransformPoint(localPoint);
            // dragEndPos = RectTransformUtility.WorldToScreenPoint(null, worldPoint);

            // 4. ����
            dragEndPos = localPoint; // �Ǵ� worldPoint / anchoredPosition, ��Ȳ�� ����
            
            //dragEndPos = eventData.position;
            dragEndTime = Time.time;

            dragStarted = false;

            throwAble = true;
            isPressing = false;

            Debug.Log("Pointer Up: " + dragEndPos);

            isDragging = false;

            if (startCircle) Destroy(startCircle);
            if (endCircle) Destroy(endCircle);

            foreach (var c in circlePool)
                c.SetActive(false);
            if (controlBox != null)
            {
                controlBox.ThrowObject();
            }

        }
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

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            CircleIn,
            eventData.position,
            null, // Overlay ���ϱ� ������ null!
            out localPoint
        );

        Rect rect = CircleIn.rect;
        localPoint.x = Mathf.Clamp(localPoint.x, rect.xMin, rect.xMax);
        localPoint.y = Mathf.Clamp(localPoint.y, rect.yMin, rect.yMax);

        Vector2 endPos = localPoint;
        float dist = Vector2.Distance(startPos, endPos);
        int activeCount = Mathf.Clamp(Mathf.FloorToInt((dist / maxDistance) * maxCircleCount), 2, maxCircleCount);

        for (int i = 0; i < maxCircleCount; i++)
        {
            if (i < activeCount)
            {
                if (!circlePool[i].activeSelf)
                    circlePool[i].SetActive(true);

                float t = (i + 1f) / (activeCount + 1f);
                Vector2 pos = Vector2.Lerp(startPos, endPos, t);
                circlePool[i].GetComponent<RectTransform>().anchoredPosition = pos;
            }
            else
            {
                if (circlePool[i].activeSelf)
                    circlePool[i].SetActive(false);
            }
        }

        if (endCircle != null)
            endCircle.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }
}
