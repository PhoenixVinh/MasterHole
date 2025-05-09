using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Data.CollectionData;
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

        private CollectionSO collection;

        public void OnEnable()
        {
            collection = Resources.Load<CollectionSO>("CollectionSO/DataCollection");
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

            float start = 0;
            while (start < currentLevel)
            {
                ItemStatusImage.fillAmount = start/collection.ItemCollectionData[currentIndex].LevelUnlock;
                start += speed;
                yield return new WaitForSecondsRealtime(0.02f);;
            }
            
            
        }
    }
}