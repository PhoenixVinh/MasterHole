using _Scripts.Event;
using _Scripts.UI;
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
            
        }
    }
}