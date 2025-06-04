using System;
using System.Collections.Generic;
using _Scripts.IAP;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
  
    IStoreController m_StoreController; // The Unity Purchasing system.

    public List<PackGold> packs;
    //Your products IDs. They should match the ids of your products in your store.

    
    void Start()
    {
        InitializePurchasing();
        UpdateUI();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Add products that will be purchasable and indicate its type.
        builder.AddProduct(StringKeyIAP.STARTER_DEAL, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.COMMON_PACK, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.NORMAL_PACK, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.RARE_PACK, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.EPIC_PACK, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.LEGENDARY_PACK, ProductType.NonConsumable);
        builder.AddProduct(StringKeyIAP.COIN_FEW, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.COIN_BAG, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.COIN_PILE, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.COIN_BOX, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.COIN_CHEST, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.COIN_VAULT, ProductType.Consumable);
        builder.AddProduct(StringKeyIAP.REMOVE_ADS, ProductType.NonConsumable);


        UnityPurchasing.Initialize(this, builder);
    }


    // public void BuyGold()
    // {
    //     m_StoreController.InitiatePurchase(goldProductId);
    // }

    // public void BuyDiamond()
    // {
    //     m_StoreController.InitiatePurchase(diamondProductId);
    // }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        //Add the purchased product to the players inventory
        if (product.definition.id == StringKeyIAP.STARTER_DEAL)
        {
            packs[0].OnBuySuccess();
        }
        
        else if (product.definition.id == StringKeyIAP.COMMON_PACK)
        {
            packs[1].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.NORMAL_PACK)
        {
            packs[2].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.RARE_PACK)
        {
            packs[3].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.EPIC_PACK)
        {
            packs[4].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.LEGENDARY_PACK)
        {
            packs[5].OnBuySuccess();
            
        }
        else if (product.definition.id == StringKeyIAP.COIN_FEW)
        {
            packs[6].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.COIN_BAG)
        {
            packs[7].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.COIN_PILE)
        {
            packs[8].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.COIN_BOX)
        {
            packs[9].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.COIN_CHEST)
        {
            packs[10].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.COIN_VAULT)
        {
            packs[11].OnBuySuccess();
        }
        else if (product.definition.id == StringKeyIAP.REMOVE_ADS)
        {
            packs[12].OnBuySuccess();
        }

        UpdateUI();
        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                  $" Purchase failure reason: {failureDescription.reason}," +
                  $" Purchase failure details: {failureDescription.message}");
    }

    public void UpdateUI()
    {
        if (!PlayerPrefs.HasKey(StringPlayerPrefs.STARTER_DEAL_PACK))
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.STARTER_DEAL_PACK, 0);
        }

        if (!PlayerPrefs.HasKey(StringPlayerPrefs.REMOVED_ADS_PACK))
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.REMOVED_ADS_PACK, 0);
        }

        if (!PlayerPrefs.HasKey(StringPlayerPrefs.REMOVED_ADS_VIP))
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.REMOVED_ADS_VIP, 0);
        }




        int removeAds = PlayerPrefs.GetInt(StringPlayerPrefs.STARTER_DEAL_PACK);
        int startDeal = PlayerPrefs.GetInt(StringPlayerPrefs.STARTER_DEAL_PACK);

        if (startDeal == 1)
        {
            packs[0].gameObject.SetActive(false);
        }

        if (removeAds == 1)
        {
            packs[packs.Count - 1].gameObject.SetActive(false);
        }
        
    }


    #region initiate Click Button
    public void BuyStartDeal()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.STARTER_DEAL);
    }

    public void BuyCommonPack()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COMMON_PACK);
    }

    public void BuyNormalPack()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.NORMAL_PACK);
    }

    public void BuyRarePack()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.RARE_PACK);
    }

    public void BuyEpicPack()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.EPIC_PACK);
    }

    public void BuyLegendaryPack()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.LEGENDARY_PACK);
    }

    public void BuyCoinFew()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COIN_FEW);
    }

    public void BuyCoinBag()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COIN_BAG);
    }

    public void BuyCoinPile()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COIN_PILE);
    }

    public void BuyCoinBox()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COIN_BOX);
    }

    public void BuyCoinChest()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COIN_CHEST);
    }

    public void BuyCoinVault()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.COIN_VAULT);
    }

    public void BuyRemoveAds()
    {
        m_StoreController.InitiatePurchase(StringKeyIAP.REMOVE_ADS);
    }
    
    
    #endregion
    
}