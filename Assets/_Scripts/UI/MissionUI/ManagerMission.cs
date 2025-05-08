using System;
using System.Collections.Generic;
using _Scripts.Event;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.UI.MissionUI
{
    public class ManagerMission : MonoBehaviour
    {
        public static ManagerMission Instance;
        
        
        public GameObject Mission;
        
        // Misition For One Level => Get next level using Addressable
        public MissionSO MissionsSO;
        
        
        public Dictionary<string, Mission> TypeItems = new Dictionary<string, Mission>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
         


        }

        private void CreateMissions()
        {



            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            TypeItems.Clear();
            foreach (var missionSo in MissionsSO.misstionsData)
            {
                GameObject mission = Instantiate(Mission, transform);
                mission.name = "Mission";
                mission.GetComponent<Mission>().SetData(missionSo);
                TypeItems[missionSo.idItem] = mission.GetComponent<Mission>();
            }
        }


        public void CheckMinusItems(string itemType, Vector3 position)
        {
            if (!TypeItems.ContainsKey(itemType)) return;
            TypeItems[itemType].MinusItem(position);
            
            if (TypeItems[itemType].IsDone())
            {
               
                GameObject game = TypeItems[itemType].gameObject; 
                TypeItems.Remove(itemType);
                Sequence sequence = DOTween.Sequence();
                sequence.SetDelay(1.2f);
                sequence.SetUpdate(true);
                // Append rotation to the sequence
                sequence.Append(
                    game.transform.DORotate(
                            new Vector3(0, 360*3, 0),
                            1,
                            RotateMode.FastBeyond360
                        )
                        .SetEase(Ease.Linear) // Smooth, consistent speed
                );
               
                
                

                // Append a callback to destroy the GameObject when the sequence completes
                sequence.AppendCallback(() =>
                {
                    game.transform.DOScale(Vector3.one*0.3f, 0.5f).SetUpdate(true).OnComplete(
                        () =>
                        {
                            Destroy(game);
                        }
                        );
                   
                    
                });
                
            }

            if (TypeItems.Count == 0)
            {
                WinLossEvent.OnWin?.Invoke();
            }
        }

        public Sprite GetSprite(string itemType)
        {
            if (!TypeItems.ContainsKey(itemType)) return null;
            return TypeItems[itemType].GetImage();
        }
        
        
        public Dictionary<string, int> GetSuggestItems(int amount)
        {
            Dictionary<string, int > results = new Dictionary<string, int >();
            
            int amountTypeItem = Mathf.CeilToInt(TypeItems.Count / 2);
            amountTypeItem = amountTypeItem <= 3 ? amountTypeItem : 3;
            int indexRandom = -1;
            List<string> typeItemString = new List<string>();
            int amountSussgest = 0;
            foreach (var item in TypeItems)
            {
                typeItemString.Add(item.Key);
                
            }

            for (int i = 0; i < amountTypeItem + 1; i++)
            {
                amountSussgest += TypeItems[typeItemString[i]].GetAmountItem();
            }

            amount = amount <= amountSussgest ? amount : amountSussgest;
            
            for (int i = 0; i < amount; i++)
            {
                indexRandom = Random.Range(0, amountTypeItem + 1);
                if(!results.ContainsKey(typeItemString[indexRandom]))
                {
                    results[typeItemString[indexRandom]] = 1;
                }else
                {
                    if (TypeItems[typeItemString[indexRandom]].amountItem > results[typeItemString[indexRandom]])
                    {
                        results[typeItemString[indexRandom]]++;
                    }
                }
            }
            return results;
        }

        public void SetData(MissionSO  mission)
        {
            this.MissionsSO = mission;
            CreateMissions();
        }


        public void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}