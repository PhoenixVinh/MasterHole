using System;
using System.Collections.Generic;
using _Scripts.Data.CollectionData;
using _Scripts.UI;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Collection
{
    public class ManagerColllection : MonoBehaviour
    {

        public List<ItemCollection> UIItemCollections;

        public CollectionSO dataCollection;


        private int currentLevel = 1;

        public void OnEnable()
        {
            LoadData();
            ResetData();
            SetDataUI();
            
        }


        public void LoadData()
        {
            currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL,1);
           
        }

        public void ResetData()
        {
            int statusIndex = 0;
            for (int i = 0; i < dataCollection.ItemCollectionData.Count; i++)
            {
                if (currentLevel >= dataCollection.ItemCollectionData[i].LevelUnlock)
                {
                    dataCollection.ItemCollectionData[i].Lock = false;
                }
                else if (currentLevel < dataCollection.ItemCollectionData[i].LevelUnlock)
                {
                    dataCollection.ItemCollectionData[i].Lock = false;

                    statusIndex = i;
                    break;
                }

                

            }

            if (currentLevel >= dataCollection.ItemCollectionData[dataCollection.ItemCollectionData.Count - 1]
                    .LevelUnlock)
            {
                statusIndex = dataCollection.ItemCollectionData.Count - 1;
            }
            
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COLLECTION,statusIndex);
            
            statusIndex++;
            while (statusIndex < dataCollection.ItemCollectionData.Count)
            {
                dataCollection.ItemCollectionData[statusIndex].Lock = true;
                statusIndex++;
            }
            
           
        }

        public void SetDataUI()
        {
            for (int i = 0; i < dataCollection.ItemCollectionData.Count; i++)
            {
                UIItemCollections[i].SetData(dataCollection.ItemCollectionData[i], currentLevel);
            }
        }
        
        
    }
}