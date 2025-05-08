using System;
using System.Collections.Generic;
using _Scripts.Booster;
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
                this.gameObject.SetActive(false);
                // Buy Item
            }
            else
            {
                return;
            }
            
        }

        public override void OnDisable()
        {
            
            buyButton.onClick.RemoveAllListeners();
            base.OnDisable();
            
        }

        public void UpdateUI()
        {
            this.icon.texture = listSprites[indexSpecialSkill].texture;
            if (indexSpecialSkill == 0)
            {
                descriptionText = "<color=#00FF00>Attract items</color> into hole for 12 seconds";
                this.title.text = "Booster Scale";
            }
            else if (indexSpecialSkill == 1)
            {
                descriptionText = "<color=#00FF00>Attract items</color> into hole for 12 seconds";
                this.title.text = "Booster Magnet";
            }
            else if (indexSpecialSkill == 2)
            {
                descriptionText = "<color=#00FF00>Attract items</color> into hole for 12 seconds";
                this.title.text = "Booster Location";
            }
            else
            {
                descriptionText = "<color=#00FF00>Attract items</color> into hole for 12 seconds";
                this.title.text = "Booster Ice";
            }
            
        }

        
    }
}