using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController; // Đối tượng điều khiển Store
    private static IExtensionProvider m_StoreExtensionProvider; // Đối tượng mở rộng cho các nền tảng

    // ID sản phẩm (phải khớp với ID trên Google Play hoặc App Store)
    public static string kProductIDConsumable = "com.holemaster.pack.starter_deal";
    public static string kProductIDNonConsumable = "com.holemaster.pack.common_pack";
    public static string kProductIDSubscription = "subscription_item";

    void Start()
    {
        // Kiểm tra xem IAP đã được khởi tạo chưa
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // Kiểm tra xem IAP có được hỗ trợ không
        if (IsInitialized())
        {
            return;
        }

        // Cấu hình các sản phẩm IAP
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Thêm sản phẩm vào danh sách
        builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
        //builder.AddProduct(kProductIDSubscription, ProductType.Subscription);

        // Khởi tạo IAP
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    // Gọi khi khởi tạo thành công
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP Khởi tạo thành công!");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAP Khởi tạo thất bại: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP Khởi tạo thất bại: {message}");
    }

    // Gọi khi giao dịch mua hàng
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // Kiểm tra ID sản phẩm
        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
        {
            Debug.Log("Mua thành công sản phẩm Consumable!");
            // Xử lý logic (ví dụ: cộng tiền, vật phẩm,...)
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
        {
            Debug.Log("Mua thành công sản phẩm Non-Consumable!");
            // Xử lý logic
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
        {
            Debug.Log("Mua thành công sản phẩm Subscription!");
            // Xử lý logic
        }
        else
        {
            Debug.Log($"Mua thất bại: Sản phẩm không xác định {args.purchasedProduct.definition.id}");
        }

        return PurchaseProcessingResult.Complete;
    }

    // Gọi khi mua hàng thất bại
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Mua thất bại: {product.definition.id}, Lý do: {failureReason}");
    }

    // Hàm gọi khi người chơi nhấn nút mua
    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"Bắt đầu mua sản phẩm: {productId}");
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Sản phẩm không khả dụng hoặc không tìm thấy.");
            }
        }
        else
        {
            Debug.LogError("IAP chưa được khởi tạo.");
        }
    }

    // Ví dụ gọi hàm mua sản phẩm
    public void BuyConsumable()
    {
        BuyProductID(kProductIDConsumable);
    }

    public void BuyNonConsumable()
    {
        BuyProductID(kProductIDNonConsumable);
    }

    public void BuySubscription()
    {
        BuyProductID(kProductIDSubscription);
    }
}