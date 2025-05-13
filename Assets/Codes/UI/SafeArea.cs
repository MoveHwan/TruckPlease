using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    RectTransform rectTransform; //safearea ��ũ��Ʈ �ٴ� ������Ʈ �־��ֱ�
    Rect safeArea;
    Vector2 minAnchor;
    Vector2 maxAnchor;

    string deviceModel;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        deviceModel = SystemInfo.deviceModel;
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        //safeArea�� �޾Ƽ� min ��Ŀ�� max ��Ŀ�� Position �ο�
        //�ȼ��� ��ȯ�Ǵ� ��Ŀ�� �ֱ� ���ؼ��� ������ ��ȯ �ʿ�
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        //�ν����� ������Ƽ�� ������� �� �ְ� ������ ��ȯ �� �Ҵ�
        minAnchor.x /= Screen.width;
        minAnchor.y *= 0;
        //minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;

    }

}