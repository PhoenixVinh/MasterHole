using _Scripts.UI;
using UnityEngine;

namespace _Scripts.HACK
{
    public class AddCoinBtn : MonoBehaviour
    {
        public int coin = 10000;
        public void AddGold()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
        }
    }
}