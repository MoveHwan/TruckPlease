using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;

    [Header("Product ID")]
    public readonly string productId_test_id = "removead";

    [Header("Cache")]
    private IStoreController storeController; //���� ������ �����ϴ� �Լ� ������
    private IExtensionProvider storeExtensionProvider; //���� �÷����� ���� Ȯ�� ó�� ������

    private void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitUnityIAP(); //Start ������ �ʱ�ȭ �ʼ�
    }

    /* Unity IAP�� �ʱ�ȭ�ϴ� �Լ� */
    private void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* ���� �÷��� ��ǰ�� �߰� */
        //builder.AddProduct(productId_test_id, ProductType.NonConsumable, new IDs() { { productId_test_id, GooglePlay.Name } });
        builder.AddProduct(productId_test_id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    /* ��ǰ ���� ��ȯ */
    public Product GetProduct(string productId)
    {
        return storeController.products.WithID(productId);
    }

    public void BuyRemoveAds()
    {
        BuyProductID(productId_test_id);
    }

    void BuyProductID(string productId)
    {
        if (storeController != null && storeController.products != null)
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("��ǰ ��� �Ұ�");
            }
        }
        else
        {
            Debug.Log("IAP �ý����� �ʱ�ȭ���� �ʾҽ��ϴ�.");
        }
    }


    /* �����ϴ� �Լ� */
    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId); //��ǰ ����

        if (product != null && product.availableToPurchase) //��ǰ�� �����ϸ鼭 ���� �����ϸ�
        {
            storeController.InitiatePurchase(product); //���Ű� �����ϸ� ����
        }
        else //��ǰ�� �������� �ʰų� ���� �Ұ����ϸ�
        {
            Debug.Log("��ǰ�� ���ų� ���� ���Ű� �Ұ����մϴ�");
        }
    }

    #region Interface
    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");

        storeController = controller;
        storeExtensionProvider = extension;
    }

    /* �ʱ�ȭ ���� �� ����Ǵ� �Լ� */
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");
    }

    public void OnInitializeFailed(InitializationFailureReason error , string message)
    {
        Debug.Log("�ʱ�ȭ�� �����߽��ϴ�");
        Debug.Log(message);
    }

    /* ���ſ� �������� �� ����Ǵ� �Լ� */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("���ſ� �����߽��ϴ�");
    }

    /* ���Ÿ� ó���ϴ� �Լ� */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("���ſ� �����߽��ϴ�");

        if (args.purchasedProduct.definition.id == productId_test_id)
        {
            /* removead ���� ó�� */
            PlayerPrefs.SetInt("RemoveAd", 1);

            if (GoogleAd.instance != null)
                GoogleAd.instance.HideBanner();

            if (PurchaManager.instance != null)
                PurchaManager.instance.PurchaComplete_RemoveAD();
        }

        return PurchaseProcessingResult.Complete;
    }
    #endregion
}