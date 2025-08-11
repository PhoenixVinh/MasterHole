using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.SpecialSkillUI
{
    public class SpecialSkill : MonoBehaviour
    {
        public List<Button> specialBtns;

        private void Start()
        {
            
            // AddEventForEach Button 

            for (int i = 0; i < specialBtns.Count; i++)
            {
                int index = i;
                specialBtns[index].onClick.AddListener(() => { HoleController.Instance.ProcessSkill(index); });
            }
            
            
            
        }
    }
}