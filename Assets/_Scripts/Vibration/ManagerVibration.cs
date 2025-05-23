using System;
using _Scripts.UI;
using CandyCoded.HapticFeedback;
using UnityEngine;

namespace _Scripts.Vibration
{


    public enum EnumVibration
    {
        Light = 0, 
        Medium = 1,
        Heavy = 2,
    }
    
    
    
    public class ManagerVibration : MonoBehaviour
    {

        public static ManagerVibration Instance;

        public bool canVibrate = true;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        
        {
            if (!PlayerPrefs.HasKey(StringPlayerPrefs.USE_VIBRATION))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.USE_VIBRATION, 1);
            }
            canVibrate = PlayerPrefs.GetInt(StringPlayerPrefs.USE_VIBRATION, 1) == 1; 
        }

       

        public void UseVibration(EnumVibration typeVibration)
        {
            if (!canVibrate) return;
            if (typeVibration == EnumVibration.Light)
            {
                HapticFeedback.LightFeedback();
            }
            else if (typeVibration == EnumVibration.Medium)
            {
                HapticFeedback.MediumFeedback();
            }
            else if (typeVibration == EnumVibration.Heavy)
            {
                HapticFeedback.HeavyFeedback();
            }
        }

        public void SetVibration(bool value)
        {
            canVibrate = value;
            PlayerPrefs.SetInt(StringPlayerPrefs.USE_VIBRATION, value == true ? 1 : 0);
        }

        
    }
}