using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI
{
    public class ItemAnim : MonoBehaviour
    {
        public Image image;
        public TMP_Text amountText;

        public void SetData(Sprite sprite, string text, Vector3 postionStart, Vector3 positionEnd)
        {
            image.sprite = sprite; 
            amountText.text = text;
            
            // Set start position and scale
            transform.position = postionStart;
            transform.localScale = Vector3.zero;

            // Animate scale
            transform.DOScale(Vector3.one, 0.4f).SetUpdate(true);

            // Animate move
            transform.DOMove(positionEnd, 0.5f).SetUpdate(true);
            
        }
        
        
    }
}