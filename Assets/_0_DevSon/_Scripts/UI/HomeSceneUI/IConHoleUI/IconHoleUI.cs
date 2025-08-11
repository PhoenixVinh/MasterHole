using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.IConHoleUI
{
    public class IconHoleUI : MonoBehaviour
    {
        public List<GameObject> Ups;
        public List<GameObject> Downs;
        private void OnEnable()
        {
            TurnOffAllSkin();

            int selectedIndexSkin = PlayerPrefs.GetInt(StringPlayerPrefs.HOLESKININDEX, 0);
            Ups[selectedIndexSkin].SetActive(true);
            Downs[selectedIndexSkin].SetActive(true);
        }


        public void TurnOffAllSkin()
        {
            for (int i = 0; i < Ups.Count; i++)
            {
                Ups[i].SetActive(false);
                Downs[i].SetActive(false);
            }
        }
    }
}