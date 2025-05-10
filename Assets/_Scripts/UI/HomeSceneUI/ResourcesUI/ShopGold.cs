using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.ResourcesUI
{
    public class ShopGold : MonoBehaviour
    {
        public TMP_Text goldText;

        public void OnEnable()
        {
            int coin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 1);
            goldText.text = coin.ToString();
        }
        
    }
}