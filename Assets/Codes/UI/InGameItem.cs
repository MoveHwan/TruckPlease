using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameItem : MonoBehaviour
{
    public enum Items
    {
        item1, item2, item3
    }

    public Items currentItems;

    public TextMeshProUGUI ItemCountText;

    int count;

    void Start()
    {
        int count = 6;
        ItemCountText.text = count.ToString();
    }

    public void UseItem()
    {
        if (count <= 0) return;

        count -= 1;

        ItemCountText.text = count.ToString();

        switch (currentItems)
        {
            case Items.item1:
                break;
            case Items.item2:
                break;
            case Items.item3:
                BoxManager.Instance.DeleteBox();

                break;
        }
    }
}
