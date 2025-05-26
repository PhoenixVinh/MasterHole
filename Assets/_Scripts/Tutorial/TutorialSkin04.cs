using System;
using _Scripts.UI;
using _Scripts.UI.HoleSkinUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Tutorial
{
    public class TutorialSkin04 : MonoBehaviour
    {
        public Button btnEquip;
        
        public void OnEnable()
        {
            btnEquip.onClick.AddListener(SetSkin);
        }

        private  void SetSkin()
        {
            
            PlayerPrefs.SetInt(StringPlayerPrefs.HOLESKININDEX, 1);
            
            this.gameObject.SetActive(false);
        }
    }
}