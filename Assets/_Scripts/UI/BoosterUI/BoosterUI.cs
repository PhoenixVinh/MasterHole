using System;
using System.Collections;
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

        public GameObject effectSnow;
        
        
        public BoosterData boosterData;
        public float time = 15f;
        
        [Header("Booster UI")]
        public GameObject LockUI;
        public TMP_Text levelCanUseText;
        public GameObject buttonUse;

        [Header("Amount Item UI")] 
        public GameObject hasItem;
        public TMP_Text amountText;
        public GameObject noItem;
        public Image FillAmount;
        
        
        
        private int amount = 0;
        private string name = "Booster";
        private int levelCanUse = 0; 
        private int indexSpecialSkill = 0;

        private void Start()
        {
            FillAmount.fillAmount = 0;
            buttonUse.GetComponent<Button>().onClick.AddListener(UseSpecialSkill);
        }


        private void OnEnable()
        {
            HoleEvent.OnTurnOffSkillUI += TurnOffUIProcess;
        }

        private void OnDisable()
        {
            HoleEvent.OnTurnOffSkillUI -= TurnOffUIProcess;
        }

        private void TurnOffUIProcess()
        {
            StopAllCoroutines();
            FillAmount.fillAmount = 0;
            if (indexSpecialSkill == 3)
            {
                effectSnow.SetActive(false);
            }
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
            
            
            
            

            if (this.amount <=0)
            {
                ManagerPopup.Instance.ShowPopupBuyItemInGame(this.indexSpecialSkill);
            }
            else
            {
                StartCoroutine(ShowFillAmount());
                HoleController.Instance.ProcessSkill(this.indexSpecialSkill);

                if (indexSpecialSkill == 3)
                {
                    effectSnow.SetActive(true);
                }
                amount--;
                ManagerBooster.Instance.ChangeAmountBooster(indexSpecialSkill, -1);
                UpdateUI();
            }
            
        }

        private IEnumerator ShowFillAmount()
        {
            float startTime = 0;
            while (startTime < time)
            {
                float currentamount = FillAmount.fillAmount;
                
                startTime += Time.deltaTime;
                FillAmount.fillAmount = 1 -  startTime / time;
                yield return null;
            }
            if(effectSnow != null)
                effectSnow.SetActive(false);
            FillAmount.fillAmount = 0;
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