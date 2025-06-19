using System.Collections;
using System.Collections.Generic;
using _Scripts.Sound;
using _Scripts.UI.HoleSkinUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.WinLossUI.SkinCollectionUI
{
    public class SkinProcess : MonoBehaviour
    {
        public SkinSO skin;
        public TMP_Text text;
        public float duration = 1f;
        
        public List<FillImage> items;

        public Button Continue;
        public SkinMains skinMains;
        private bool isSkinMainsActive = false;
        public Button equipNow;
        int currentLevel = 0;
        int targetSkin = 0;
        public void OnEnable()
        {
            currentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL);
            targetSkin = GetTarget(currentLevel);
            TurnOffAllSkin();


            items[targetSkin - 1].gameObject.SetActive(true);
            int targetLevel = skin.skins[targetSkin].levelUnlock;
            int baseLevel = skin.skins[targetSkin - 1].levelUnlock;

          
            
            StartCoroutine((UpdatePercentage(currentLevel, targetLevel, items[targetSkin - 1].image)));
            Continue.onClick.AddListener(ChangeNextGame);
            equipNow.onClick.AddListener(EquipSkin); 
        }

        
        
        public void EquipSkin()
        {
        
            PlayerPrefs.SetInt(StringPlayerPrefs.HOLESKININDEX, targetSkin);
            PlayerPrefs.Save();
            this.gameObject.SetActive(false);
            ManagerLevelGamePlay.Instance.LoadNextLevel();
            //this.EquipButton.gameObject.SetActive(false);
       
        }

        public void OnDisable()
        {
            Continue.onClick.RemoveAllListeners();
        }
        private void ChangeNextGame()
        {
            this.gameObject.SetActive(false);
            if (isSkinMainsActive)
            {
                skinMains.SetData(targetSkin);
            }
            else
            {
                ManagerLevelGamePlay.Instance.LoadNextLevel();
            }
            
           
        }


        public void TurnOffAllSkin()
        {
            foreach (var item in items)
            {
                item.gameObject.SetActive(false);
            }
        }


        public int GetTarget(int currentLevel)
        {
            int result = -1;
            for (int i = 0; i < skin.skins.Count; i++)
            {
                if (currentLevel <= skin.skins[i].levelUnlock+1)
                {
                    result = i;
                    break;
                }
            }
            Debug.Log(result);
            return result;
        }
        
        
        private IEnumerator UpdatePercentage(int currentLevel, int target, Image image)
        {
            float elapsedTime = 0f;
            float startPercentage = (float)(currentLevel - 2) / target * 100f;
            float endPercentage = (float)(currentLevel - 1) / target * 100f; // Tính phần trăm
            if (endPercentage > 99f)
            {
                isSkinMainsActive = true;
                equipNow.gameObject.SetActive(true);
            }
            else
            {
                isSkinMainsActive = false;
                equipNow.gameObject.SetActive(false);
                
            }
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float percentage = Mathf.Lerp(startPercentage, endPercentage, elapsedTime / duration);
                text.text = $"{Mathf.RoundToInt(percentage)}%"; // Cập nhật text với giá trị làm tròn
                image.fillAmount = percentage / 100;
                yield return null; // Chờ frame tiếp theo
            }

            if (endPercentage > 99f)
            {
                ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.CompleteHole);
            }

            // Đảm bảo giá trị cuối cùng chính xác
            text.text = $"{Mathf.RoundToInt(endPercentage)}%";
        }
        
    }
}