using System;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class PackBundleVip : PackBundle
    {
        public int timeInfinity;

        public override void OnBuySuccess()
        {
            base.OnBuySuccess();
            
            
            DateTime infi = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            if (infi < DateTime.Now)
            {
                infi = DateTime.Now;
            }
            infi = infi.AddSeconds(timeInfinity);
            
            PlayerPrefs.SetString(StringPlayerPrefs.UNLIMITED_TIME, infi.ToString());
            Energy.Instance?.LoadInfinity();
        }
    }
}