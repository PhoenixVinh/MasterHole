using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.SpecialSkillUI
{
    public class DirectionItem : MonoBehaviour
    {
        public Image content;

        public void SetContent(Sprite sprite)
        {
            this.content.sprite = sprite;
        }
    }
}