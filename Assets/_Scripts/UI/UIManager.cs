using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public BoardGameUI boardGameUI;
        public CircleTransition_2 FadeLoading;

        public GameObject Shop;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void ShowShop()
        {
            this.Shop.SetActive(true);
        }

        public void ShowBoardGame(MissionSO missionData, float timeToComplete)
        {

            StartCoroutine(ShowBoardGameCourutine(missionData, timeToComplete));
           
        }

        private IEnumerator ShowBoardGameCourutine(MissionSO missionData, float timeToComplete)
        {
            FadeLoading.SetTransition();

            while(FadeLoading.gameObject.activeInHierarchy){
                yield return null;
            }

            boardGameUI.SetData(missionData, timeToComplete);
        }

        public void HideUIBoardGame()
        {
            StartCoroutine(HideBoardGameCorutine());
        }

        private IEnumerator HideBoardGameCorutine()
        {
            yield return new WaitForSeconds(4f);
            this.boardGameUI.ShowAnimDisable();
            yield return new WaitForSeconds(1f);
            this.boardGameUI.gameObject.SetActive(false);
            this.FadeLoading.ReverseTransition();
        }
    }
}