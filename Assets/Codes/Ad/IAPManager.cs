using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;

    [Header("Product ID")]
    public readonly string productId_test_id = "removeads";

    [Header("Cache")]
    private IStoreController storeController; //구매 과정을 제어하는 함수 제공자
    private IExtensionProvider storeExtensionProvider; //여러 플랫폼을 위한 확장 처리 제공자

    public bool initializeEnd;

    PurchaManager PurchaProduct;

    bool purchaseTrigger;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitUnityIAP(); //Start 문에서 초기화 필수
    }

    /* Unity IAP를 초기화하는 함수 */
    private void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* 구글 플레이 상품들 추가 */
        //builder.AddProduct(productId_test_id, ProductType.NonConsumable, new IDs() { { productId_test_id, GooglePlay.Name } });
        builder.AddProduct(productId_test_id, ProductType.NonConsumable);
        builder.AddProduct("removeads_sale", ProductType.NonConsumable);
        builder.AddProduct("removeads_gold_pack", ProductType.NonConsumable);
        builder.AddProduct("removeads_premium_pack", ProductType.NonConsumable);
        builder.AddProduct("gold_2000", ProductType.Consumable);
        builder.AddProduct("gold_5000", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    /* 상품 정보 반환 */
    public Product GetProduct(string productId)
    {
        if (storeController != null && storeController.products != null)
            return storeController.products.WithID(productId);

        return null;
    }

    public void BuyProduct(string name, PurchaManager purchaProduct)
    {
        PurchaProduct = purchaProduct;

        BuyProductID(name);
    }

    void BuyProductID(string productId)
    {
        if (storeController != null && storeController.products != null)
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
                purchaseTrigger = true;
            }
            else
            {
                Debug.Log("상품 사용 불가");
            }
        }
        else
        {
            Debug.Log("IAP 시스템이 초기화되지 않았습니다.");
        }
    }


    /* 구매하는 함수 */
    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId); //상품 정의

        if (product != null && product.availableToPurchase) //상품이 존재하면서 구매 가능하면
        {
            storeController.InitiatePurchase(product); //구매가 가능하면 진행
        }
        else //상품이 존재하지 않거나 구매 불가능하면
        {
            Debug.Log("상품이 없거나 현재 구매가 불가능합니다");
        }
    }

    #region Interface
    /* 초기화 성공 시 실행되는 함수 */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        storeController = controller;
        storeExtensionProvider = extension;

        bool removeAds_hasReceipt = false;
        string receipt = "";
        string productId = "";

        if (GameDatas.instance != null)
            receipt = GameDatas.instance.dataSettings.RemoveAdsReceipt;

        foreach (var product in controller.products.all)
        {
            if (product.definition.type == ProductType.NonConsumable)
            {
                if (!removeAds_hasReceipt && product.hasReceipt && product.receipt == receipt && product.definition.id.Split("_")[0] == "removeads")
                {
                    removeAds_hasReceipt = true;
                    productId = product.definition.id;

                    Debug.Log("removeads 상품 영수증 확인");
                }
            }

            Debug.Log($"{product.definition.id} 상품 체크");
        }

        // 광고제거 환불확인
        if (!removeAds_hasReceipt && PlayerPrefs.GetInt("RemoveAD", 0) == 1)
        {
            PlayerPrefs.SetInt("RemoveAD", 0);
           
            switch (productId)
            {
                case "removeads_gold_pack":
                    PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 1000);
                    break;
                case "removeads_premium_pack":
                    PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 6000);
                    PlayerPrefs.SetInt("Fatigue", 5);
                    PlayerPrefs.SetInt("Unlimited_Heart", 0);
                    PlayerPrefs.SetInt("Item_Save", PlayerPrefs.GetInt("Item_Save", 3) - 3);
                    PlayerPrefs.SetInt("Item_Delete", PlayerPrefs.GetInt("Item_Delete", 3) - 3);
                    break;
            }

            PlayerPrefs.Save();

            if (GameDatas.instance != null)
                GameDatas.instance.CloudSave();

            Debug.LogWarning("removeads 상품 영수증 확인 불가");
        }
            

        initializeEnd = true;

        Debug.Log("초기화에 성공했습니다");
    }

    /* 초기화 실패 시 실행되는 함수 */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화에 실패했습니다");

        initializeEnd = true;
    }

    public void OnInitializeFailed(InitializationFailureReason error , string message)
    {
        Debug.Log("초기화에 실패했습니다");
        Debug.Log(message);

        initializeEnd = true;
    }

    /* 구매에 실패했을 때 실행되는 함수 */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("구매에 실패했습니다");

        purchaseTrigger = false;
    }

    /* 구매를 처리하는 함수 */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Product product = args.purchasedProduct;
        
        // 자동 구매 복원인지 확인
        bool isRestoredPurchase = string.IsNullOrEmpty(product.transactionID) == false
                                   && purchaseTrigger == false
                                   && product.hasReceipt;

        if (isRestoredPurchase) 
        {
            string checkRemoveAd = args.purchasedProduct.definition.id;

            if (checkRemoveAd.Split("_")[0] == "removeads")
            {
                PlayerPrefs.SetInt("RemoveAD", 1);
                PlayerPrefs.Save();

                if (GoogleAd.instance != null)
                {
                    GoogleAd.instance.LoadAd();
                    GoogleAd.instance.HideBanner();
                }

                Debug.Log($"[IAP] 자동 복원 처리된 상품: {product.definition.id}");
                return PurchaseProcessingResult.Complete;
            }
        }

        PurchaProduct.PurchaComplete(args.purchasedProduct.definition.id, args);
        PurchaProduct = null;
        purchaseTrigger = false;

        Debug.Log($"[IAP] 사용자가 직접 구매한 상품: {product.definition.id}");
        Debug.Log($"{product.definition.id} 구매에 성공했습니다");

        return PurchaseProcessingResult.Complete;
    }
    #endregion

}