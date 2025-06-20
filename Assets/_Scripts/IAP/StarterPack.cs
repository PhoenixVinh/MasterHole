using _Scripts.Booster;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class StarterPack: PackBundleVip
    {
        public override void OnBuySuccess()
        {
            
            PlayerPrefs.SetInt(StringPlayerPrefs.STARTER_DEAL_PACK, 1);
            ManagerBooster.Instance?.SetData();
            base.OnBuySuccess();
        }
    }
}