using System;
using System.Collections.Generic;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Hole.Skin
{
    public class ManagerSkinGamePlay : MonoBehaviour
    {
        public List<GameObject> skins;


        public void OnEnable()
        {
            TurnOffAllSkins();
            TurnOnSkin();
        }

        private void TurnOnSkin()
        {
            int currentSkinIndex = PlayerPrefs.GetInt(StringPlayerPrefs.HOLESKININDEX, 0);
            skins[currentSkinIndex].gameObject.SetActive(true);
        }

        private void TurnOffAllSkins()
        {
            foreach (var skin in skins)
            {
                skin.SetActive(false);
            }
        }
    }
}