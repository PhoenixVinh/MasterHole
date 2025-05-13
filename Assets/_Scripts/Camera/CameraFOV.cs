using System;
using System.Collections;
using _Scripts.Event;
using Cinemachine;
using UnityEngine;

namespace _Scripts.Camera
{
    public class CameraFOV : MonoBehaviour
    {
        [Header("FOV Start Level")] public float startLevelFOV = 15f;
        
        
        public CinemachineVirtualCamera _virtualCamera;

        public float baseFOV = 10f;
        public float scaleByHole = 1f;
        public float _targetFOV;

        private bool isStartLevel = false;
        private bool isLevelUp = false;
        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _targetFOV = _virtualCamera.m_Lens.FieldOfView;
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
            _virtualCamera.m_Lens.FieldOfView = startLevelFOV;
            isStartLevel = true;
            yield return new WaitForSeconds(0.75f);
            
            isStartLevel = false;
        }

        private void UpdateFOVBySkill(float timeskill)
        {
            StartCoroutine(UpdateFOVBySkillCoroutine(timeskill));
        }

        private IEnumerator UpdateFOVBySkillCoroutine(float timeskill)
        {
            _targetFOV *= 1.2f;
            yield return new WaitForSeconds(timeskill);
            _targetFOV /= 1.2f;
        }



        private void FixedUpdate()
        {
            if (isStartLevel) return;
            if(isLevelUp) return;
            float addingFOV = HoleController.Instance.transform.localScale.x * scaleByHole;
            
            
            _virtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView, baseFOV + addingFOV, Time.deltaTime*5f);
        }

        private void UpdateFOV() 
        {
            // Change FOV of Camera;
            _targetFOV*= 1.1f;
           
        }
        
        
        
    }
}