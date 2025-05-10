using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Sound
{
    public class ManagerSound : MonoBehaviour
    {
        public static ManagerSound Instance;
        
        [Range(0f,1f)]
        public float volumeBg = 1.0f;
        [Range(0f,1f)]
        public float volumeSfx = 1.0f;
        
        

        [SerializeField] private GameObject BGMusic;
        [SerializeField] private GameObject SFXMusic;
        
        
        [SerializeField]private Dictionary<EnumBackgroundSound, AudioSource> DictBackgroundMusic = new Dictionary<EnumBackgroundSound, AudioSource>(); 
        [SerializeField]private Dictionary<EnumEffectSound, AudioSource> DictEffectMusic = new Dictionary<EnumEffectSound, AudioSource>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

            GetData();
        }

        private void GetData()
        {
            for (int i = 0; i < BGMusic.transform.childCount; i++)
            {
                DictBackgroundMusic.Add((EnumBackgroundSound)i, BGMusic.transform.GetChild(i).GetComponent<AudioSource>());
            }

            for (int i = 0; i < SFXMusic.transform.childCount; i++)
            {
                DictEffectMusic.Add((EnumEffectSound)i, SFXMusic.transform.GetChild(i).GetComponent<AudioSource>());
                
            }
            
        }

        public void Start()
        {
            DictBackgroundMusic[EnumBackgroundSound.HomeMusic].volume = volumeBg;
            DictBackgroundMusic[EnumBackgroundSound.HomeMusic].Play();
        }

        public void ChangeBackgroundMusic(EnumBackgroundSound backgroundSound)
        {
            foreach (var audioSource in DictBackgroundMusic)
            {
                audioSource.Value.Stop();
            }
            DictBackgroundMusic[backgroundSound].volume = volumeBg;
            DictBackgroundMusic[backgroundSound].Play();
        }

        public void PlayEffectSound(EnumEffectSound effectSound)
        {
            //DictEffectMusic[effectSound].volume = volumeSfx;
            var musicSouce = DictEffectMusic[effectSound];
            musicSouce.volume = volumeSfx;
            musicSouce.Play();

            if (effectSound == EnumEffectSound.Magnet)
            {
                //Debug.Log("Magnet");
                musicSouce.loop = true;
                musicSouce.Play();
            }
        }

        public void StopEffectSound(EnumEffectSound effectSound)
        {
            var musicSouce = DictEffectMusic[effectSound];
            musicSouce.Stop();
        }
    }
}