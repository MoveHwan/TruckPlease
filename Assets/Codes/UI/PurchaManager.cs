using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class PurchaManager : MonoBehaviour
{
    static List<PurchaManager> RemoveAdsPurchas = new List<PurchaManager>();

    public enum Products
    {
        removeads, removeads_sale, removeads_gold_pack, removeads_premium_pack, gold_2000, gold_5000, heart_3, heart_10, heart_30
    }

    public Products currentProduct;

    [Header("Product UI")]
    public GameObject Block;
    public GameObject PriceBtn;
    public TextMeshProUGUI PriceText;

    [Header("RemoveAD PopUp")]
    public GameObject RemoveADPopUp;
    
    private void Start()
    {
        string checkName = currentProduct.ToString().Split("_")[0];

        if (checkName == "removeads")
        {
            int idx = RemoveAdsPurchas.FindIndex(p => p.currentProduct == currentProduct);

            if (idx == -1)
                RemoveAdsPurchas.Add(this);
            else
                RemoveAdsPurchas[idx] = this;
            
            if (PlayerPrefs.GetInt("RemoveAD", 0) == 1)
            {
                HideRemoveAdsProduct();
                return;
            }
        }

        if (checkName != "heart")
            StartCoroutine(WaitIAPManager());
    }

    IEnumerator WaitIAPManager()
    {
        yield return new WaitUntil(() => IAPManager.Instance != null && IAPManager.Instance.initializeEnd);

        string productName = currentProduct.ToString();
        Product product = IAPManager.Instance.GetProduct(productName);

        if (product == null || !product.availableToPurchase)
        {
            Debug.LogWarning($"상품 {productName}을 찾을 수가 없습니다.");
            yield break;
        }

        PriceText.text = product.metadata.localizedPriceString;

        Debug.Log($"상품 {productName} 세팅 완료.");
    }

    // 구매 버튼 할당 함수
    public void PurchaProduct()
    {
        switch (currentProduct)
        {
            // 골드 구매 상품
            case Products.heart_3:
                if (PlayerPrefs.GetInt("Gold", 0) < 30)
                {
                    Debug.LogWarning("골드가 부족합니다");
                    return;
                }

                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 30);
                PlayerPrefs.SetInt("Fatigue", PlayerPrefs.GetInt("Fatigue", 5) + 3);
                break;

            case Products.heart_10:
                if (PlayerPrefs.GetInt("Gold", 0) < 80)
                {
                    Debug.LogWarning("골드가 부족합니다");
                    return;
                }

                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 80);
                PlayerPrefs.SetInt("Fatigue", PlayerPrefs.GetInt("Fatigue", 5) + 10);
                break;

            case Products.heart_30:
                if (PlayerPrefs.GetInt("Gold", 0) < 100)
                {
                    Debug.LogWarning("골드가 부족합니다");
                    return;
                }
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 100);
                PlayerPrefs.SetInt("Fatigue", PlayerPrefs.GetInt("Fatigue", 5) + 30);
                break;

            // 유료 구매 상품
            default:
                IAPManager.Instance.BuyProduct(currentProduct.ToString(), this);
                return;
        }

        Debug.Log($"{currentProduct} 구매 완료 Gold:{PlayerPrefs.GetInt("Gold", 0)} Fatigue{PlayerPrefs.GetInt("Fatigue", 5)}");

        PlayerPrefs.Save();

        if (GameDatas.instance != null)
            GameDatas.instance.CloudSave();
    }

    // 구매완료 상품지급
    public void PurchaComplete(string name, PurchaseEventArgs args)
    {
        // 상품 이름 확인
        if (name != currentProduct.ToString())
        {
            Debug.LogWarning("상품 이름이 다릅니다.");
            return;
        }
        
        // 영수증 확인
        if (args == null || args.purchasedProduct == null || string.IsNullOrEmpty(args.purchasedProduct.receipt))
        {
            Debug.LogWarning("영수증 정보가 없습니다.");
            return;
        }

        Debug.Log("영수증이 확인되었습니다: " + args.purchasedProduct.receipt);

        // 광고제거 상품일시
        string checkRemoveAd = name.Split("_")[0];
        if (checkRemoveAd == "removeads")
        {
            PlayerPrefs.SetInt("RemoveAD", 1);

            if (GoogleAd.instance != null)
            {
                GoogleAd.instance.LoadAd();
                GoogleAd.instance.HideBanner();
            }

            if (GameDatas.instance != null)
                GameDatas.instance.dataSettings.RemoveAdsReceipt = args.purchasedProduct.receipt;

            BuyRemoveAdsComplete();
        }

        // 상품 지급
        switch (currentProduct)
        {
            case Products.removeads_gold_pack:
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + 1000);
                break;

            case Products.removeads_premium_pack:
                PlayerPrefs.SetInt("Unlimited_Heart", 1);
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + 6000);
                PlayerPrefs.SetInt("Item_Save", PlayerPrefs.GetInt("Item_Save", 3) + 3);
                PlayerPrefs.SetInt("Item_Delete", PlayerPrefs.GetInt("Item_Delete", 3) + 3);
                break;

            case Products.gold_2000:
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + 2000);
                break;

            case Products.gold_5000:
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + 5000);
                break;
        }

        PlayerPrefs.Save();

        if (GameDatas.instance != null)
            GameDatas.instance.CloudSave();
    }

    void BuyRemoveAdsComplete()
    {
        if (RemoveAdsPurchas == null || RemoveAdsPurchas.Count == 0)
        {
            Debug.LogWarning("RemoveAdsPurchas is Null");
            return;
        }

        foreach (var purcha in RemoveAdsPurchas)
        {
            purcha.HideRemoveAdsProduct();
        }
    }

    public void HideRemoveAdsProduct()
    {
        PriceBtn.SetActive(false);

        if (Block != null)
            Block.SetActive(true);

        if (RemoveADPopUp != null)
            RemoveADPopUp.SetActive(false);
    }

}
