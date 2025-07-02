using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameItem : MonoBehaviour
{
    public enum Items
    {
        item1, Item_Save, Item_Delete
    }

    public Items currentItems;

    public float hueSpeed = 0.2f; // 속도 조절
    public float saturation = 0.5f;
    public float value = 0.9f;

    [Header("[ GameObject ]")]
    public GameObject ItemPurchasePanel;
    public GameObject Lock;
    public GameObject TouchBlock;
    public GameObject SelectedItem;

    [Header("[ UI ]")]
    public BuyItem_InGame BuyItem_InGame;
    public Button OtherBlock;
    public Image SelectFrame;
    public TextMeshProUGUI[] ItemCountTexts;
    public Transform UseCountUI;
    public Transform SelectUseCountUI;
    


    BoxManager BoxManager;
    DeleteCourier DeleteCourier;

    GameObject SaveBox;

    int count, useCount;
    float hue = 0f;

    bool rainbowOn;


    void Start()
    {
        BoxManager = BoxManager.Instance;
        DeleteCourier = DeleteCourier.Instance;

        useCount = 3;

        SetText();

        SelectedItem.SetActive(false);
        OtherBlock.gameObject.SetActive(false);

        if (currentItems.ToString() == "Item_Save")
        {
            Lock.SetActive(!(PlayerPrefs.GetInt("Stage", 1) > 9));
            ItemCountTexts[0].transform.parent.gameObject.SetActive((PlayerPrefs.GetInt("Stage", 1) > 9));
        }
        
    }

    void Update()
    {
        if (rainbowOn)
            StartRainbow();

        if (GameManager.Instance.gameEnd)
        {
            ItemCancel();
            ItemPurchasePanel.SetActive(false);
            TouchBlock.SetActive(false);
        }
    }

    // 아이템 클릭
    public void ItemCheck()
    {
        if (useCount <= 0) return;

        switch (currentItems)
        {
            case Items.item1:
                if (count <= 0)
                {
                    BuyItem_InGame.BuyItemOn(this, "item1", "This! is! item1!!!!!", 10000);
                    return;
                }
                      
                break;

            case Items.Item_Save:
                if (BoxManager.spawnedBoxes.Count <= 0 && BoxManager.GoaledBoxes.Count <= 0) return;

                if (count <= 0)
                {
                    BuyItem_InGame.BuyItemOn(this, "Save", "Save selected box", 80);
                    return;
                }

                OtherBlock.onClick.RemoveAllListeners();
                BoxManager.ActiveKeepItem();

                break;

            case Items.Item_Delete:
                if (BoxManager.spawnedBoxes.Count <= 0 || !BoxManager.boxReady) return;

                if (count <= 0)
                {
                    BuyItem_InGame.BuyItemOn(this, "Revert", "Revert previous box", 100);
                    return;
                }

                UseItem();
                return;

            default:
                break;
        }

        SelectedItem.SetActive(true);
        
        OtherBlock.gameObject.SetActive(true);

        rainbowOn = true;
    }

    // 아이템 취소
    public void ItemCancel()
    {
        rainbowOn = false;
        SelectedItem.SetActive(false);
        OtherBlock.gameObject.SetActive(false);

    }

    // 아이템 사용
    public void UseItem()
    {
        ItemCancel();


        switch (currentItems)
        {
            case Items.item1:
                //
                break;
            case Items.Item_Save:
                if (!SelectSaveBoxCheck())
                {
                    BoxManager.ActiveKeepItem();
                    return;
                }

                SubtractCount();

                DeleteCourier.CarryingStart(SaveBox);

                break;
            case Items.Item_Delete:

                SubtractCount();

                BoxManager.DeleteBox();

                break;
            default:
                count = 0;
                break;
        }
    }

    void StartRainbow()
    {
        hue += Time.deltaTime * hueSpeed;
        if (hue > 1f) hue -= 1f;

        Color rainbowColor = Color.HSVToRGB(hue, saturation, value);
        SelectFrame.color = rainbowColor;
    }

    void SubtractCount()
    {
        if (count - 1 < 0 || useCount - 1 < 0) return;

        useCount -= 1;
        count -= 1;

        PlayerPrefs.SetInt(currentItems.ToString(), count);

        SetText();
    }

    public void SetText()
    {
        Lock.SetActive(currentItems.ToString() == "item1");
        ItemCountTexts[0].transform.parent.gameObject.SetActive(!(currentItems.ToString() == "item1"));

        count = PlayerPrefs.GetInt(currentItems.ToString(), 3);

        for (int i = 0; i < ItemCountTexts.Length; i++)
            ItemCountTexts[i].text = count.ToString();

        for (int count = 0; count < UseCountUI.childCount; count++)
        {
            UseCountUI.GetChild(count).GetChild(0).gameObject.SetActive(count < useCount);

            if (SelectUseCountUI)
                SelectUseCountUI.GetChild(count).GetChild(0).gameObject.SetActive(count < useCount);
        }
    }

    

    public void DeleteEnd()
    {
        TouchBlock.SetActive(false);
    }

    bool SelectSaveBoxCheck()
    {
        TouchOutline to;

        for (int i = 0; i < BoxManager.GoaledBoxes.Count; i++)
        {
            to = BoxManager.GoaledBoxes[i].GetComponent<TouchOutline>();

            if (to != null && to.isOutlined)
            {
                SaveBox = BoxManager.GoaledBoxes[i];
                return true;
            }
                
        }

        return false;
    }
}
