using _Scripts.UI;
using UnityEngine;

namespace _Scripts.HACK
{
    public class AddCoinBtn : MonoBehaviour
    {
        public int coin = 1000000;
        public void AddGold()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, coin);
        }
    }
}