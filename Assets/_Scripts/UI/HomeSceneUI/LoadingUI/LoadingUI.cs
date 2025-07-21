using System;
using System.Collections;
using _Scripts.Ads;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HomeSceneUI.LoadingUI
{
    public class LoadingUI: MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        public bool IsLoading = false;


        public void OnEnable()
        {
            loadingBar.fillAmount = 0;
            IsLoading = true;

          
            StartCoroutine(LoadingBarCoroutine());
           
            
        }


        private IEnumerator LoadingBarCoroutine()
        {


            while (loadingBar.fillAmount < 1)
            {

                loadingBar.fillAmount += Time.deltaTime;



                yield return null;
            }
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (currentLevel == 1)
            {
                yield return new WaitForSeconds(1f);
            }


            //int currentLevel = PlayerPrefs.GetInt(StringPla)
            IsLoading = false;


            yield return new WaitForSeconds(0.5f);
            this.gameObject.SetActive(false);

        }


        public void OnDisable()
        {
            StopAllCoroutines();
            
           
        }
    }
}