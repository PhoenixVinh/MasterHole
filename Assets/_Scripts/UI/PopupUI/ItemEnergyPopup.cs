using TMPro;
using UnityEngine;

namespace _Scripts.UI.PopupUI
{
    public class ItemEnergyPopup : MonoBehaviour
    {
        public GameObject hasEnergy;
        public GameObject noEnergy;


        public GameObject Timmer;
        public TMP_Text energyText;


        public void SetData(bool turnOn, string timer)
        {
            if (turnOn)
            {
                hasEnergy.SetActive(true);
                noEnergy.SetActive(false);
            }
            else
            {
                hasEnergy.SetActive(false);
                noEnergy.SetActive(true);
                if (timer != "")
                {
                    energyText.text = timer;
                    Timmer.SetActive(true);
                }
                else
                {
                    Timmer.SetActive(false);
                }
               
            }
        }
    }
}