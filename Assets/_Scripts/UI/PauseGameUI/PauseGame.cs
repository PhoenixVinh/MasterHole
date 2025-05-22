using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PauseGameUI
{
    public class PauseGame : MonoBehaviour
    {
        
        
        
        
        public virtual void OnEnable()
        {
            
            
            Time.timeScale = 0;
        }


        public virtual void OnDisable()
        {
          
         
            Time.timeScale = 1;
        }
    }
}