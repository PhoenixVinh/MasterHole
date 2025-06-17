using System;
using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.Firebase;
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

        public List<GameObject> Titles;
        
        public void SetData(int indexSpecialSkill)
        {
        
            Utills.ChangePositionBoosterFirebase(indexSpecialSkill);
            this.indexSpecialSkill = indexSpecialSkill;
            continueButton.onClick.AddListener(AddBoosterItem);
            UpdateUI();
        }

        private void UpdateUI()
        {
            this.icon.texture = listSprites[indexSpecialSkill].texture;
            if (indexSpecialSkill == 0)
            {
                descriptionText = "Increase the hole size over 15 seconds";
               
            }
            else if (indexSpecialSkill == 1)
            {
                descriptionText = "Pull in distant items using the hole for 12 seconds";
                
            }
            else if (indexSpecialSkill == 2)
            {
                descriptionText = "Reveal the locations of mission objects for 15 seconds";
               
            }
            else
            {
                descriptionText = "Attract items freeze for 12 seconds";
                
            }
            this.description.text = descriptionText;
            TurnOffAllTitle();
            Titles[indexSpecialSkill].SetActive(true);

        }

        public void OnDisable()
        {
            continueButton.onClick.RemoveAllListeners();
            PopupItemEvent.getITem?.Invoke(indexSpecialSkill);
            //ManagerFirebase.Instance.positionPopup = PositionFirebase.none;
        }
        
        
        public void TurnOffAllTitle()
        {
            foreach (var title in Titles)
            {
                title.SetActive(false);
            }
        }

        private void AddBoosterItem()
        {
            
            
            // Adding Booster
            // var boosterDatas =  JsonUtility.FromJson<BoosterDatas>(PlayerPrefs.GetString(StringPlayerPrefs.BOOSTER_DATA));
            //
            // boosterDatas.Boosters[indexSpecialSkill].Amount += 3;
            
            //String convert = JsonUtility.ToJson(boosterDatas);
            //PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, convert);
            ManagerBooster.Instance?.ChangeAmountBooster(indexSpecialSkill,3);
           // ManagerFirebase.Instance?.LogEarnResource(ResourceType.booster,Utills.GetBoosterNameByIndex(this.indexSpecialSkill), "3", Reson.reward);
            this.gameObject.SetActive(false);
            
        }
    }
}