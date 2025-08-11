using System;
using _Scripts.Event;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.ResourcesUI
{
    public class ShopGold : MonoBehaviour
    {
        public TMP_Text goldText;

        public void OnEnable()
        {
            goldText.text = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 1).ToString();
            UpdateUI();
            ResourceEvent.OnUpdateResource += UpdateUI;
        }

        public void OnDisable()
        {
            ResourceEvent.OnUpdateResource -= UpdateUI;
        }

        private void UpdateUI()
        {
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 1);
            goldText.text = coin.ToString();
        }
    }
}