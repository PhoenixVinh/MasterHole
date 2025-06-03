using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Data.CollectionData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.WinLossUI
{
    public class CollectionFeatureUI : MonoBehaviour
    {
        public List<Sprite> ItemSprites;

        public Image Icon;
        public Image ItemStatusImage;

        public float speed;

        public TMP_Text levelDes;
        public GameObject lockIcon;

        public  CollectionSO collection;

        public void OnEnable()
        {
            
            ItemStatusImage.fillAmount = 0;
            
            
        }

        public void SetData(int level)
        {
           
            
            Icon.color =  new Color(0.6f, 0.6f,0.6f,1f);
            
         

            StartCoroutine(ChangeFill(level, collection.GetLevelCollection(level)));
            Icon.sprite  = collection.ItemCollectionData[collection.GetLevelCollection(level)].image;
                
        }

        private IEnumerator ChangeFill(int currentLevel, int currentIndex)
        {
            
            yield return new WaitForSecondsRealtime(0.2f);
            levelDes.text = $"Lv {currentLevel}/{collection.ItemCollectionData[currentIndex].LevelUnlock}";
            float start = 0;
            while (start < currentLevel)
            {
                ItemStatusImage.fillAmount = start/collection.ItemCollectionData[currentIndex].LevelUnlock;
                start += speed;
                yield return new WaitForSecondsRealtime(0.02f);;
            }

            if (currentLevel == collection.ItemCollectionData[currentIndex].LevelUnlock)
            {
                Icon.color =  Color.white;
                lockIcon.SetActive(false);
            }
            ResetData(currentLevel);
           
        }
        public void ResetData(int currentLevel)
        {
            int statusIndex = 0;
            for (int i = 0; i < collection.ItemCollectionData.Count; i++)
            {
                if (currentLevel >= collection.ItemCollectionData[i].LevelUnlock)
                {
                    collection.ItemCollectionData[i].Lock = false;
                }
                else if (currentLevel < collection.ItemCollectionData[i].LevelUnlock)
                {
                    collection.ItemCollectionData[i].Lock = false;

                    statusIndex = i;
                    break;
                }
                
            }
            
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COLLECTION,statusIndex);
            
            statusIndex++;
            while (statusIndex < collection.ItemCollectionData.Count)
            {
                collection.ItemCollectionData[statusIndex].Lock = true;
                statusIndex++;
            }
            
           
        }
    }
}