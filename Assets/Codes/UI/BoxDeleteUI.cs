using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxDeleteUI : MonoBehaviour
{
    public static BoxDeleteUI Instance;

    public InGameItem DeleteItem;

    public GameObject OtherItemBlock;
    public GameObject TouchBlock;

    public DeleteCourier DeleteCourier;
    public Image SelectFrame;

    public float hueSpeed = 0.2f; // 속도 조절
    public float saturation = 0.5f;
    public float value = 0.9f;

    Tween colorTween;

    Color[] rainbowColors;

    int currentIndex;
    float hue = 0f;

    bool rainbowOn;

    void Awake()
    {
        Instance = this;

        SelectFrame.gameObject.SetActive(false);
        OtherItemBlock.SetActive(false);
        TouchBlock.SetActive(false);
    }

    void Update()
    {
        if (rainbowOn)
            StartRainbow();

    }


    public void DeleteModeOn()
    {
        OtherItemBlock.SetActive(true);
        SelectFrame.gameObject.SetActive(true);

        rainbowOn = true;

    }

    public void DeleteCheck()
    {
        rainbowOn = false;

        SelectFrame.gameObject.SetActive(false);
        OtherItemBlock.SetActive(false);

        if (BoxManager.Instance.GoaledBoxes.Count == 0) return;

        // 삭제할 상자를 선택한 경우
        TouchBlock.SetActive(true);

        DeleteItem.SubtractCount();

        DeleteCourier.CarryingStart();
    }

    public void DeleteEnd()
    {
        TouchBlock.SetActive(false);
    }

    void StartRainbow()
    {
        SelectFrame.gameObject.SetActive(true);

        hue += Time.deltaTime * hueSpeed;
        if (hue > 1f) hue -= 1f;

        Color rainbowColor = Color.HSVToRGB(hue, saturation, value);
        SelectFrame.color = rainbowColor;
    }

}
