
using System;
using UnityEngine;

namespace _Scripts.UI.AnimationUI
{
    public class RotateAnimation : MonoBehaviour
    {
        public float rotateSpeed = 5;
        
        private RectTransform rectTransform;


        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.RotateAround(transform.position, new Vector3(0,0,-1), rotateSpeed);
            
        }
    }
}