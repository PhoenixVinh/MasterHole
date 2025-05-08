

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace _Scripts.UI.AnimationUI
{
    public class PressAnimBtn : MonoBehaviour
    {
        private Button button;
        public float sizeScale = 0.8f;
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(AnimationPressBtn);
        }

        private void AnimationPressBtn()
        {
            DOTween.Sequence()
                .SetUpdate(true)
                .Append(transform.DOScale(Vector3.one * sizeScale, 0.2f))
                .Append(transform.DOScale(Vector3.one, 0.1f));
        }

        public void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}