using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.PauseGameUI
{
    public class PauseGame : MonoBehaviour
    {
        public Button pauseButton;
        
        
        
        
        public virtual void OnEnable()
        {
            if (pauseButton != null)
            {
                pauseButton.gameObject.SetActive(false);
            }
            
            Time.timeScale = 0;
        }


        public virtual void OnDisable()
        {
            if (pauseButton != null)
            {
                pauseButton.gameObject.SetActive(true);
            }
         
            Time.timeScale = 1;
        }
    }
}