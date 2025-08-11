using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HoleUI
{
    public class LevelUpBar : MonoBehaviour
    {
        public Image silder;
        public RectTransform movementExp;

        public TMP_Text text;
        private IPrecent holeLevel;
        private void Start()
        {
           
            holeLevel = HoleController.Instance.HoleLevel;
        }

        private void FixedUpdate()
        {
            if (holeLevel.Precent() != 0)
            {
                silder.fillAmount = Mathf.Lerp(silder.fillAmount, holeLevel.Precent(), Time.fixedDeltaTime * 2);
                float valueRotate = silder.fillAmount * 360;
                valueRotate = valueRotate >= -416 ? valueRotate : -416; 
                movementExp.rotation = Quaternion.Euler(90, 180 , -64-valueRotate);
            }
            else
            {
                movementExp.rotation = Quaternion.Euler(90, 180 , -64);
                silder.fillAmount = 0f;
            }

            text.text = $"SIZE {HoleController.Instance.GetCurrentLevel() + 1}";


        }
    }
}