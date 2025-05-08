using System;
using _Scripts.Booster;
using _Scripts.UI.PopupUI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace _Scripts.UI.BoosterUI
{
    public class BoosterUI : MonoBehaviour
    {
        public BoosterData boosterData;

        
        [Header("Booster UI")]
        public GameObject LockUI;
        public TMP_Text levelCanUseText;
        public GameObject buttonUse;

        [Header("Amount Item UI")] 
        public GameObject hasItem;
        public TMP_Text amountText;
        public GameObject noItem;
        
        
        
        
        private int amount = 0;
        private string name = "Booster";
        private int levelCanUse = 0; 
        private int indexSpecialSkill = 0;

        private void Start()
        {
            buttonUse.GetComponent<Button>().onClick.AddListener(UseSpecialSkill);
        }


        public void SetData(BoosterData boosterData, int indexSpecialSkill)
        {
            this.amount = boosterData.Amount;
            this.name = boosterData.Name;
            this.levelCanUse = boosterData.LevelCanUse;
         
            this.indexSpecialSkill = indexSpecialSkill;
            UpdateUI();
        }

       

        private void UseSpecialSkill()
        {
            if (HoleController.Instance.IsProcessSkill(indexSpecialSkill)) return;
            // Check Should Show PopUp Or Not 

            if (this.amount <=0)
            {
                ManagerPopup.Instance.ShowPopupBuyItemInGame(this.indexSpecialSkill);
            }
            else
            {
                HoleController.Instance.ProcessSkill(this.indexSpecialSkill);
                amount--;
                ManagerBooster.Instance.ChangeAmountBooster(indexSpecialSkill, -1);
                UpdateUI();
            }
            
        }


        public void UpdateUI()
        {
            int CurrentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            
            if (CurrentLevel < levelCanUse)
            {
                this.LockUI.SetActive(true);
                levelCanUseText.text = $"Lv.{levelCanUse}";
                this.buttonUse.SetActive(false);
            
                
            }
            else
            {
                this.LockUI.SetActive(false);
                this.buttonUse.SetActive(true);
                

                if (this.amount >= 1)
                {
                    this.noItem.SetActive(false);
                    this.hasItem.SetActive(true);
                    this.amountText.text = this.amount.ToString();
                }
                else
                {
                    this.noItem.SetActive(true);
                    this.hasItem.SetActive(false);
                }
            }
        }
        
    }
}