using System.Collections.Generic;
using _Scripts.Event;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.IAP
{
    public class PackRemoveAds: PackBundle
    {
        public override void OnBuySuccess()
        {
            
            PlayerPrefs.SetInt(StringPlayerPrefs.REMOVED_ADS_PACK, 1);
            base.OnBuySuccess();
        }
        
        
        public override void PlayAnim()
        {
            List<DataReward> data = new List<DataReward>();
            data.Add(new DataReward
            {
                id = 0,
                amound = gold.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 1,
                amound ="x" +  amount_BScale.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 2,
                amound ="x" +  amount_BMagnet.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 3,
                amound ="x" +  amount_BLocation.ToString(),
            });
            data.Add(new DataReward()
            {
                id = 5,
                amound = "Ads Removed",
            });
            UIEvent.OnRewardedSuccess?.Invoke(data);
            MaxAdsManager.Instance.isRemoveInter = true;
        }
    }
}