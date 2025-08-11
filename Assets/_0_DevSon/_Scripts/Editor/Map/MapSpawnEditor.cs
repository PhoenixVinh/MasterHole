using System.Collections.Generic;
using _Scripts.Editor.Ultils;
using _Scripts.Map.MapSpawnItem;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Map
{
    public class MapSpawnEditor : EditorWindow
    {
        
        public LevelGamePlaySO levelGamePlaySO;
        
        private LevelSpawnData levelSpawnData;

        public Dictionary<string, List<ItemSpawnData>> itemDatas = new Dictionary<string, List<ItemSpawnData>>(); 
        
        
        public Transform parentSpawn; 
        
        
        
        public string prefabPath = "PrefabInstance/";
        public List<string> subfolderLoad = new List<string>()
        {
            "Drink",
            "hoa qua",
            "Junk food",
            "other utensils",
            "GameObject"
        };
        
        
        [MenuItem("Tools/Map Spawn Editor")]
        public static void ShowWindow()
        {
            GetWindow<MapSpawnEditor>("Map Spawn Editor");
        }

        public bool ShowEditorSize = false;
        public GameObject map;
       
        public Vector3 mapPosition;
        public Vector3 mapSize;
        public void OnGUI()
        {
            
            GUILayout.Label("Change Map ", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            levelGamePlaySO = (LevelGamePlaySO)EditorGUILayout.ObjectField("Item Spawn", levelGamePlaySO, typeof(LevelGamePlaySO), false);
            //levelSpawnData = (LevelSpawnData)EditorGUILayout.ObjectField("Item Spawn", levelSpawnData, typeof(LevelSpawnData), false);
            parentSpawn = (Transform)EditorGUILayout.ObjectField("Parent Spawn", parentSpawn, typeof(Transform), true);
            if (levelGamePlaySO == null) return;
            
            if (EditorGUI.EndChangeCheck())
            {
                while (parentSpawn.childCount > 0)
                {
                    DestroyImmediate(parentSpawn.GetChild(0).gameObject);
                }

               
            
                //GetData();
                //SpawnObject();
                ShowEditorSize = false;
            }
          
            

            if (GUILayout.Button("Spawn Map"))
            {
                if (levelGamePlaySO != null)
                {
                   
                    GetData();
                    SpawnObject();
                    SpawnMap();
                    ShowEditorSize = true;
                    
                  
                   
                }
                
            }
            
            if (ShowEditorSize)
            {
              
                
                //GUILayout.BeginVertical();
                
                int index = 0;
              
                
                
                this.mapPosition = EditorGUILayout.Vector3Field("Map Position", this.mapPosition);
                this.mapSize = EditorGUILayout.Vector3Field("Map Size", this.mapSize);
                
                
                if (EditorGUI.EndChangeCheck())
                {
                    ShowItemAgain();
                    this.levelGamePlaySO.mapPosition = mapPosition;
                    this.levelGamePlaySO.mapScale = mapSize;
                    //EditorUtility.SetDirty(this.levelGamePlaySO);
                    AssetDatabase.SaveAssets();
                    
                }
               

     
            }

            if (GUILayout.Button("Clear"))
            {
                var serializedObject = new SerializedObject(levelGamePlaySO);
                serializedObject.ApplyModifiedProperties();
               
                EditorUtility.SetDirty(levelGamePlaySO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                itemDatas.Clear();
                while (parentSpawn.childCount > 0)
                {
                    DestroyImmediate(parentSpawn.GetChild(0).gameObject);
                }
                
                
                levelGamePlaySO = null;
                ShowEditorSize = false;

                DestroyImmediate(map);

            }

          
            
        }

        private void ShowItemAgain()
        {
            map.transform.position = mapPosition;
            map.transform.localScale = mapSize;
        }

        private void SpawnMap()
        {
            if (map != null)
            {
                map.transform.position = levelGamePlaySO.mapPosition;
                map.transform.localScale = levelGamePlaySO.mapScale;
            }
            else
            {
                GameObject loadMap = Resources.Load<GameObject>("Map/Prefab/Map");
                
                map = Instantiate(loadMap, levelGamePlaySO.mapPosition, Quaternion.identity);
               
                map.transform.localScale = levelGamePlaySO.mapScale;
                
            }
        }


        private void SpawnObject()
        {
            while (parentSpawn.childCount > 0)
            {
                DestroyImmediate(parentSpawn.GetChild(0).gameObject);
            }
             Dictionary<string, GameObject> spawnedObjects = new Dictionary<string,GameObject>();
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
                // Spawner Item
                foreach (var dataspawn in item.listSpawnDatas)
                {
                    GameObject itemSpawn = Instantiate(prefabInstance, parentSpawn);
                    itemSpawn.name = prefabInstance.name;
                    itemSpawn.transform.position = dataspawn.p.ToVector3();
                    itemSpawn.transform.rotation = Quaternion.Euler(dataspawn.r.ToVector3());
                    itemSpawn.transform.localScale = dataspawn.s.ToVector3();
                    if (dataspawn.kinematic)
                    {
                        itemSpawn.GetComponent<Rigidbody>().isKinematic = true;
                    }
                
                }
                
                
                
            }
        }

        private void GetData()
        {
            itemDatas = new Dictionary<string, List<ItemSpawnData>>();
            levelSpawnData = levelGamePlaySO.levelSpawnData;
            this.mapPosition = levelGamePlaySO.mapPosition;
            this.mapSize = levelGamePlaySO.mapScale;
            foreach (var listItemSpawn in levelSpawnData.listItemSpawns)
            {
                foreach (var data in listItemSpawn.listSpawnDatas)
                {
                    if (!itemDatas.ContainsKey(listItemSpawn.id))
                    {
                        itemDatas[listItemSpawn.id] = new List<ItemSpawnData>();
                        itemDatas[listItemSpawn.id].Add(data);
                       
                    }
                    else
                    {
                        itemDatas[listItemSpawn.id].Add(data);
                    }
                     
                }
            }
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

        public void OnDestroy()
        {
            
            
            var serializedObject = new SerializedObject(levelGamePlaySO);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(levelGamePlaySO);
            AssetDatabase.SaveAssets();
            itemDatas = null;
           
            while (parentSpawn.childCount > 0)
            {
                DestroyImmediate(parentSpawn.GetChild(0).gameObject);
            }
            
            parentSpawn = null;
            
            ShowEditorSize = false;
            
            levelGamePlaySO = null;
            DestroyImmediate(map);
        }
    }
}