using System;
using System.Collections.Generic;
using _Scripts.Event;
using DG.Tweening;
using TMPro;
using UnityEngine;


namespace _Scripts.UI.AnimationUI
{
    public class CoinRewardAnim : MonoBehaviour
    {
        [SerializeField] private GameObject pileOfCoins;
        [SerializeField] private GameObject targetPos;

        [SerializeField] private GameObject iconCoin;
        [SerializeField] private TMP_Text counter;
        [SerializeField] private List<Vector2> initialPos;
        [SerializeField] private List<Quaternion> initialRotation;


        public void OnEnable()
        {
            RewardCoinEvent.OnRewardCoin += CountCoins;
        }
        


        void Awake()
        {
            
             
            
            initialPos = new List<Vector2>();
            initialRotation = new List<Quaternion>();
            
            for (int i = 0; i < pileOfCoins.transform.childCount; i++)
            {
                initialPos.Add(pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition);
                initialRotation.Add(pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation);
            }
        }

        public void OnDisable()
        {
            RewardCoinEvent.OnRewardCoin -= CountCoins;
        }

        void Reset()
        {
            for (int i = 0; i < pileOfCoins.transform.childCount; i++)
            {
                pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = initialPos[i];
                pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation = initialRotation[i];
            }
        }

        
        public void CountCoins(int startCoint, int TargetCoin)
        {
            
          
            this.counter.text = startCoint.ToString();
            float adding = (float)(TargetCoin - startCoint) / pileOfCoins.transform.childCount;
            
            
            Reset();
            pileOfCoins.SetActive(true);
            
            var delay = 0f;
            
            for (int i = 0; i < pileOfCoins.transform.childCount; i++)
            {
                pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetUpdate(true).SetDelay(delay).SetEase(Ease.OutBack);

                pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(targetPos.GetComponent<RectTransform>().anchoredPosition, 0.8f)
                    .SetUpdate(true).SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete(
                        () =>
                        {
                            iconCoin.transform.DOScale(new Vector3(1f, 1f, 1f)*1.1f, 0.3f).SetUpdate(true).OnComplete(
                                () =>
                                {
                                    iconCoin.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetUpdate(true);
                                    this.counter.text = $"{Mathf.RoundToInt(startCoint + (i)*adding)}";
                                }
                                );
                        }
                    );
                 

                pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetUpdate(true).SetDelay(delay + 0.5f)
                    .SetEase(Ease.Flash);
                
                
                pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetUpdate(true).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

                delay += 0.1f;

                
            }

            
        }
        
        

        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}