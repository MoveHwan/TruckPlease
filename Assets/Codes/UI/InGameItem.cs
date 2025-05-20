using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameItem : MonoBehaviour
{
    public GameObject ItemPurchasePanel;

    public TextMeshProUGUI ItemCountText;

    public enum Items
    {
        item1, item2, Item_Delete
    }

    public Items currentItems;

    int count;

    void Start()
    {
        switch (currentItems)
        {
            case Items.item1:
                count = PlayerPrefs.GetInt("item1", 0);
                break;
            case Items.item2:
                count = PlayerPrefs.GetInt("item2", 0);
                break;
            case Items.Item_Delete:
                count = PlayerPrefs.GetInt("Item_Delete", 6);
                break;
            default:
                count = 0;
                break;
        }

        ItemCountText.text = count.ToString();
    }

    public void UseItem()
    {
        if (count <= 0)
        {
            ItemPurchasePanel.SetActive(true);

            return;
        }

        ItemCountText.text = count.ToString();

        switch (currentItems)
        {
            case Items.item1:
                SubtractCount();
                break;
            case Items.item2:
                SubtractCount();
                break;
            case Items.Item_Delete:
                BoxDeleteUI.Instance.DeleteModeOn();
                break;
        }
    }

    public void SubtractCount()
    {
        count -= 1;

        //PlayerPrefs.SetInt(currentItems.ToString(), count);

        ItemCountText.text = count.ToString();
    }
}
