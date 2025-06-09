using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.WinLossUI.SkinCollectionUI
{
    public class FillImage : MonoBehaviour
    {
        
        public Image image;

        public void SetData(float present)
        {
            image.fillAmount = present;
        }
    }
}