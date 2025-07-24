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

        private float timeTransition = 0.2f;

        public Image circle;

        public void Awake()
        {
            maxRadius = (float)Math.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
            
            
            currentRadious = 0;
            
        }

        [ContextMenu("Transition")]
        public void UseTransition()
        {
            DOTween.To(() => currentRadious, x => currentRadious = x, maxRadius, timeTransition)
                .SetUpdate(true) // Không bị ảnh hưởng bởi timescale
                .OnUpdate(OnTweenUpdate); // Gọi hàm này mỗi khi giá trị được cập nhật


        }

        private void OnTweenUpdate()
        {
            circle.rectTransform.sizeDelta = new Vector2(currentRadious, currentRadious);
        }
    }
}