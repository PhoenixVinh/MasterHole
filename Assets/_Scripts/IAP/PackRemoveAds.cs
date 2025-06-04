using _Scripts.UI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class PackRemoveAds: PackBundle
    {
        public override void OnBuySuccess()
        {
            base.OnBuySuccess();
            PlayerPrefs.SetInt(StringPlayerPrefs.REMOVED_ADS_PACK, 1);
        }
    }
}