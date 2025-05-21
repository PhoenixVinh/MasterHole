using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.IConHoleUI
{
    public class IconHoleUI : MonoBehaviour
    {
        public List<GameObject> listIconSkins;

        private void OnEnable()
        {
            TurnOffAllSkin();

            int selectedIndexSkin = PlayerPrefs.GetInt(StringPlayerPrefs.HOLESKININDEX, 0);
            listIconSkins[selectedIndexSkin].SetActive(true);
        }


        public void TurnOffAllSkin()
        {
            foreach (var skin in listIconSkins)
            {
                skin.SetActive(false);
            }
        }
    }
}