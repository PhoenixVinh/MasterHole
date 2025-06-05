using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HomeSceneUI.LoadingUI
{
    public class LoadingUI: MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        


        public void OnEnable()
        {
            loadingBar.fillAmount = 0;
            StartCoroutine(LoadingBarCoroutine());
           

        }

        private IEnumerator LoadingBarCoroutine()
        {
            
            while (loadingBar.fillAmount < 1)
            {
                loadingBar.fillAmount += Time.deltaTime;
                
                yield return null;
            }
            
        }


        public void OnDisable()
        {
            StopAllCoroutines();
            
           
        }
    }
}