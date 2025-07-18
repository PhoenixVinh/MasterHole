using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.AnimationUI
{
    public class ChangeStatusBtnUI : MonoBehaviour
    {
        public Sprite spriteStart;
        public Sprite spriteEnd;
        
        private Button btn;

        public Image imgRef;

        public bool isLeft;

        public void Start()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(ChangeStatus);
        }

        private void ChangeStatus()
        {
            imgRef.sprite = spriteEnd;
            // Roate 
            if (isLeft)
            {
                imgRef.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                imgRef.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
           
        }

        public void SetBegin()
        {
            imgRef.sprite = spriteStart;
            if (isLeft)
                imgRef.transform.rotation = Quaternion.Euler(0, 180, 0);
            else
            {
                imgRef.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}