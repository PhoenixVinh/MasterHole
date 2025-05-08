
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Map.MapSpawnItem
{
    public class SpawnItemMap : MonoBehaviour
    {
        public static SpawnItemMap Instance;
        
       
        
        public LevelSpawnData levelSpawnData;
        public string prefabPath = "PrefabInstance/";
        public List<string> subfolderLoad;
        public List<ItemScoreData> itemScores;


        public Dictionary<string, List<GameObject>> mapObjects;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                mapObjects = new Dictionary<string, List<GameObject>>();
            }
            else
            {
                Destroy(gameObject);
            }
            //Instance = this;
            //SpawnMap();
            //Get Scroce Data 
          
            
        }

        







        public void SetData(LevelSpawnData levelSpawnData, List<ItemScoreData> itemScoreData)
        {
            this.levelSpawnData = ScriptableObject.CreateInstance<LevelSpawnData>();
            this.levelSpawnData = levelSpawnData;
            this.itemScores.Clear();
            this.itemScores = itemScoreData;
            SpawnMap();
        }
        
        [ContextMenu("Spawn")]
        public void SpawnMap()
        {
            
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            if (levelSpawnData == null) return;
            if (mapObjects != null)
            {
                mapObjects.Clear();
            }
            else
            {
                mapObjects = new Dictionary<string, List<GameObject>>();
            }
            
            Dictionary<string, GameObject> spawnedObjects = new Dictionary<string,GameObject>();
            spawnedObjects.Clear();
            foreach(var item in levelSpawnData.listItemSpawns)
            {
                string nameItem = item.id;
                
                GameObject prefabInstance = null;
                if (!spawnedObjects.ContainsKey(nameItem))
                {
                    GameObject spawnedObject = LoadPrefab(nameItem, prefabPath);
                    
                    if (spawnedObject == null)
                    {
                        Debug.LogError($"Could not find Spawn Item {nameItem}");
                        continue;
                    }
                    spawnedObjects.Add(nameItem, spawnedObject);
                    prefabInstance = spawnedObject;
                    
                }
                else
                {
                    prefabInstance = spawnedObjects[nameItem];
                }

                int scoreItem = 1;
                foreach (var score in itemScores)
                {
                    if (score.itemName == nameItem)
                    {
                        scoreItem = score.score;
                        break;
                    }
                }
                
                
                
                // Spawner Item
                foreach (var dataspawn in item.listSpawnDatas)
                {
                    GameObject itemSpawn = Instantiate(prefabInstance, transform);
                    itemSpawn.name = prefabInstance.name;
                    itemSpawn.transform.position = dataspawn.p.ToVector3();
                    itemSpawn.transform.rotation = Quaternion.Euler(dataspawn.r.ToVector3());
                    itemSpawn.transform.localScale = dataspawn.s.ToVector3();
                    if (dataspawn.kinematic)
                    {
                        itemSpawn.GetComponent<Rigidbody>().isKinematic = true;
                    }

                    Item itemType =  itemSpawn.AddComponent<Item>();
                    itemType.SetData(itemSpawn.name, scoreItem);
                    if (!mapObjects.ContainsKey(nameItem))
                    {
                        mapObjects.Add(nameItem, new List<GameObject>());
                        mapObjects[nameItem].Add(itemSpawn);
                    }
                    else
                    {
                        mapObjects[nameItem].Add(itemSpawn);
                    }
                }
                
                
                
            }

            levelSpawnData = null;
        }

        public List<GameObject> GetMappingObject(Dictionary<string, int> suggestItems)
        {
            List<GameObject> results = new List<GameObject>();
            foreach (var item in suggestItems)
            {
                HashSet<int> postions = new HashSet<int>();
                for (int i = 0; i < item.Value; i++)
                {
                    int indexRandom= Random.Range(0, mapObjects[item.Key].Count);
                    while (postions.Contains(indexRandom))
                    {
                        indexRandom = Random.Range(0, mapObjects[item.Key].Count);
                    }
                    postions.Add(indexRandom);
                    
                   
                    
                }

                foreach (var postion in postions)
                {
                    results.Add(mapObjects[item.Key][postion]);
                }
            }
            
            return results;
        }
        
        
        
        
        public  GameObject LoadPrefab(string prefabName, string searchPath = "")
        {
            // Construct the full path
            string fullPath = string.IsNullOrEmpty(searchPath) ? prefabName : $"{searchPath}{prefabName}";
        
            // Try to load the prefab
            GameObject prefab = Resources.Load<GameObject>(fullPath);
        
            if (prefab != null)
            {
                return prefab;
            }
            
            // Find Assets in the all Subforlder

            foreach (var name in subfolderLoad)
            {
                prefab = Resources.Load<GameObject>($"{searchPath}{name}/{prefabName}");
                if(prefab != null) return prefab;
            }
            Debug.LogWarning($"Prefab '{prefabName}' not found in Resources");
            return null;
        }
        
        public void RemoveItem(GameObject item)
        {
            string name = item.transform.parent.name;
            mapObjects[name].Remove(item.transform.parent.gameObject);
        }
        
    }
}