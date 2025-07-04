using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI;
using _Scripts.UI.PopupUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Tutorial
{
    public class ManagerTutorial : MonoBehaviour
    {
        public List<GameObject> tutorials;
        
        
        
        public static ManagerTutorial Instance;


        public GameObject LevelCoin;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnEnable()
        {
            PopupItemEvent.getITem += ShowTut;
        }

        public void OnDisable()
        {
            PopupItemEvent.getITem -= ShowTut;
        }

        private void ShowTut(int index)
        {
            tutorials[index +2].SetActive(true);
        }

        public async Task<bool> ShowTutorials(int index)
        {
            TurnOffTutorials();
            if (index <= 2)
            {
                await Task.Delay(500);
                tutorials[index-1].SetActive(true);
                return false;
            }

            if (index == 3)
            {
                SetTutorialFreeIcon(StringPlayerPrefs.TUTORIAL_LEVEL_3, 0, 2);
                return true;
            }

           
            

            if (index == 5)
            {
                SetTutorialFreeIcon(StringPlayerPrefs.TUTORIAL_LEVEL_5, 1, 3);
                return true;
            }

            if (index == 7)
            {
                SetTutorialFreeIcon(StringPlayerPrefs.TUTORIAL_LEVEL_7, 2, 4);
                return true;
            }

            if (index == 9)
            {
                SetTutorialFreeIcon(StringPlayerPrefs.TUTORIAL_LEVEL_9, 3, 3);
                return true;
            }
            

            // if (index % 20 == 0 && index >= 20 )
            // {
            //     LevelCoin.SetActive(true);
            // }
            // else
            // {
            //     LevelCoin.SetActive(false);
            // }

            return false;

        }

        public async void SetTutorialFreeIcon(string keyTutorial, int
            indexFree, int indexTutorial)
        {
            
            
            if (!PlayerPrefs.HasKey(keyTutorial))
            {
                PlayerPrefs.SetInt(keyTutorial, 0);
            }
                
            int checkTutorial = PlayerPrefs.GetInt(keyTutorial, 0);
            if (checkTutorial == 0)
            {
                    
                PlayerPrefs.SetInt(keyTutorial, 1);
                
                await Task.Delay(1200);
                ManagerPopup.Instance.ShowPopupFreeItem(indexFree);
                
                //SceneManager.LoadScene(EnumScene.HomeScene.ToString());
                
                //ManagerHomeScene.Instance?.ShowPopUpFreeITem(indexFree);
            }
            
        }

        public void TurnOffTutorials()
        {
            foreach (var tutorial in tutorials)
            {
                tutorial.SetActive(false);
            }
        }
        
        
    }
}