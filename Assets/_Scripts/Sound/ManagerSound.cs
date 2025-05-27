using System;
using System.Collections.Generic;
using _Scripts.UI;
using DG.Tweening;
using Unity.VisualScripting;
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


        public bool canBgMusic = true;
        public bool canSfxMusic = true;
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
            
            if(!PlayerPrefs.HasKey(StringPlayerPrefs.USE_BGMUSIC))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.USE_BGMUSIC, 1);
            }
            if(!PlayerPrefs.HasKey(StringPlayerPrefs.USE_SFXSOUND))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.USE_SFXSOUND, 1);
            }
            
            canBgMusic = PlayerPrefs.GetInt(StringPlayerPrefs.USE_BGMUSIC, 1) == 1;
            canSfxMusic = PlayerPrefs.GetInt(StringPlayerPrefs.USE_SFXSOUND, 1) == 1;         
            DictBackgroundMusic[EnumBackgroundSound.HomeMusic].volume = volumeBg;
            if (canBgMusic)
            {
                DictBackgroundMusic[EnumBackgroundSound.HomeMusic].Play();
            }
           
        }

        public void ChangeBackgroundMusic(EnumBackgroundSound backgroundSound)
        {
            if (!canBgMusic) return;
            foreach (var audioSource in DictBackgroundMusic)
            {
                audioSource.Value.Stop();
            }
            DictBackgroundMusic[backgroundSound].volume = volumeBg;
            DictBackgroundMusic[backgroundSound].Play();
        }

        public void PlayEffectSound(EnumEffectSound effectSound)
        {
            if (!canSfxMusic) return;
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

            if (effectSound == EnumEffectSound.TimeEnd)
            {
                musicSouce.loop = true;
                musicSouce.Play();
            }
        }

        public void StopEffectSound(EnumEffectSound effectSound)
        {
            var musicSouce = DictEffectMusic[effectSound];
            musicSouce.Stop();
        }

        public void SetSound(bool value)
        {
            //canSound = value;
            //PlayerPrefs.SetInt(StringPlayerPrefs.USE_SOUND, value? 1 : 0);
            if (value)
            {
                TurnOnBGMusic();
            }
            else
            {
                TurnOffBGMusic();
            }
            
        }

        public void TurnOffBGMusic()
        {
            DictBackgroundMusic[EnumBackgroundSound.HomeMusic].Stop();
        }

        public void TurnOnBGMusic()
        {
            DictBackgroundMusic[EnumBackgroundSound.HomeMusic].Play();
        }

        public void SetBGMusic(bool value)
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.USE_BGMUSIC, value ? 1 : 0);
            this.canBgMusic = value;
            if (canBgMusic)
            {
                TurnOnBGMusic();
            }
            else
            {
                TurnOffBGMusic();
            }
        }

        public void SetSfxSound(bool value)
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.USE_SFXSOUND, value ? 1 : 0);
            this.canSfxMusic = value;
        }
    }
}