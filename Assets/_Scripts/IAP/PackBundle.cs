using _Scripts.Booster;
using _Scripts.Event;
using _Scripts.UI;
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
            base.OnBuySuccess();
            var BoosterDatas = JsonUtility.FromJson<BoosterDatas>(PlayerPrefs.GetString(StringPlayerPrefs.BOOSTER_DATA));
            BoosterDatas.Boosters[0].Amount += amount_BScale;
            BoosterDatas.Boosters[1].Amount += amount_BMagnet;
            BoosterDatas.Boosters[2].Amount += amount_BLocation;
            BoosterDatas.Boosters[3].Amount += amount_BICe;
            
            string convert = JsonUtility.ToJson(BoosterDatas);
            PlayerPrefs.SetString(StringPlayerPrefs.BOOSTER_DATA, convert);
            ResourceEvent.OnUpdateResource?.Invoke();
        }
        
    }
}