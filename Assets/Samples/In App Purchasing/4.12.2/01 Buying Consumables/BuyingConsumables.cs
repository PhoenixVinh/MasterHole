using System;
using System.Collections.Generic;
using _Scripts.IAP;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace Samples.Purchasing.Core.BuyingConsumables
{
    public class BuyingConsumables : MonoBehaviour, IDetailedStoreListener
    {
        IStoreController m_StoreController; // The Unity Purchasing system.

        public List<IPack> packs;
        //Your products IDs. They should match the ids of your products in your store.


        public Text GoldCountText;
        public Text DiamondCountText;

        int m_GoldCount;
        int m_DiamondCount;

        void Start()
        {
            InitializePurchasing();
           
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
            
            else if (product.definition.id == "")
            {

            }

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

}
