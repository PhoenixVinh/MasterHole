using System;
using System.Collections;
using _Scripts.Event;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Camera
{
    public class CameraFOV : MonoBehaviour
    {
       public float startLevelDistance = 15f;
        
        
        public CinemachineFramingTransposer _virtualCamera;

        public float targetDistance = 10;
        public float scaleByHole = 5;
        public float baseDistance = 5;
        //public Slider silider;
        
        //
        // public float baseFOV = 10f;
        // public float scaleByHole = 1f;
        // public float _targetFOV;

        private bool isStartLevel = false;
        private bool isLevelUp = false;
        private bool isMove = false;
        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            targetDistance = _virtualCamera.m_CameraDistance;
        }
        
        


        private void OnEnable()
        {
            CameraFOVEvent.OnStarLevelEvent += StartLevelFOV;
            CameraFOVEvent.OnLevelUpEvent += LevelUpEventFOV;
            // HoleEvent.OnLevelUp += UpdateFOV;
            // HoleEvent.OnStartIncreaseSpecialSkill += UpdateFOVBySkill;
        }

       
        private void OnDisable()
        {
            CameraFOVEvent.OnStarLevelEvent -= StartLevelFOV;
            CameraFOVEvent.OnLevelUpEvent -= LevelUpEventFOV;
        }

        private void StartLevelFOV()
        {
            StartCoroutine(StartLevelFOVCorutine());
            
        }
        private void LevelUpEventFOV(float delaytime)
        {
            StartCoroutine(LevelUpCoroutine(delaytime));
        }

        private IEnumerator LevelUpCoroutine(float delaytime)
        {
            isLevelUp = true;
            yield return new WaitForSeconds(delaytime);
            isLevelUp = false;
        }


        private IEnumerator StartLevelFOVCorutine()
        {
            _virtualCamera.m_CameraDistance = startLevelDistance;
            isStartLevel = true;
            yield return new WaitForSeconds(0.75f);
            float addingFOV = HoleController.Instance.transform.localScale.x * scaleByHole;
            // targetDistance = baseDistance + addingFOV;
            // _virtualCamera.m_CameraDistance = targetDistance;
            isStartLevel = false;
        }

        private void UpdateFOVBySkill(float timeskill)
        {
            StartCoroutine(UpdateFOVBySkillCoroutine(timeskill));
        }

        private IEnumerator UpdateFOVBySkillCoroutine(float timeskill)
        {
            targetDistance *= 1.2f;
            yield return new WaitForSeconds(timeskill);
            targetDistance /= 1.2f;
        }



        private void LateUpdate()
        {
            if (isStartLevel) return;
            if(isLevelUp) return;
            //_virtualCamera.m_CameraDistance = silider.value * 200;

            float addingFOV = HoleController.Instance.transform.localScale.x * scaleByHole;
            if (HoleController.Instance.HoleMovement.GetDirectionMovement() == Vector2.zero && !isMove)
            {
               
                targetDistance = baseDistance + addingFOV + 25;
            }
            else
            {
                isMove = true;
                targetDistance = baseDistance + addingFOV;
            }
            
            //_virtualCamera.m_CameraDistance = targetDistance;
            _virtualCamera.m_CameraDistance = Mathf.Lerp(_virtualCamera.m_CameraDistance, targetDistance, Time.deltaTime * 10f);

        }

        
        
        
        
    }
}