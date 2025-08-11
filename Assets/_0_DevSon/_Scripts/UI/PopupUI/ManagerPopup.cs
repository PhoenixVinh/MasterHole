using System;
using _Scripts.Firebase;
using _Scripts.UI.WinLossUI;
using UnityEngine;

namespace _Scripts.UI.PopupUI
{
    public class ManagerPopup: MonoBehaviour
    {
        public static ManagerPopup Instance;
        public PopupItemInGame popupBuyItemInGame;
        public PopupFreeItem popupFreeItem;
        public PopupBuyEnergy popupBuyEnergy;
        public GameObject popupRateUs;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowPopupBuyItemInGame(int index)
        {
            popupBuyItemInGame.SetData(index);
            popupBuyItemInGame.gameObject.SetActive(true);
        }

        public void ShowPopupFreeItem(int index)
        {
            popupFreeItem.SetData(index);
            popupFreeItem.gameObject.SetActive(true);
        }

        public void ShowPopupBuyEnergy()
        {
            popupBuyEnergy.gameObject.SetActive(true);
        }

        public void TurnOffPopup()
        {
            popupBuyItemInGame.gameObject.SetActive(false);
            popupFreeItem.gameObject.SetActive(false);
        }

        public void ShowPopupRateUs()
        {
            int IsRateUs = PlayerPrefs.GetInt(StringPlayerPrefs.IS_RATE_US, 0);
            if (IsRateUs != 0) return;
            
            
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 2);
            int firstRate = ManagerFirebase.Instance.firebaseInitial.first_popup_rate;
            int distanceRate = ManagerFirebase.Instance.firebaseInitial.distance_popup_rate;
            if(currentLevel == firstRate)
            {
                popupRateUs.SetActive(true);
            }
            
            if(currentLevel > firstRate && 
               (currentLevel - firstRate) % distanceRate == 0)
            {
                popupRateUs.SetActive(true);
            }
            
        }
        
        
        
    }
}