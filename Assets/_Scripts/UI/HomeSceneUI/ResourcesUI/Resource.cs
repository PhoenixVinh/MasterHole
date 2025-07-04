using _Scripts.Event;
using _Scripts.UI.PopupUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HomeSceneUI.ResourcesUI
{
    public class Resource : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text coin;
        [SerializeField] private TMP_Text heart;
        //public Energy Energy;
        private int amountCoin = 0;
        
        public static Resource Instance;



        public Button energyBtn;
        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
               
            } else {
                Destroy (gameObject);
            }

            if (!PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_COIN))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, 900);
            }
            amountCoin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 900); 
        }
        
        public void OnEnable()
        {
            energyBtn.onClick.AddListener(
                () => ManagerPopup.Instance?.ShowPopupBuyEnergy()
            );
            UpdateText();
            ResourceEvent.OnUpdateResource += UpdateText;
        }
        private void UpdateText()
        {
            amountCoin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 9000); 
            coin.text = amountCoin.ToString();
            
        }

        public void MinusHealth()
        {
            Energy.Instance.UseEnergy();
        }

        public void PlusHealth()
        {
            Energy.Instance.AddEnergy();
        }

        public void AddMaxHealth()
        {
            Energy.Instance.AddMaxEnergy();
            
        }

        public void AddMaxCoin()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, 900);
            amountCoin = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_COIN, 900); 
            coin.text = amountCoin.ToString();
        }
        
        public bool useCoin(int amount)
        {
            if (this.amountCoin >= amount)
            {
                this.amountCoin -= amount;
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_COIN, amountCoin);
                return true;
            }

            return false;
        }
    }
}