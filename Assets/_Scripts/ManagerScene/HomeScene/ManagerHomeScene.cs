using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.UI;
using _Scripts.UI.AnimationUI;
using _Scripts.UI.HomeSceneUI.ResourcesUI;
using _Scripts.UI.PopupUI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.ManagerScene.HomeScene
{
    public class ManagerHomeScene: MonoBehaviour
    {
        public static ManagerHomeScene Instance;

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
            //ShowLoseGame.SetActive(false);
        }

        private async void Start()
        {
            LoadTutorial();
        }

        public async void LoadTutorial()
        {
           
            int CurrentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (CurrentLevel == 3)
            {
                int showTutorialLv3 = PlayerPrefs.GetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3);
                if (showTutorialLv3 == 0)
                {
                    // Show Pop Up Free Item  
                    await Task.Delay(1000);
                    Debug.Log(ManagerPopup.Instance == null);
                    ManagerPopup.Instance.ShowPopupFreeItem(0);
                    PlayerPrefs.SetInt(StringPlayerPrefs.TUTORIAL_LEVEL_3, 1);
                }
            }
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
            yield return new WaitForSeconds(1f);
            LoadingUI.gameObject.SetActive(false);
        }
    }
}