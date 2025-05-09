using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEffect : MonoBehaviour
{
    public RectTransform canvasRectTransform;
    public GameObject startCirclePrefab;
    public GameObject endCirclePrefab;
    public List<GameObject> circlePrefabs; // ũ�⺰ ������ (�ۡ�ū) 8��

    private const int maxCircleCount = 8;

    private GameObject startCircle;
    private GameObject endCircle;
    private List<GameObject> circlePool = new List<GameObject>();

    private bool isDragging = false;
    private Vector2 startPos;

    public float maxDistance = 800f;

    private float screenHeight;
    private float screenWidth;

    void Start()
    {
        for (int i = 0; i < maxCircleCount; i++)
        {
            GameObject circle = Instantiate(circlePrefabs[i], canvasRectTransform);
            circle.SetActive(false);
            circlePool.Add(circle);
        }

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void Update()
    {
        Vector2 inputPosition = Vector2.zero;
        bool inputDown = false;
        bool inputHeld = false;
        bool inputUp = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPosition = touch.position;
            inputDown = touch.phase == TouchPhase.Began;
            inputHeld = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            inputUp = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
        else
        {
            inputPosition = Input.mousePosition;
            inputDown = Input.GetMouseButtonDown(0);
            inputHeld = Input.GetMouseButton(0);
            inputUp = Input.GetMouseButtonUp(0);
        }

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            inputPosition,
            canvasRectTransform.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceCamera
                ? canvasRectTransform.GetComponent<Canvas>().worldCamera
                : null,
            out localPoint
        );

        // �Է� ���� �� - ����: ���� 70%�� ����
        if (inputDown && IsInTopScreen(inputPosition))
        {
            isDragging = true;

            startCircle = Instantiate(startCirclePrefab, canvasRectTransform);
            startCircle.GetComponent<RectTransform>().anchoredPosition = localPoint;
            startPos = localPoint;

            endCircle = Instantiate(endCirclePrefab, canvasRectTransform);
            endCircle.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }
        // �巡�� �� - ����: �巡�� ���̰�, ������ ������ ����
        else if (inputHeld && isDragging && IsInTopScreen(inputPosition))
        {
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
        // �Է� ����
        else if (inputUp)
        {
            isDragging = false;

            if (startCircle) Destroy(startCircle);
            if (endCircle) Destroy(endCircle);

            foreach (var c in circlePool)
                c.SetActive(false);
        }
    }

    // ȭ�� ���� 70% �ȿ� �ִ��� Ȯ���ϴ� �Լ�
    bool IsInTopScreen(Vector2 position)
    {
        return position.y >= screenHeight * 0.3f;
    }
}
