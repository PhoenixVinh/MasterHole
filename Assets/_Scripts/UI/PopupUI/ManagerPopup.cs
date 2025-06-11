using System;
using UnityEngine;

namespace _Scripts.UI.PopupUI
{
    public class ManagerPopup: MonoBehaviour
    {
        public static ManagerPopup Instance;
        public PopupItemInGame popupBuyItemInGame;
        public PopupFreeItem popupFreeItem;
        public PopupBuyEnergy popupBuyEnergy;
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
    }
}