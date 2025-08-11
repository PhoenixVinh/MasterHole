using System.Collections.Generic;
using _Scripts.Event;
using _Scripts.Firebase;
using _Scripts.UI;
using _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI;
using UnityEngine;

namespace _Scripts.IAP
{
    public class PackGold: MonoBehaviour, IPack
    {
        public int gold;

        public virtual void OnBuySuccess()
        {
            int currentGold = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN);
            currentGold += gold;
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, currentGold);
            ResourceEvent.OnUpdateResource?.Invoke();

            PlayAnim();
            LogFirebase();
        }


        public virtual void PlayAnim()
        {
            UIEvent.OnRewardedSuccess?.Invoke(new List<DataReward>
            {
                new DataReward
                {
                    id = 0,
                    amound = gold.ToString(),
                }
            });
        }

        public virtual void LogFirebase()
        {
            ManagerFirebase.Instance?.LogEarnResource(ResourceType.currency, ResourceName.Coin.ToString(), gold.ToString(), Reson.purchase);
        }
    }
}