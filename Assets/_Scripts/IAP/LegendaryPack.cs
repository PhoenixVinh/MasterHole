using System;
using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;
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

            ManagerBooster.Instance?.SetData();
        }
        
        public override void PlayAnim()
        {
            TimeSpan t = TimeSpan.FromSeconds(timeInfinity-1);
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
                amound ="x" + amount_BMagnet.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 3,
                amound ="x" + amount_BLocation.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 4,
                amound = formatted
            });
            data.Add(new DataReward()
            {
                id = 5,
                amound = "Ads Removed",
            });
        
            UIEvent.OnRewardedSuccess?.Invoke(data);
            MaxAdsManager.Instance.isRemoveAds = true;
            
        }
    }
    
   
}