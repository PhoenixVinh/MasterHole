using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.ManagerScene;
using _Scripts.ManagerScene.HomeScene;
using _Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Tutorial
{
    public class ManagerTutorial : MonoBehaviour
    {
        public List<GameObject> tutorials;
        
        public static ManagerTutorial Instance;

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

        public async void ShowTutorials(int index)
        {
            TurnOffTutorials();
            if (index <= tutorials.Count)
            {
                await Task.Delay(500);
                tutorials[index-1].SetActive(true);
            }

            if (index == 3)
            {
               
                int checkTutorialLv3 = PlayerPrefs.GetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3, 0);
                if (checkTutorialLv3 == 0)
                {
                    
                    PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3, 0);
                    PlayerPrefs.Save();
                    
                    SceneManager.LoadScene(EnumScene.HomeScene.ToString());
                    ManagerHomeScene.Instance.LoadTutorial();
                }
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