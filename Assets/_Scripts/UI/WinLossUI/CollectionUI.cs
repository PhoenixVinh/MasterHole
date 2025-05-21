using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public struct ItemCollection
{
    public int levelUnlock;
    public Sprite sprite;
}


namespace _Scripts.UI.WinLossUI
{
    public class CollectionUI : MonoBehaviour
    {
        public List<ItemCollection> items;

        public Image image;

        public GameObject content;
        public Button continueButton;
        public Button homeBtn;

        public void OnEnable()
        {
            continueButton.onClick.AddListener(ShowNextlevel);
            homeBtn.onClick.AddListener(ChangeHomeScene);
        }


        public bool CanShowContent(int level)
        {
           
            foreach (ItemCollection item in items)
            {
                if (item.levelUnlock == level)
                {
                    image.sprite = item.sprite;
                    return true;
                }
            }

            return false;
        }

        public void ShowContent()
        {
            this.content.SetActive(true);
        }
        
        private void ChangeHomeScene()
        {
            SceneManager.LoadScene(EnumScene.HomeScene.ToString());
         
        }
        
        private void ShowNextlevel()
        {
            
            this.gameObject.SetActive(false);
            // Change Data Level 

            ManagerLevelGamePlay.Instance.LoadNextLevel();
           
        }
        

    }
}