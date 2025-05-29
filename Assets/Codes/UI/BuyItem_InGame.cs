using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem_InGame : MonoBehaviour
{
    public Button[] CancelBtns;

    [Header("[ Image ]")]
    public Image ItemBack;
    public Image ItemBaclPattern;
    public Image ItemIcon;

    [Header("[ Text ]")]
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemInfo;
    public TextMeshProUGUI ItemPrice;

    InGameItem item;
    int price;


    public void BuyItemOn(InGameItem item, string name, string info, int price)
    {
        this.item = item;
        this.price = price;

        ItemBack.sprite = item.GetComponent<Image>().sprite;
        ItemBaclPattern.sprite = item.transform.GetChild(0).GetComponent<Image>().sprite;
        ItemIcon.sprite = item.transform.GetChild(1).GetComponent<Image>().sprite;
        ItemIcon.color = item.transform.GetChild(1).GetComponent<Image>().color;

        ItemName.text = name;
        ItemInfo.text = info;
        ItemPrice.text = price.ToString();

        for (int i = 0; i < CancelBtns.Length; i++)
        {
            CancelBtns[i].onClick.RemoveAllListeners();
            CancelBtns[i].onClick.AddListener(() => item.ItemCancel());
        }

        GameManager.Instance.GamePause();

        gameObject.SetActive(true);
    }
    
    // 아이템 구매
    public void Buy()
    {
        //if (PlayerPrefs.GetInt("Coin", 0) < price) return;

        PlayerPrefs.SetInt(item.currentItems.ToString(), PlayerPrefs.GetInt(item.currentItems.ToString(), 0) + 1);

        item.SetText();

        GameManager.Instance.GameResume();

        gameObject.SetActive(false);
    }

    // 아이템 광고 보상
    public void AD_Gift()
    {
        PlayerPrefs.SetInt(item.currentItems.ToString(), PlayerPrefs.GetInt(item.currentItems.ToString(), 0) + 3);
    }

    public void ClosePopUp()
    {
        GameManager.Instance.GameResume();

        gameObject.SetActive(false);
    }
}
