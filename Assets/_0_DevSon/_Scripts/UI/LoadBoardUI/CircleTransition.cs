using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.LoadBoardUI
{
    public class CircleTransition : MonoBehaviour
    {
        private float maxRadius;
        private float currentRadious = 0;

        private float timeTransition = 0.6f;

        public Image circle;

        public void Awake()
        {
            maxRadius = (int)Math.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
            
            
            currentRadious = 0;
            
        }

        [ContextMenu("Transition")]
        public void UseTransition()
        {
            circle.rectTransform.sizeDelta = new Vector2(0, 0);
            currentRadious = 0;
            DOTween.KillAll();
            DOTween.To(() => currentRadious, x => currentRadious = x, maxRadius, 1f)
                .SetUpdate(true) // Không bị ảnh hưởng bởi timescale
                .OnUpdate(OnTweenUpdate); // Gọi hàm này mỗi khi giá trị được cập nhật


        }
        [ContextMenu("ReserveTransition")]
        public void ReverserTransition()
        {
            currentRadious = maxRadius;
            DOTween.KillAll();
            circle.rectTransform.sizeDelta = new Vector2(maxRadius, maxRadius);
            
            DOTween.To(() => currentRadious, x => currentRadious = x, 0, 1f)
                    .SetUpdate(true)
                    .OnUpdate(OnTweenUpdate);
        }
        



        private void OnTweenUpdate()
        {
            circle.rectTransform.sizeDelta = new Vector2(currentRadious, currentRadious);
        }
    }
}