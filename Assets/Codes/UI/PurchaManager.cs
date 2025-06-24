using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class PurchaManager : MonoBehaviour
{
    public static PurchaManager instance;

    public GameObject RemoveADBtn;
    public GameObject RemoveADPopUp;

    public TextMeshProUGUI RemoveAD_PriceText;

    private void Awake()
    {
        instance = this;

        RemoveADPopUp.SetActive(false);

        if (PlayerPrefs.GetInt("RemoveAd", 0) == 1)
        {
            RemoveADBtn.SetActive(false);
        }
        else
        {
            RemoveADBtn.SetActive(true);
        }

        if (IAPManager.Instance != null)
        {
            Product product = IAPManager.Instance.GetProduct("removead"); // 등록한 상품 ID
            if (product != null && product.availableToPurchase)
            {
                RemoveAD_PriceText.text = product.metadata.localizedPriceString;
            }
        }
    }



    public void PurchaComplete_RemoveAD()
    {
        if (GameDatas.instance != null)
            GameDatas.instance.CloudSave();

        if (GoogleAd.instance != null)
            GoogleAd.instance.LoadAd();

        RemoveADPopUp.SetActive(false);
        RemoveADBtn.SetActive(false);
    }
}
