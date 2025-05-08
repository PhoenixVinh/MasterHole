using System;
using _Scripts.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Collection
{
    public class ItemCollection : MonoBehaviour
    {
        
        
        [Header("GameObject Status")]
        public Image ItemStatusImage;
        public GameObject Check;
        public GameObject Lock;
        
        [Header("Processbar Status")]
        public GameObject ProcressBarMain;
        public Image ProcessBar;
        public TMP_Text ProcessText;

        private int currentLevel;
        
        private ItemCollectionData dataCollection;
        
        private void Start()
        {
            Check.SetActive(false);
            Lock.SetActive(false);
            ProcressBarMain.SetActive(false);
            
        }

        public void SetData(ItemCollectionData dataCollection, int currentLevel)
        {
            this.dataCollection = dataCollection;
            this.currentLevel = currentLevel;
            SetUI();
        }

        private void SetUI()
        {
            if (dataCollection.Lock)
            {
                this.Lock.SetActive(true);
                this.ProcressBarMain.SetActive(false);
                this.Check.SetActive(false);
                ItemStatusImage.color = new Color(0.25f, 0.25f, 0.25f, 1f);
            }
            else
            {
                ItemStatusImage.color = Color.white;
                if (currentLevel >= dataCollection.LevelUnlock)
                {
                    Check.SetActive(true);
                    
                }
                else
                {
                    Check.SetActive(false);
                    ProcessBar.fillAmount = (float)currentLevel / dataCollection.LevelUnlock;
                    ProcessText.text = $"Lv {currentLevel}/{dataCollection.LevelUnlock}";
                }
                
            }
        }
        
    }
}