using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.AnimationUI
{
    public class ChangeStatusBtnUI : MonoBehaviour
    {
        public Sprite spriteStart;
        public Sprite spriteEnd;
        
        private Button btn;


        public void Start()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(ChangeStatus);
        }

        private void ChangeStatus()
        {
            btn.GetComponent<Image>().sprite = spriteEnd;
        }

        public void SetBegin()
        {
            btn.GetComponent<Image>().sprite = spriteStart;
        }
    }
}