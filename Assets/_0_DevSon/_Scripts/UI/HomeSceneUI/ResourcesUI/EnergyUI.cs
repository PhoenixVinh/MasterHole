using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.ResourcesUI
{
    public class EnergyUI : MonoBehaviour
    {
        [SerializeField] TMP_Text energyText;
        [SerializeField] TMP_Text timeText;

      
        
        
        [Header("InFinityHealth")]
        [SerializeField] private GameObject infinityHealth;
        [SerializeField] private GameObject adding;

        
       

        public void FixedUpdate()
        {
            UpdateEnergyTimer();
            UpdateEnergy();
        }
        private void UpdateEnergy()
        {
            energyText.text = Energy.Instance.CurrentEnergy.ToString();
        }
        
        
        private void UpdateEnergyTimer()
        {
            if (Energy.Instance.IsUnlimitedEnergy)
            {
                timeText.text = Energy.Instance.TimeValue;
               
                infinityHealth.SetActive(true);
                adding.SetActive(false);

                return;

            }
            else
            {
                infinityHealth.SetActive(false);
                adding.SetActive(true);
            }
            
            
            
            
            if (Energy.Instance.IsFullEnergy)
            {
                timeText.text = "FULL";
                return;
            }

            
            timeText.text = Energy.Instance.TimeValue;
        }

    }
}