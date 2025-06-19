using UnityEngine;

namespace _Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameObject Shop;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void ShowShop()
        {
            this.Shop.SetActive(true);
        }
    }
}