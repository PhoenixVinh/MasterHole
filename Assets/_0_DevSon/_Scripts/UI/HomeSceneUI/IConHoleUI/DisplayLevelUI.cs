using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HomeSceneUI.IConHoleUI
{
    public class DisplayLevelUI : MonoBehaviour
    {
        public Transform parrent;

        public GameObject prefabUI;
        
        
        public List<Texture> textures;
        private void Start()
        {

            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            Convert(currentLevel);
           
        }


        public void Convert(int currentLevel)
        {
            
            string levelString = currentLevel.ToString();
            char[] digits = levelString.ToCharArray();
            
            int[] intDigits = digits.Select(c => c - '0').ToArray();

            foreach (var i in intDigits)
            {
                GameObject NumberUI = Instantiate(prefabUI, parrent);
                NumberUI.GetComponent<RawImage>().texture = textures[i];
            }
        }
        
    }
}