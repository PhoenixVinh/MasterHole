using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.ResourcesUI
{
    public class Energy : MonoBehaviour
    {
        public static Energy Instance;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
         
            
            Load();
            Save();
        }
        
        
        

        // [SerializeField] TMP_Text energyText;
        // [SerializeField] TMP_Text timeText;

        public int restoreDuration = 5;
        [SerializeField]private int maxEnergy = 25;
        
        
        // [Header("InFinityHealth")]
        // [SerializeField] private GameObject infinityHealth;
        // [SerializeField] private GameObject adding;
        //
        public int MaxEnergy => maxEnergy;
        private int currentEnergy = 5;
        public int CurrentEnergy => currentEnergy;
        
        private DateTime nextEnergyTime;
        private DateTime lastEnergyTime;
        private DateTime unlimitedEnergyEndTime;
        private bool isResoring = false;


        private const string NEXT_TIME = "NextTime";
        private const string LAST_TIME = "LastTime"; 
        
        
        private string timeValue;
        
       
        public string TimeValue => timeValue;
        
        public bool IsUnlimitedEnergy => unlimitedEnergyEndTime > DateTime.Now;
        
        
        public bool IsFullEnergy => currentEnergy >= maxEnergy;
        
        private void OnEnable()
        {
            if (!PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_ENERGY))
            {
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, 5);
                Load();
                StartCoroutine(RestoreEnergy());
            }
            else
            {
               
                Load();
                StartCoroutine(RestoreEnergy());
            }
        }

        [ContextMenu("Use Energy")]
        public bool UseEnergy()
        {
           
            //currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
    
            if (IsUnlimitedEnergy)
            {
                return true;
            }
            
            
            Debug.Log($"Current energy: {currentEnergy}");
          
            if (currentEnergy >= 0)
            {
                currentEnergy--;
                
                
                
               
                if (currentEnergy < maxEnergy)
                {
                    nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
                }
                
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
                StartCoroutine(RestoreEnergy());
                
                return true;
            }
            return false;
            
            
        }

        public void AddEnergy()
        {
            currentEnergy++;
          
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
            
            UpdateEnergyTimer();
            
        }

        private IEnumerator RestoreEnergy()
        {
           
            isResoring = true;

            while (this.unlimitedEnergyEndTime > DateTime.Now)
            {
                UpdateEnergyTimer();
               
                yield return null;
            }
            UpdateEnergyTimer();
            while (currentEnergy < maxEnergy)
            {
                DateTime currentDateTime = DateTime.Now;
                DateTime nextDateTime = nextEnergyTime;
                bool isEnergyAdding = false;

                while (currentDateTime > nextDateTime)
                {
                    if (currentEnergy < maxEnergy)
                    {
                        isEnergyAdding = true;
                        currentEnergy++;
                        //UpdateEnergy();
                        DateTime timeToAdd = lastEnergyTime > nextDateTime ? lastEnergyTime : nextDateTime;
                        nextDateTime = AddDuration(timeToAdd, restoreDuration);
                    }
                    else
                    {
                        break;
                    }
                   
                }
                if (isEnergyAdding)
                {
                    lastEnergyTime = DateTime.Now;
                    nextEnergyTime = nextDateTime;
                }

                UpdateEnergyTimer();
                //UpdateEnergy();
                Save();
                yield return null;
                

                
            }
            isResoring = false;

        }

        private DateTime AddDuration(DateTime time, int duration)
        {
            return time.AddSeconds(duration);
        }

        private void UpdateEnergyTimer()
        {
            if (IsUnlimitedEnergy)
            {
                TimeSpan remainingTime = unlimitedEnergyEndTime - DateTime.Now;
                if (remainingTime.TotalSeconds > 0)
                {
                    if (remainingTime.TotalHours < 24)
                    {
                        timeValue = String.Format("{0:D2}:{1:D2}:{2:D2}",remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                    }
                    else
                    {
                        timeValue = String.Format("{0}d:{1:D2}h", remainingTime.Days, remainingTime.Hours);
                    }
                    //timeText.text = timeValue;
                    
                }
                //infinityHealth.SetActive(true);
                //adding.SetActive(false);

                return;

            }
            else
            {
                //infinityHealth.SetActive(false);
                //adding.SetActive(true);
            }
            
            
            
            
            if (currentEnergy >= maxEnergy)
            {
                //timeText.text = "FULL";
                return;
            }

            TimeSpan time = nextEnergyTime - DateTime.Now;

            timeValue = String.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
            //timeText.text = timeValue;
        }



        public void LoadInfinity()
        {
            unlimitedEnergyEndTime = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            StopAllCoroutines();
            StartCoroutine(RestoreEnergy());
            
        }

        private void Load()
        {
            
            if(!PlayerPrefs.HasKey(StringPlayerPrefs.CURRENT_ENERGY)){
                PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, 5);
            }
            
            currentEnergy = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_ENERGY);
            
            if (!PlayerPrefs.HasKey(NEXT_TIME))
            {
                PlayerPrefs.SetString(NEXT_TIME, AddDuration(DateTime.Now, restoreDuration).ToString());
                
                PlayerPrefs.SetString(LAST_TIME, DateTime.Now.ToString());
            }

            if (!PlayerPrefs.HasKey(StringPlayerPrefs.UNLIMITED_TIME))
            {
                PlayerPrefs.SetString(StringPlayerPrefs.UNLIMITED_TIME, DateTime.Now.ToString());
            }
            
            
            unlimitedEnergyEndTime = Utills.StringToDate(PlayerPrefs.GetString(StringPlayerPrefs.UNLIMITED_TIME));
            nextEnergyTime =  Utills.StringToDate(PlayerPrefs.GetString(NEXT_TIME));
            lastEnergyTime =  Utills.StringToDate(PlayerPrefs.GetString(LAST_TIME));

        }


        private void Save()
        {
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
            PlayerPrefs.SetString(NEXT_TIME, nextEnergyTime.ToString());
            PlayerPrefs.SetString(LAST_TIME, lastEnergyTime.ToString());
            
        }


        public void AddMaxEnergy()
        {
            this.currentEnergy = maxEnergy;
            PlayerPrefs.SetInt(StringPlayerPrefs.CURRENT_ENERGY, currentEnergy);
            //UpdateEnergy();
            UpdateEnergyTimer();
            
        }

        [ContextMenu("USE INFINITY HEALTH")]
        public void UseInfinityHealth()
        {
            DateTime timeInfinity = DateTime.Now.AddSeconds(100);
            PlayerPrefs.SetString(StringPlayerPrefs.UNLIMITED_TIME, timeInfinity.ToString());
        }
        
        
        
    }
}