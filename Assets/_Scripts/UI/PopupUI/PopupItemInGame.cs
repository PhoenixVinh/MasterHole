using System;
using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.Firebase;
using _Scripts.ObjectPooling;
using _Scripts.UI.PauseGameUI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PopupUI
{
    public class PopupItemInGame : PauseGame
    {
        public TMP_Text title;
        public RawImage icon;
        public TMP_Text description;
        public Button buyButton;
        int indexSpecialSkill;
        int amount  = 3;
        private int price = 1800;
        
        [Header("Sprite For Icon")] public List<Sprite> listSprites;
        
        
        private string descriptionText;
        public void SetData(int indexSpecialSkill)
        {
            this.indexSpecialSkill = indexSpecialSkill;
            Utills.ChangePositionBoosterFirebase(indexSpecialSkill);
            buyButton.onClick.AddListener(BuyItem);
            UpdateUI();
            
        }

        
        private void BuyItem()
        {
            int getcurrentCoint = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
            if (getcurrentCoint >= price)
            {
                getcurrentCoint -= price;
                ManagerBooster.Instance.ChangeAmountBooster(indexSpecialSkill, 3);
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, getcurrentCoint);
                PlayerPrefs.Save();
               
                ManagerFirebase.Instance?.LogEarnResource(ResourceType.booster, Utills.GetBoosterNameByIndex(this.indexSpecialSkill), "3", Reson.exchange);
                this.gameObject.SetActive(false);
                
                ManagerFirebase.Instance?.LogSpendResource(ResourceType.currency,Utills.GetBoosterNameByIndex(this.indexSpecialSkill), price.ToString(), Reson.exchange );
                // Buy Item
            }
            else
            {
                //ManagerWinLoss
                this.gameObject.SetActive(false);
                UIManager.Instance.ShowShop();
                //MessagePooling.Instance.SpawnMessage(buyButton.transform.position, Utills.NOT_ENOUGH_COIN);
                return;
            }
            
        }

        public override void OnDisable()
        {
            
            buyButton.onClick.RemoveAllListeners();
            ManagerFirebase.Instance.positionPopup = PositionFirebase.none;
            base.OnDisable();
            
        }

        public void UpdateUI()
        {
            this.icon.texture = listSprites[indexSpecialSkill].texture;
            if (indexSpecialSkill == 0)
            {
                descriptionText = "<color=#4CAF50>Increase the hole</color> <color=#4A4A7B>size over 15 seconds</color>";
                this.title.text = "SCALE";
            }
            else if (indexSpecialSkill == 1)
            {
                
                descriptionText = "<color=#4CAF50>Pull in distant</color> <color=#4A4A7B>items using the hole for 12 seconds</color>";
                this.title.text = "MAGNET";
            }
            else if (indexSpecialSkill == 2)
            {
                descriptionText = "<color=#4CAF50>Reveal the locations</color> <color=#4A4A7B>of mission objects for 15 seconds</color>";
                this.title.text = "LOCATION";
            }
            else
            {
                descriptionText = "<color=#4CAF50>Attract items</color> <color=#4A4A7B>freeze for 12 seconds</color>";
                this.title.text = "ICE";
            }

            this.description.text = descriptionText;
        }

        
    }
}