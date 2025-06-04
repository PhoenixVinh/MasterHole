using _Scripts.UI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class StarterPack: PackBundleVip
    {
        public override void OnBuySuccess()
        {
            base.OnBuySuccess();
            PlayerPrefs.SetInt(StringPlayerPrefs.STARTER_DEAL_PACK, 1);
        }
    }
}