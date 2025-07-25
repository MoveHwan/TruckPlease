using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPadScale : MonoBehaviour
{
    CanvasScaler canvas;

    void Awake()
    {
        canvas = GetComponent<CanvasScaler>();

        Vector2 referenceVec = canvas.referenceResolution;

        float referenceRatio = (float)referenceVec.y / (float)referenceVec.x;
        float screenRatio = (float)Screen.height / (float)Screen.width;

        float match = screenRatio < referenceRatio ? 1 : 0;

        canvas.matchWidthOrHeight = match;

        Debug.Log($"ScreenRatio: {screenRatio} Match: {match}");
    }

}
