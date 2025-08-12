using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTruck : MonoBehaviour
{
    public static PlayerTruck Instance;

    public RectTransform rect;
    public RectTransform target;
    public Transform content;
    public GameObject DeliveryBox;

    public bool moveEnd, check;

    static Vector2 defaultDis = Vector2.right * 180 + Vector2.up * 50;

    Vector3 localPos;
    int dir;
    bool isleft;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (target != null && Vector3.Distance(content.InverseTransformPoint(target.position), localPos) > 0.001f)
        {
            SetPlayerTruck(isleft, target);
        }
    }

    public void SetPlayerTruck(bool left, RectTransform target)
    {
        this.target = target;
        isleft = left;

        Vector3 scale = rect.localScale;

        dir = left ? -1 : 1;
        scale.x = -dir;
        rect.localScale = scale;

        localPos = content.InverseTransformPoint(target.position);

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
                    dis.y += 25;
                    break;
            }

            GameObject boxObject = Instantiate(DeliveryBox, transform.parent);
            boxObject.SetActive(false);

            box = boxObject.GetComponent<DeliveryBox>();

            box.transform.localPosition = transform.localPosition + Vector3.up * rect.sizeDelta.y / 2;
            box.BoxMoveOn(dir, dis);

            yield return new WaitUntil(() => box.MoveCheck());
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.6f);

        moveEnd = true;

        yield break;
    }
}
