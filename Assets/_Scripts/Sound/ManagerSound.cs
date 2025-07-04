using System;
using System.Collections.Generic;
using _Scripts.Firebase;
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


        public bool isLevelCoin = false;
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
                int getcurrentLevel = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
                if (getcurrentLevel != 1)
                {
                    DictBackgroundMusic[EnumBackgroundSound.HomeMusic].Play();
                }
                
                //DictBackgroundMusic[EnumBackgroundSound.InGameMusic].Stop();
            }
            else
            {
                TurnOffBGMusic();
            }
                
           
        }

        public void StopAllSoundSFX()
        {
            DictEffectMusic[EnumEffectSound.Magnet].Stop();
            DictEffectMusic[EnumEffectSound.TimeEnd].Stop();
            DictEffectMusic[EnumEffectSound.Ice].Stop();
        }

        public void ChangeBackgroundMusic(EnumBackgroundSound backgroundSound)
        {
            foreach (var audioSource in DictBackgroundMusic)
            {
                audioSource.Value.Stop();
            }
            if (!canBgMusic) return;
            
            DictBackgroundMusic[backgroundSound].volume = volumeBg;
            DictBackgroundMusic[backgroundSound].Play();
        }

        public void PlayEffectSound(EnumEffectSound effectSound)
        {
            if (!canSfxMusic) return;

            if (effectSound == EnumEffectSound.EatItem && isLevelCoin)
            {
                return;
            }
            
            var musicSouce = DictEffectMusic[effectSound];
            musicSouce.volume = volumeSfx;
            musicSouce.Play();

            if (effectSound == EnumEffectSound.Magnet || effectSound == EnumEffectSound.TimeEnd || effectSound == EnumEffectSound.Ice)
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

        public void SetSound(bool value, bool isHome)
        {
            //canSound = value;
            //PlayerPrefs.SetInt(StringPlayerPrefs.USE_SOUND, value? 1 : 0);
            if (value)
            {
                TurnOnBGMusic(isHome);
            }
            else
            {
                TurnOffBGMusic();
            }
            
        }

        public void TurnOffBGMusic()
        {
            DictBackgroundMusic[EnumBackgroundSound.HomeMusic].Stop();
            DictBackgroundMusic[EnumBackgroundSound.InGameMusic].Stop();
        }

        public void TurnOnBGMusic(bool isHome)
        {

            if (isHome)
            {
                ChangeBackgroundMusic(EnumBackgroundSound.HomeMusic);
            }
            else
            {
                ChangeBackgroundMusic(EnumBackgroundSound.InGameMusic);
            }
            
            //DictBackgroundMusic[EnumBackgroundSound.InGameMusic].Play();
        }

        public void SetBGMusic(bool value, bool isHome)
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.USE_BGMUSIC, value ? 1 : 0);
            this.canBgMusic = value;
            if (canBgMusic)
            {
                TurnOnBGMusic(isHome);
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
            if (value == false)
            {
                StopAllSoundSFX();
            }
        }

        public void SetValueLevelCoint(bool isLevelCoin)
        {
            this.isLevelCoin = isLevelCoin;
        }
    }
}