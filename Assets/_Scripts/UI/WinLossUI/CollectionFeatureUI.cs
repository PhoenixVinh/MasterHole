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

            try
            {
                StartCoroutine(ChangeFill(level, collection.GetLevelCollection(level)));
                Icon.sprite = collection.ItemCollectionData[collection.GetLevelCollection(level)].image;
            }
            catch (Exception e)
            {
                this.gameObject.SetActive(false);
            }

          
                
        }

        private IEnumerator ChangeFill(int currentLevel, int currentIndex)
        { 
            float start = currentLevel - 1;
            ItemStatusImage.fillAmount = start/collection.ItemCollectionData[currentIndex].LevelUnlock;
            //yield return new WaitForSecondsRealtime(0.2f);
            levelDes.text = $"Lv {currentLevel}/{collection.ItemCollectionData[currentIndex].LevelUnlock}";
           
            float startValue = ItemStatusImage.fillAmount; // Current fill amount
            float targetValue = (float)currentLevel/collection.ItemCollectionData[currentIndex].LevelUnlock; // Target fill amount
            float elapsedTime = 0f;

            while (elapsedTime < 0.5f)
            {
                elapsedTime += Time.unscaledDeltaTime; // Use unscaled time for real-time updates
                float lerpFactor = elapsedTime /0.5f; // Calculate interpolation factor (0 to 1)
                ItemStatusImage.fillAmount = Mathf.Lerp(startValue, targetValue, lerpFactor); // Smoothly interpolate
                yield return null; // Wait for the next frame
            }

            // Ensure the fill amount reaches the exact target value at the end
            ItemStatusImage.fillAmount = targetValue;

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
            
            if (currentLevel >= collection.ItemCollectionData[collection.ItemCollectionData.Count - 1]
                    .LevelUnlock)
            {
                statusIndex = collection.ItemCollectionData.Count - 1;
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