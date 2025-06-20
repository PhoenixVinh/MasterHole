using System;
using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class PackBundleVip : PackBundle
    {
        public int timeInfinity;

        public override void OnBuySuccess()
        {
           
            
            
            DateTime infi = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            if (infi < DateTime.Now)
            {
                infi = DateTime.Now;
            }
            infi = infi.AddSeconds(timeInfinity);
            
            PlayerPrefs.SetString(StringPlayerPrefs.UNLIMITED_TIME, infi.ToString());
            Energy.Instance?.LoadInfinity();
            ManagerBooster.Instance?.SetData();
            base.OnBuySuccess();
        }
        
        public override void PlayAnim()
        {
            TimeSpan t = TimeSpan.FromSeconds(timeInfinity);
            string formatted = t.ToString(@"hh\:mm\:ss");
            List<DataReward> data = new List<DataReward>();
            data.Add(new DataReward
            {
                id = 0,
                amound = gold.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 1,
                amound = "x" + amount_BScale.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 2,
                amound = "x" + amount_BMagnet.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 3,
                amound = "x" + amount_BLocation.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 4,
                amound = formatted,
                
            });
            
            UIEvent.OnRewardedSuccess?.Invoke(data);
        }
        
        
    }
}