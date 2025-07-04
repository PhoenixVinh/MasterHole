using System;
using UnityEngine;

namespace _Scripts.UI.AdsUI
{
    public class TurnOffBanner : MonoBehaviour
    {
        public void OnEnable()
        {
            MaxAdsManager.Instance?.HideBannerAd();
            //Destroy(gameObject);
        }

        public void OnDisable()
        {
            MaxAdsManager.Instance?.ShowBannerAd();
        }
    }
}