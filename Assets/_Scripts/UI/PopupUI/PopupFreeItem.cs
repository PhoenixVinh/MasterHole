using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.UI.PauseGameUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PopupUI
{
    public class PopupFreeItem : MonoBehaviour
    {
        public RawImage icon;
        public TMP_Text description;
        public Button continueButton;
        
        [Header("Sprite For Icon")] public List<Sprite> listSprites;
        
        int indexSpecialSkill;
        private string descriptionText;
        public void SetData(int indexSpecialSkill)
        {
        
            indexSpecialSkill = indexSpecialSkill;
            continueButton.onClick.AddListener(AddBoosterItem);
            UpdateUI();
        }

        private void UpdateUI()
        {
            this.icon.texture = listSprites[indexSpecialSkill].texture;
            if (indexSpecialSkill == 0)
            {
                descriptionText = "Increase the size of the hole for 10 seconds";
             
            }
            else if (indexSpecialSkill == 1)
            {
                descriptionText = "Increase the size of the hole for 10 seconds";
             
            }
            else if (indexSpecialSkill == 2)
            {
                descriptionText = "Increase the size of the hole for 10 seconds";
           
            }
            else
            {
                descriptionText = "Increase the size of the hole for 10 seconds";
              
            }
            this.description.text = descriptionText;
            
        }

        public void OnDisable()
        {
            continueButton.onClick.RemoveAllListeners();
        }

        private void AddBoosterItem()
        {
            
            
            // Adding Booster
            var boosterDataS0 = Resources.Load<BoosterDataSO>("BoosterSO/BoosterData");

            boosterDataS0.Boosters[indexSpecialSkill].Amount += 3;
            
            
            this.gameObject.SetActive(false);
            
        }
    }
}