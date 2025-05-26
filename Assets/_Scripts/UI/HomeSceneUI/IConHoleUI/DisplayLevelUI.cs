using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.IConHoleUI
{
    public class DisplayLevelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;

        private void Start()
        {

            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            _levelText.text = $"{currentLevel:D2}";
        }
        
    }
}