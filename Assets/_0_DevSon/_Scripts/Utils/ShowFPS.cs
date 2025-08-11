using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ShowFPS : MonoBehaviour
    {
        public TMP_Text fpsText;
        public static ShowFPS Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
               
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
            
            
        }

        private void Start()
        {
            Application.targetFrameRate = 120;
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