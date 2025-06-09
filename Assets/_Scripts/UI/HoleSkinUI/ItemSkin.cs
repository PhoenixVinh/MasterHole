
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.HoleSkinUI
{
    public class ItemSkin : MonoBehaviour
    {

        [Header("Sprite Button")]        
        public Sprite BGNormal;
        public Sprite BGSelected;
        public Sprite BGLock;

        
        
        [Header("Variable Item")]
        public string itemName;
        public TMP_Text txtItemName;
        

        public int price = 0;
        public int levelUnlock = 1;
        public int indexSkinHole = 0;
        private Button button;

        public TMP_Text txtStatus;
        
        
        [Header("Icon")]
        public Image icon;
        public GameObject checkicon;
        public GameObject lockicon;


        private bool isLock = false;
        
        private void Awake()
        {
            button = GetComponent<Button>();
            checkicon.SetActive(false);
            lockicon.SetActive(false);
            txtItemName.text = itemName;
            
        }

        public void Start()
        {
            SetUI();



            //SetLockUI();
        }

        private void SetLockUI()
        {
            button.image.sprite = BGLock;
            lockicon.SetActive(true);
            checkicon.SetActive(false);
            this.txtStatus.text = $"Unlock level {levelUnlock}";
            icon.color = new Color(0.2f, 0.2f, 0.2f, 1);
            isLock = true;
            SetSize(BGLock);
        }

        private void SetUI()
        {
            int currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
            if (currentLevel < levelUnlock+1)
            {
                SetLockUI();
            }
            else
            {
                int indexSkin = PlayerPrefs.GetInt(StringPlayerPrefs.HOLESKININDEX, 0);
                button.onClick.AddListener(SetEquippedSkin);
                
                
                if (indexSkin == this.indexSkinHole)
                {
                    SetUIEquipped();
                }
                else
                {
                    SetUIEquip();
                }
            }
        }

        public void SetEquippedSkin()
        {
            HoleEvent.OnSkinSelected?.Invoke();
            SetUIEquipped();
            PlayerPrefs.SetInt(StringPlayerPrefs.HOLESKININDEX, this.indexSkinHole);
        }

        public void SetUIEquipped()
        {
            button.image.sprite = BGSelected;
            lockicon.SetActive(false);
            checkicon.SetActive(true);
            icon.color = new Color(1, 1, 1, 1);
            this.txtStatus.text = "Equipped";
            
            SetSize(BGSelected);
        }

        public void SetUIEquip()
        {
            if (isLock) return;
            button.image.sprite = BGNormal;
            lockicon.SetActive(false);
            checkicon.SetActive(false);
            icon.color = new Color(1, 1, 1, 1);
            this.txtStatus.text = "Equip";
            SetSize(BGNormal);
        }


        public void SetSize(Sprite sprite)
        {
            Vector2 spriteSize = new Vector2(sprite.rect.width, sprite.rect.height);

            // Set the button's RectTransform size to match the sprite
        
            this.GetComponent<RectTransform>() .sizeDelta = spriteSize;
        }


        
    }
}