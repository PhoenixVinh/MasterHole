using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Event;
using _Scripts.Map.MapSpawnItem;
using _Scripts.Sound;
using _Scripts.UI.MissionUI;
using _Scripts.UI.SpecialSkillUI;
using _Scripts.Vibration;
using DG.Tweening;
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

        public void ProcessSkill(SpecialSkill skill)
        {
            // Check is Skill is Action => Dont use 
            if(IsProcessSkill[(int) skill]) return;
            switch (skill)
            {
                case SpecialSkill.IncreaseRange:
                    StartCoroutine(IncreaseRangeCoroutine()) ;
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
            Dictionary<string, int> result = ManagerMission.Instance.GetSuggestItems(amountFinding);
            foreach (var item in result)
            {
                Debug.Log("Key : " + item.Key + " Value : " + item.Value);
            }

            List<GameObject> itemsSuggest = SpawnItemMap.Instance.GetMappingObject(result);

            foreach (var item in itemsSuggest)
            {
                if(item == null) continue;
                DirectionItemUI UI = item.AddComponent<DirectionItemUI>();
                UI.SetData(HoleController.Instance.transform, ManagerMission.Instance.GetSprite(item.name));
            }
            
            
            yield return new WaitForSeconds(this.timeSkill03);


            foreach (var item in itemsSuggest)
            {
                if(item == null) continue;
                DirectionItemUI UI = item.GetComponent<DirectionItemUI>();
                Destroy(UI);
            }
        }

        private IEnumerator FreezeTimeCoroutine()
        {
            ManagerSound.Instance.PlayEffectSound(EnumEffectSound.Ice);
            TimeEvent.OnFreezeTime?.Invoke(timeSkill04);
            IsProcessSkill[3] = true;
            yield return new WaitForSeconds(timeSkill04);
            IsProcessSkill[3] = false;
            ManagerSound.Instance.StopEffectSound(EnumEffectSound.Ice);
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
                yield return null;
            }
            // yield return new WaitForSeconds(timeSkill01);
            
         
            // Decease Scale 
            float scaleDecrease = HoleController.Instance.GetCurrentRadius();
            //
            
            transform.DOScale(new Vector3(scaleDecrease, y, scaleDecrease), 1f);
            //
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


        public bool UsingSkill(int index)
        {
            return IsProcessSkill[index];
        }
    }
}