using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.Scroll
{
    public class ScrollViewUI: MonoBehaviour
    {
        public void ValueChange(Vector2 value)
        {
            Debug.Log("ValueChange : " + value.ToString());
        }
    }
}