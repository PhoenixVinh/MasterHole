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
        [SerializeField] private List<RectTransform> pileOfCoins;
        [SerializeField] private GameObject targetPos;

        [SerializeField] private GameObject iconCoin;
        [SerializeField] private TMP_Text counter;
        [SerializeField] private List<Vector2> initialPos;
        [SerializeField] private List<Quaternion> initialRotation;


        public void OnEnable()
        {
            
            //RewardCoinEvent.OnRewardCoin += CountCoins;
        }
        
        public void OnDisable()
        {
            //RewardCoinEvent.OnRewardCoin -= CountCoins;
            DOTween.KillAll();
        }

        void Awake()
        {
            
             
            
            initialPos = new List<Vector2>();
            initialRotation = new List<Quaternion>();
            
            for (int i = 0; i < pileOfCoins.Count; i++)
            {
                initialPos.Add(pileOfCoins[i].anchoredPosition);
                initialRotation.Add(pileOfCoins[i].rotation);
            }
        }



        void Reset()
        {
            for (int i = 0; i < pileOfCoins.Count; i++)
            {
                pileOfCoins[i].anchoredPosition = initialPos[i];
                pileOfCoins[i].rotation = initialRotation[i];
                pileOfCoins[i].transform.localScale = Vector3.zero;
            }
            
            DOTween.KillAll();
        }

        
        public void CountCoins(int startCoint, int TargetCoin)
        {
            
            DOTween.SetTweensCapacity(500, 125);

            Reset();
            this.counter.text = startCoint.ToString();
            float adding = (float)(TargetCoin - startCoint) / pileOfCoins.Count;

            var delay = 0f;

            for (int i = 0; i < pileOfCoins.Count; i++)
            {
                pileOfCoins[i].gameObject.SetActive(true);
                var sequence = DOTween.Sequence();
               
                sequence.SetUpdate(true);

                int index = i; // Lưu giá trị i

                // Tính delay tăng dần để các coin chạy lần lượt
                float coinDelay = delay;

                sequence.Append(pileOfCoins[index].DOScale(1f, 0.3f).SetDelay(coinDelay).SetEase(Ease.OutBack));

                sequence.Append(pileOfCoins[index].DOAnchorPos(targetPos.GetComponent<RectTransform>().anchoredPosition, 0.8f)
                    .SetDelay(0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        iconCoin.transform.DOScale(new Vector3(1f, 1f, 1f) * 1.1f, 0.3f).SetUpdate(true).OnComplete(() =>
                        {
                            iconCoin.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetUpdate(true);
                            this.counter.text = $"{Mathf.RoundToInt(startCoint + (index + 1) * adding)}";
                        });
                    }));

                sequence.Join(pileOfCoins[index].DORotate(Vector3.zero, 0.5f).SetDelay(0.5f).SetEase(Ease.Flash));

                sequence.Append(pileOfCoins[index].DOScale(0f, 0.3f).SetDelay(1.5f).SetEase(Ease.OutBack));

                sequence.OnComplete(() =>
                {
                    pileOfCoins[index].gameObject.SetActive(false);
                  

                    if (index == pileOfCoins.Count - 1)
                    {
                        Reset();
                        this.counter.text = TargetCoin.ToString(); // Đảm bảo counter hiển thị giá trị cuối cùng
                    }
                });

                // Tăng delay cho coin tiếp theo
                delay += 0.1f;
            }
            

            
        }
        
        
        [ContextMenu("Show Anim")]
        public void ShowAnim()
        {
            DOTween.SetTweensCapacity(500, 125);

            //Reset();
          
            var delay = 0f;

            for (int i = 0; i < pileOfCoins.Count; i++)
            {
                pileOfCoins[i].gameObject.SetActive(true);
                var sequence = DOTween.Sequence();
               
                sequence.SetUpdate(true);

                int index = i; // Lưu giá trị i

                // Tính delay tăng dần để các coin chạy lần lượt
                float coinDelay = delay;

                sequence.Append(pileOfCoins[index].DOScale(1f, 0.4f).SetDelay(coinDelay).SetEase(Ease.OutBack));

                sequence.Append(pileOfCoins[index].DOAnchorPos(targetPos.GetComponent<RectTransform>().anchoredPosition, 0.8f)
                    .SetDelay(0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        iconCoin.transform.DOScale(new Vector3(1f, 1f, 1f) * 1.1f, 0.3f).SetUpdate(true).OnComplete(() =>
                        {
                            iconCoin.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetUpdate(true);
                          
                        });
                    }));

                sequence.Join(pileOfCoins[index].DORotate(Vector3.zero, 0.5f).SetDelay(0.5f).SetEase(Ease.Flash));

                sequence.Append(pileOfCoins[index].DOScale(0f, 0.3f).SetDelay(1.5f).SetEase(Ease.OutBack));

                sequence.OnComplete(() =>
                {
                    pileOfCoins[index].gameObject.SetActive(false);
                  

                    if (index == pileOfCoins.Count - 1)
                    {
                        Reset();
                     
                    }
                });

                // Tăng delay cho coin tiếp theo
                delay += 0.1f;
            }
            

            
        }
        
       
        
        
        

        private void OnDestroy()
        {
            
            DOTween.KillAll();
            Reset();
        }
    }
}