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

        private CollectionSO collection;

        public void OnEnable()
        {
            collection = Resources.Load<CollectionSO>("CollectionSO/DataCollection");
            ItemStatusImage.fillAmount = 0;
            SetData();
            
        }

        private void SetData()
        {
            int currentIndex = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COLLECTION, 0);
            Icon.sprite = ItemSprites[currentIndex];
            Icon.color =  new Color(0.6f, 0.6f,0.6f,1f);
            
            int currentLevel =  PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 0);

            StartCoroutine(ChangeFill(currentLevel, currentIndex));
        }

        private IEnumerator ChangeFill(int currentLevel, int currentIndex)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            levelDes.text = $"Lv {currentLevel}/ {collection.ItemCollectionData[currentIndex].LevelUnlock}";
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
            
        }
    }
}