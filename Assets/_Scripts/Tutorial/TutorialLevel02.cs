using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Tutorial
{
    public class TutorialLevel02 : BaseTutorial
    {

        [SerializeField] public GameObject GuideUI;

        private int currentLevelHole;
        public void Start()
        {
            currentLevelHole = this.holeController.GetCurrentLevel();
            StartCoroutine(ShowUI());
        }

        private IEnumerator ShowUI()
        {
            GuideUI.transform.SetParent(holeController.transform.Find("Canvas"));
            GuideUI.SetActive(true);
            yield return new WaitUntil(
                () => currentLevelHole != this.holeController.GetCurrentLevel());
            GuideUI.transform.DOScale(Vector3.zero, 0.3f).OnComplete(
                () => Destroy(GuideUI)
            );
            
        }
    }
}