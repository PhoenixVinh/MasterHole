using System.Collections.Generic;
using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.Firebase;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class PackBundle : PackGold
    {


       
        public int amount_BScale;
        public int amount_BLocation;
        public int amount_BMagnet;
        public int amount_BICe;
        
        
        public override void OnBuySuccess()
        {
            
            var BoosterDatas = JsonUtility.FromJson<BoosterDatas>(PlayerPrefs.GetString(StringPlayerPrefs.BOOSTER_DATA));
            BoosterDatas.Boosters[0].Amount += amount_BScale;
            BoosterDatas.Boosters[1].Amount += amount_BMagnet;
            BoosterDatas.Boosters[2].Amount += amount_BLocation;
            BoosterDatas.Boosters[3].Amount += amount_BICe;
            
            string convert = JsonUtility.ToJson(BoosterDatas);
            PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, convert);
            ResourceEvent.OnUpdateResource?.Invoke();
            ManagerBooster.Instance?.SetData();
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
                amound ="x" +  amount_BLocation.ToString(),
            });
            UIEvent.OnRewardedSuccess?.Invoke(data);
            
        }

        public override void LogFirebase()
        {
            string resourceName = ResourceName.Coin.ToString()+ ","+ ResourceName.Scale.ToString()+","
                + ResourceName.Magnet.ToString() + "," + ResourceName.Location.ToString();
            string amount = this.gold.ToString() + "," + amount_BScale.ToString() + ","
                + amount_BMagnet.ToString() + "," + amount_BLocation.ToString();
            
            ManagerFirebase.Instance?.LogEarnResource(ResourceType.currency, resourceName, amount, Reson.purchase);
        }
        
        
        
    }
}