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

        public int AmountMissionItem = 0;


        public Dictionary<string, Mission> TypeItems = new Dictionary<string, Mission>();


        private List<GameObject> MissionItems = new List<GameObject>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }



        }

        private void CreateMissions()
        {
            AmountMissionItem = 0;
            DOTween.KillAll();

            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            TypeItems.Clear();
            int index = 0;
            foreach (var missionSo in MissionsSO.misstionsData)
            {
                GameObject mission = Instantiate(Mission, transform);
                mission.name = "Mission";
                mission.GetComponent<Mission>().SetData(missionSo, index);
                TypeItems[missionSo.idItem] = mission.GetComponent<Mission>();
                index++;
                AmountMissionItem += missionSo.AmountItems;
            }
        }


        public void CheckMinusItems(string itemType, GameObject item)
        {


            if (!TypeItems.ContainsKey(itemType))
            {

                return;
            }
            if (MissionItems.Contains(item))
            {
                MissionItems.Remove(item);
            }

            TypeItems[itemType].MinusItem();

            if (TypeItems[itemType].IsDone())
            {

                //GameObject game = TypeItems[itemType].gameObject; 
                TypeItems.Remove(itemType);
                SetIndex();

            }

            if (TypeItems.Count == 0)
            {
                WinLossEvent.OnWin?.Invoke();
            }
        }
        public void CheckWin()
        {
            if (TypeItems.Count == 0)
            {
                Debug.Log("WIN ????");
                WinLossEvent.OnWin?.Invoke();
            }
        }



        public void PassLevel()
        {
            WinLossEvent.OnWin?.Invoke();
        }

        public void SetIndex()
        {

            if (TypeItems.Count == 1) return;
            int index = 0;
            foreach (var value in TypeItems.Values)
            {
                value.SetIndexMission(index);
                index++;
            }
        }
        public Sprite GetSprite(string itemType)
        {
            if (!TypeItems.ContainsKey(itemType)) return null;
            return TypeItems[itemType].GetImage();
        }





        public List<GameObject> GetSuggestItems(int amount)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();

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
                if (!results.ContainsKey(typeItemString[indexRandom]))
                {
                    results[typeItemString[indexRandom]] = 1;
                } else
                {
                    if (TypeItems[typeItemString[indexRandom]].amountItem > results[typeItemString[indexRandom]])
                    {
                        results[typeItemString[indexRandom]]++;
                    }
                }
            }


            List<GameObject> resultGameObjects = new List<GameObject>();
            foreach (var item in results)
            {
                HashSet<int> postions = new HashSet<int>();
                for (int i = 0; i < item.Value; i++)
                {
                    int indexRandom2 = Random.Range(0, MissionItems.Count);
                    while (postions.Contains(indexRandom))
                    {
                        indexRandom = Random.Range(0, MissionItems.Count);
                    }
                    postions.Add(indexRandom);



                }

                foreach (var postion in postions)
                {
                    resultGameObjects.Add(MissionItems[postion]);
                }
            }

            return resultGameObjects;

        }



        public GameObject GetAnOtherSuggestItems(List<GameObject> items)
        {
           



            foreach (var itemmission in MissionItems)
            {
                if (!items.Contains(itemmission))
                {
                    return itemmission;
                }
            }
            return null;

        }
        
        

        public void SetData(MissionSO mission)
        {
            this.MissionsSO = mission;

            CreateMissions();
        }


        public void OnDestroy()
        {
            DOTween.KillAll();
        }

        public int GetClearFood()
        {
            int result = AmountMissionItem;
            foreach (var typeItem in TypeItems)
            {
                result -= typeItem.Value.GetAmountItem();
            }
            return result;
        }

        public List<string> GetNameMission()
        {
            List<string> result = new List<string>();
            foreach (var typeItem in TypeItems)
            {
                result.Add(typeItem.Key);
            }
            return result;

        }


        public void SetITemMissions(List<GameObject> missionItems)
        {
            this.MissionItems = missionItems;
        }



    }
}