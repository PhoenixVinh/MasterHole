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
        public TMP_Text title;
        public TMP_Text description;
        public Button continueButton;
        
        [Header("Sprite For Icon")] public List<Sprite> listSprites;
        
        int indexSpecialSkill;
        private string descriptionText;
        public void SetData(int indexSpecialSkill)
        {
        
            this.indexSpecialSkill = indexSpecialSkill;
            continueButton.onClick.AddListener(AddBoosterItem);
            UpdateUI();
        }

        private void UpdateUI()
        {
            this.icon.texture = listSprites[indexSpecialSkill].texture;
            if (indexSpecialSkill == 0)
            {
                descriptionText = "<color=#48F101>Increase the hole</color> size over 15 seconds";
                this.title.text = "Booster Scale";
            }
            else if (indexSpecialSkill == 1)
            {
                descriptionText = "<color=#48F101>Pull in distant</color> items using the hole for 12 seconds";
                this.title.text = "Booster Magnet";
            }
            else if (indexSpecialSkill == 2)
            {
                descriptionText = "<color=#48F101>Reveal the locations</color> of mission objects for 15 seconds";
                this.title.text = "Booster Location";
            }
            else
            {
                descriptionText = "<color=#48F101>Attract items</color> freeze for 12 seconds";
                this.title.text = "Booster Ice";
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