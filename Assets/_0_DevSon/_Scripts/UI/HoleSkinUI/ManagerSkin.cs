using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.UI.HoleSkinUI
{
    public class ManagerSkin : MonoBehaviour
    {
        public List<ItemSkin> listItems;


        public void OnEnable()
        {
            HoleEvent.OnSkinSelected += TurnOffAll;
        }

        public void OnDisable()
        {
            HoleEvent.OnSkinSelected -= TurnOffAll;
        }

        private void TurnOffAll()
        {
            foreach (var item in listItems)
            {
                item.SetUIEquip();
            }
        }
    }
}