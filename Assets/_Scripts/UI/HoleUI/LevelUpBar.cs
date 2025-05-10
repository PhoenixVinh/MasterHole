using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HoleUI
{
    public class LevelUpBar : MonoBehaviour
    {
        public Image silder;

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
                silder.fillAmount = Mathf.Lerp(silder.fillAmount, holeLevel.Precent(), Time.fixedDeltaTime*2);
            }
            else
            {
                silder.fillAmount = 0f;
            }

            text.text = $"SIZE {HoleController.Instance.GetCurrentLevel() + 1}";


        }
    }
}