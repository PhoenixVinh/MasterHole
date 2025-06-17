using System;
using _Scripts.Firebase;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.ButtonUI
{
    public class FirebaseEventBtn : MonoBehaviour
    {
        public  PositionFirebase pos;
        
        private Button button;

        public void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(LogFirebaseEvent);
            //button.
        }

        private void LogFirebaseEvent()
        {
           // ManagerFirebase.Instance?.LogEventNormal(pos);
        }
    }
}