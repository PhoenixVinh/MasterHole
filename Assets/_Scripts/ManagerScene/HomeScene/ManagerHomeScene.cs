using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Firebase;
using _Scripts.Sound;
using _Scripts.Tutorial;
using _Scripts.UI;
using _Scripts.UI.AnimationUI;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
using _Scripts.UI.PopupUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.ManagerScene.HomeScene
{
    public class ManagerHomeScene: MonoBehaviour
    {
        public static ManagerHomeScene Instance;
        public TutorialSkin04 tutorialSkin04;

        public GameObject ShowLoseGame;


        public GameObject LoadingUI;
        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad (this);
            } else {
                Destroy (gameObject);
            }

            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (currentLevel == 1)
            {
                SceneManager.LoadScene(EnumScene.PlayScene.ToString());
                ManagerFirebase.Instance?.ChangePositionFirebase(PositionFirebase.ingame);
                //ManagerSound.Instance?.ChangeBackgroundMusic(EnumBackgroundSound.InGameMusic);
                
            }
            //ShowLoseGame.SetActive(false);
        }

        


        private async void OnEnable()
        {
            //LoadTutorial();
        }

        public async void LoadTutorial()
        {
           
            int CurrentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (CurrentLevel == 3)
            {
                ShowTutorialByLevel(StringPlayerPrefs.TUTORIAL_LEVEL_3, 1);
            }
            if (CurrentLevel == 4)
            {
                //ShowTutorialSkin();

            }
            if (CurrentLevel == 5)
            {
                ShowTutorialByLevel(StringPlayerPrefs.TUTORIAL_LEVEL_5, 1);
            }

           

            if (CurrentLevel == 7)
            {
                ShowTutorialByLevel(StringPlayerPrefs.TUTORIAL_LEVEL_7, 2);
            }

            if (CurrentLevel == 9)
            {
                ShowTutorialByLevel(StringPlayerPrefs.TUTORIAL_LEVEL_9, 3);
            }
        }

        public void ShowTutorialByLevel(string LevelTutorial, int indexFree)
        {
            int showTutorialLv = PlayerPrefs.GetInt(LevelTutorial);
            if (showTutorialLv == 0)
            {
                PlayerPrefs.SetInt(LevelTutorial, 1);
                ShowPopUpFreeITem(indexFree);
            }
        }

        public async void ShowPopUpFreeITem(int index)
        {
            HideLoadingUI();
            await Task.Delay(1000);
           
            ManagerPopup.Instance?.ShowPopupFreeItem(index);
            //PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3, 1);
        }

        public async void ShowRewardCoin(int amount)
        {
            
            int currentCoin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 1);

            await Task.Delay(200);
            RewardCoinEvent.OnRewardCoin?.Invoke(currentCoin - amount, currentCoin);
            
           
        }
        public async void ShowLoseGameUI()
        {
            ShowLoseGame.SetActive(true);
            await Task.Delay(3000);
            ShowLoseGame.SetActive(false);
            Resource.Instance.MinusHealth();
            
        }

        public async void ShowTutorialSkin()
        {

            
            if (!PlayerPrefs.HasKey(StringPlayerPrefs.TUTORIAL_SKIN_4))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_SKIN_4, 0);
            }
            int showTutorialLv = PlayerPrefs.GetInt(StringPlayerPrefs.TUTORIAL_SKIN_4, 0);
            if (showTutorialLv == 0)
            {
             
                await Task.Delay(1000);
                tutorialSkin04.gameObject.SetActive(true);
                PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_SKIN_4, 1);
                
            }
            
            
           
            
        }


        public void ShowLoadingUI()
        {
            LoadingUI.gameObject.SetActive(true);
        }

        public void HideLoadingUI()
        {
            StartCoroutine(HideLoadingUICoroutine());
          
        }

        private IEnumerator HideLoadingUICoroutine()
        {
            yield return new WaitForSecondsRealtime(1f);
            LoadingUI.gameObject.SetActive(false);
        }

        public async void MinusHealth()
        {
            await Utills.DelayUntil(() =>
            {
                return Resource.Instance != null;
            });
            Resource.Instance.MinusHealth();
        }
        
        
      
     
    }
}