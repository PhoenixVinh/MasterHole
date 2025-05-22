using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ShowFPS : MonoBehaviour
    {
        public TMP_Text fpsText;

        private void Awake()
        {
            Application.targetFrameRate = 120;
            DontDestroyOnLoad(this);
        }


        private float deltaTime = 0.0f;

        void Update()
        {
            // Tính toán thời gian giữa các frame
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            // Tính FPS
            float fps = 1.0f / deltaTime;
            // Hiển thị FPS trên Text UI
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
        }
        
        
    }
}