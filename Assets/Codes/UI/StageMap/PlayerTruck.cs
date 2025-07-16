using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerTruck : MonoBehaviour
{
    public static PlayerTruck Instance;

    public RectTransform rect;
    public Transform content;
    public GameObject DeliveryBox;

    public bool moveEnd;

    static Vector2 defaultDis = Vector2.right * 180;

    int dir;


    void Awake()
    {
        Instance = this;
    }


    public void SetPlayerTruck(bool left, RectTransform target)
    {
        Vector3 scale = rect.localScale;

        dir = left ? -1 : 1;
        scale.x *= dir;
        rect.localScale = scale;

        Vector3 localPos = content.InverseTransformPoint(target.position);

        rect.localPosition = localPos;
        rect.anchoredPosition += dir * defaultDis * Vector2.right - 40 * Vector2.up;
    }

    public void DeliveryBoxOn()
    {
        StartCoroutine(DeliveryBoxCoroutine());
    }

    IEnumerator DeliveryBoxCoroutine()
    {
        DeliveryBox box = null;

        Vector2 dis = defaultDis;

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    dis.x = defaultDis.x - 25;
                    break;
                case 1:
                    dis.x = defaultDis.x + 25;
                    break;
                case 2:
                    dis = defaultDis;
                    dis.y = 25;
                    break;
            }

            box = Instantiate(DeliveryBox, transform.parent).GetComponent<DeliveryBox>();

            box.transform.position = transform.position;

            box.BoxMoveOn(dir, dis);

            yield return new WaitUntil(() => box.MoveCheck());
        }

        yield return new WaitUntil(() => box.MoveCheck());

        moveEnd = true;

        yield break;
    }
}
