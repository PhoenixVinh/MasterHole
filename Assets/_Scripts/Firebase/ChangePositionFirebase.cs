using UnityEngine;

namespace _Scripts.Firebase
{
    public class ChangePositionFirebase : MonoBehaviour
    {
        public PositionFirebase typePosition;
        public bool isPopupPosition;


        public void OnEnable()
        {
            if (isPopupPosition)
            {
                ManagerFirebase.Instance.positionPopup = typePosition;
            }
            else
            {
                ManagerFirebase.Instance.positionFirebase = typePosition;
            }
            
        }

        public void OnDisable()
        {
            if (isPopupPosition)
            {
                ManagerFirebase.Instance.positionPopup = PositionFirebase.none;
            }
        }
    }
}