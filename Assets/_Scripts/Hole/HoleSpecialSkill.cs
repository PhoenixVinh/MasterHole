using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Event;
using _Scripts.Map.MapSpawnItem;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using _Scripts.UI;
using _Scripts.UI.MissionUI;
using _Scripts.UI.SpecialSkillUI;
using _Scripts.Vibration;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Hole
{
    public class HoleSpecialSkill : MonoBehaviour
    {
        private bool[] IsProcessSkill = new bool[4];
        [Header("Variable Skill Increase Size")]
        public float timeSkill01 = 15f;
        
        
        [Header("Variable Skill Use Magnet")]
        public float timeSkill02 = 15f;
        public ParticleSystem EffectSkill02; 
        public GameObject TriggerMagnet;
        
        [Header("Variable Skill Use Finding")]
        public float timeSkill03 = 12f;
        public int amountFinding = 7;


        private List<GameObject> _itemsSuggest = new List<GameObject>();
        
        

        [Header("Variable Skill Freeze Time")]
        public float timeSkill04 = 12f;

        private void Awake()
        {
            EffectSkill02.gameObject.SetActive(false);
        }

        private void Start()
        {
          
            TriggerMagnet.SetActive(false);
            for (int i = 0; i < IsProcessSkill.Length; i++)
            {
                IsProcessSkill[i] = false;
            }
        }


        private void OnEnable()
        {
            ItemEvent.OnItemMissionFinding += ChangeObjectMission;
        }
        private void OnDisable()
        {
            ItemEvent.OnItemMissionFinding -= ChangeObjectMission;
            StopEventSkill();
        }

    

        public void ProcessSkill(SpecialSkill skill)
        {
            // Check is Skill is Action => Dont use 
            if (IsProcessSkill[(int)skill]) return;
            switch (skill)
            {
                case SpecialSkill.IncreaseRange:
                    StartCoroutine(IncreaseRangeCoroutine());
                    break;
                case SpecialSkill.Magnet:
                    StartCoroutine(UseMagnetCoroutine());
                    break;
                case SpecialSkill.Direction:
                    StartCoroutine(UseFindingCoroutine());
                    break;
                case SpecialSkill.FreezeColdown:
                    StartCoroutine(FreezeTimeCoroutine());
                    break;
            }
        }

        private IEnumerator UseFindingCoroutine()
        {
            
            Debug.Log("Use Finding");
            IsProcessSkill[2] = true;
            
            _itemsSuggest = ManagerMission.Instance.GetSuggestItems(amountFinding);
            // foreach (var item in result)
            // {
            //     Debug.Log("Key : " + item.Key + " Value : " + item.Value);
            // }
            //
            // List<GameObject> itemsSuggest = SpawnItemMap.Instance.GetMappingObject(result);

            foreach (var item in _itemsSuggest)
            {
                if(item == null) continue;
                DirectionItemUI UI = item.AddComponent<DirectionItemUI>();
                UI.SetData(HoleController.Instance.transform, ManagerMission.Instance.GetSprite(item.name));
            }
            yield return new WaitForSecondsRealtime(this.timeSkill03);
            
            foreach (var item in _itemsSuggest)
            {
                if(item == null) continue;
                DirectionItemUI UI = item.GetComponent<DirectionItemUI>();
                DestroyImmediate(UI);
               
            }
           
            
            
            IsProcessSkill[2] = false;

           
        }

        private IEnumerator FreezeTimeCoroutine()
        {

            IsProcessSkill[3] = true;
            int currentUseBooster = PlayerPrefs.GetInt(StringPlayerPrefs.COUNT_USE_BOOSTER_ICE);
            currentUseBooster++;
            PlayerPrefs.SetInt(StringPlayerPrefs.COUNT_USE_BOOSTER_ICE, currentUseBooster);
            ManagerSound.Instance?.PlayEffectSound(EnumEffectSound.Ice);
            TimeEvent.OnFreezeTime?.Invoke(timeSkill04);
            
            yield return new WaitForSeconds(timeSkill04);
            IsProcessSkill[3] = false;
            ManagerSound.Instance?.StopEffectSound(EnumEffectSound.Ice);
        }

        private IEnumerator UseMagnetCoroutine()
        {
           
            IsProcessSkill[1] = true;
            float timeColdown = timeSkill02;
            
            TriggerMagnet.SetActive(true);
            EffectSkill02.gameObject.SetActive(true);
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.PlayEffectSound(EnumEffectSound.Magnet);
            }
            while (timeColdown > 0)
            {
                EffectSkill02.startSpeed = HoleController.Instance.GetCurrentScale() * 1.5f;
                timeColdown -= Time.deltaTime;
                yield return null;
            }
            TriggerMagnet.SetActive(false);
            EffectSkill02.gameObject.SetActive(false);
         
            IsProcessSkill[1] = false;
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.StopEffectSound(EnumEffectSound.Magnet);
            }
        }

        private IEnumerator IncreaseRangeCoroutine()
        {
            IsProcessSkill[0] = true;
            float scaleIncrease = HoleController.Instance.GetCurrentRadius() *1.5f;
            var sequence = DOTween.Sequence();
            
            float y = transform.localScale.y;
            //transform.localScale = new Vector3(scaleIncrease, y, scaleIncrease);
            transform.DOScale(new Vector3(scaleIncrease, y, scaleIncrease), 1f);
            
            
            //DOTween.Kill(sequence);
            //this.transform.localScale = new Vector3(scaleIncrease, transform.localScale.y, scaleIncrease);
            //HoleController.Instance.Upscale(timeSkill01*10);
            float _timeSkill01 = timeSkill01;
            while (_timeSkill01 > 0)
            {
                
                _timeSkill01 -= Time.deltaTime;
                
                if (scaleIncrease < HoleController.Instance.GetCurrentRadius() * 1.5f)
                {
                    scaleIncrease = HoleController.Instance.GetCurrentRadius() * 1.5f;
                    var sequence2= DOTween.Sequence();
                    sequence2.Append(transform.DOScale(new Vector3(scaleIncrease, transform.localScale.y, scaleIncrease), 0.5f));
                    
                }
                HoleEvent.OnUpdateFade?.Invoke(HoleController.Instance.GetCurrentScale());
                yield return null;
            }
            // yield return new WaitForSeconds(timeSkill01);
            
         
            // Decease Scale 
            float scaleDecrease = HoleController.Instance.GetCurrentRadius();
            //
            
            transform.DOScale(new Vector3(scaleDecrease, y, scaleDecrease), 1f);
            //
            HoleEvent.OnUpdateFade?.Invoke(HoleController.Instance.GetCurrentScale());
            IsProcessSkill[0] = false;
        }

        public void StopEventSkill()
        {
            StopAllCoroutines();
            DOTween.KillAll();
            if (IsProcessSkill[1])
            {
                // magnet Skill stop 
                TriggerMagnet.SetActive(false);
                EffectSkill02.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < IsProcessSkill.Length; i++)
            {
                IsProcessSkill[i] = false;
            }
            
            
         
            // use Scale 
            


        }




        private void ChangeObjectMission(GameObject item)
        {
            if (IsProcessSkill[2] == false) return;


            if (this._itemsSuggest.Contains(item))
            {
                //Debug.Log("Change Object Mission : " + item.name);
                this._itemsSuggest.Remove(item);
                DirectionItemUI UI = item.GetComponent<DirectionItemUI>();
                Destroy(UI);



                // Get An Other Suggest Item
                GameObject newItem = ManagerMission.Instance.GetAnOtherSuggestItems(this._itemsSuggest);
                if (newItem != null)
                {
                    DirectionItemUI newUI = newItem.AddComponent<DirectionItemUI>();
                    newUI.SetData(HoleController.Instance.transform, ManagerMission.Instance.GetSprite(newItem.name));
                    this._itemsSuggest.Add(newItem);
                }
                //  GetAnOtherSuggestItems(this._itemsSuggest);
                ////GameObject newItem = 

            }
        }


        public bool UsingSkill(int index)
        {
            return IsProcessSkill[index];
        }
    }
}