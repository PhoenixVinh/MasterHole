using _Scripts.UI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class LegendaryPack : PackBundleVip
    {
        public override void OnBuySuccess()
        {
            base.OnBuySuccess();
            int getRemoveAds = PlayerPrefs.GetInt(StringPlayerPrefs.REMOVED_ADS_PACK);
            int getRemovesAdsVip = PlayerPrefs.GetInt(StringPlayerPrefs.REMOVED_ADS_VIP);
            if (getRemoveAds == 1 || getRemovesAdsVip == 1)
            {
                int currentCoint = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
                currentCoint += 1500;
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, currentCoint);
            }
            else
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.REMOVED_ADS_VIP, 1);
            }
        }
    }
}