using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI.AnimationUI
{
    public class MoveDownAnimation : MonoBehaviour
    {
        [Header("Animation Settings")]
        public float startPosY = 100f;
        private float originPosY = 0f;
        public float timeMove = 0.6f;
        private RectTransform rectTransform;
        [Header("Adding Offset")]
        public float offsetY = 50f;
        public float timeAdd = 0.6f;

        public bool isAnimOutBack = true;
        public void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originPosY = rectTransform.anchoredPosition.y;
        }
        

        public void OnEnable()
        {
            MoveAnim();
        }
        
        private void MoveAnim()
        {
            if (isAnimOutBack)
            {
                rectTransform.anchoredPosition = rectTransform.anchoredPosition + new Vector2(0f, startPosY);
                rectTransform.DOAnchorPosY(originPosY + offsetY, timeMove)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        rectTransform.DOAnchorPosY(originPosY, timeAdd)
                            .SetEase(Ease.InBack);
                    });
            }
               
            else
            {
                rectTransform.anchoredPosition = rectTransform.anchoredPosition + new Vector2(0f, startPosY);
                rectTransform.DOAnchorPosY(originPosY, timeMove);
            }
        }
    }
}