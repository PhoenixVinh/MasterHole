using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HoleUI
{
    public class LevelUpBar : MonoBehaviour
    {
        private Slider silder;

        private IPrecent holeLevel;
        private void Start()
        {
            silder = GetComponent<Slider>();
            holeLevel = HoleController.Instance.HoleLevel;
        }

        private void FixedUpdate()
        {
            if (holeLevel.Precent() != 0)
            {
                silder.value = Mathf.Lerp(silder.value, holeLevel.Precent(), Time.fixedDeltaTime*2);
            }
            else
            {
                silder.value = 0f;
            }

            
        }
    }
}