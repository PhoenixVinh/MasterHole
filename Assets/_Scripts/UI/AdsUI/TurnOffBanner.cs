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
            if(ManagerLevelGamePlay.Instance.isShowBanner)
                
                MaxAdsManager.Instance?.ShowBannerAd();
        }
    }
}