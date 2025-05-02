using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetRectByTarget : MonoBehaviour
{
    public RectTransform target;
    [SerializeField] Vector2 rect;
    public Vector2 times = Vector2.one;

    void OnEnable()
    {
        if (target == null) return;

        rect.x = target.rect.width * times.x;
        rect.y = target.rect.height * times.y;

        GetComponent<RectTransform>().sizeDelta = rect;
    }

}
