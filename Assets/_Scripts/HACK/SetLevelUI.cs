using System;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.HACK
{
    public class SetLevelUI : MonoBehaviour
    {
        public TMP_InputField inputField;
        public Button setButton;


        public void OnEnable()
        {
            if (!PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_LEVEL))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            }
            inputField.text = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1).ToString();
            setButton.onClick.AddListener(SetLevelPlayer);
        }

        private void SetLevelPlayer()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_LEVEL, int.Parse(inputField.text));
        }
    }
}