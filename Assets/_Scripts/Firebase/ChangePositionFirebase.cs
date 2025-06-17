using UnityEngine;

namespace _Scripts.Firebase
{
    public class ChangePositionFirebase : MonoBehaviour
    {
        public PositionFirebase typePosition;
        public bool isPopupPosition;


        public void OnEnable()
        {
           // if (ManagerFirebase.Instance == null) return;
            if (isPopupPosition)
            {
               // ManagerFirebase.Instance.positionPopup = typePosition;
            }
            else
            {
               // ManagerFirebase.Instance.positionPopup = PositionFirebase.none;
               // ManagerFirebase.Instance.positionFirebase = typePosition;
            }
            
        }

        public void OnDisable()
        {
           // if (ManagerFirebase.Instance == null) return;
            if (isPopupPosition)
            {
              //  ManagerFirebase.Instance.positionPopup = PositionFirebase.none;
            }
        }
    }
}