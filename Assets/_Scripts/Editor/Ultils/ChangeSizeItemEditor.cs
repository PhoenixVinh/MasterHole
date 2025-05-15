using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Ultils
{
    public class ChangeSizeItemEditor : EditorWindow
    {
        public LevelSpawnData levelSpawnData;

        public Dictionary<string, List<ItemSpawnData>> itemDatas = new Dictionary<string, List<ItemSpawnData>>(); 
        
        
        public Transform parentSpawn;

        public string searchITems = "";
        
        public string prefabPath = "PrefabInstance/";
        public List<string> subfolderLoad = new List<string>()
        {
            "Drink",
            "hoa qua",
            "Junk food",
            "other utensils"
        };
        
        
        [MenuItem("Tools/Change Size Editor")]
        public static void ShowWindow()
        {
            GetWindow<ChangeSizeItemEditor>("Change Size Editor");
        }

        public bool ShowEditorSize = false;
        public List<Scale> Size;
        private Vector3 scrollPosition = new Vector3(0, 0, 0);
        public void OnGUI()
        {
            
            GUILayout.Label("Change Size ", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            levelSpawnData = (LevelSpawnData)EditorGUILayout.ObjectField("Item Spawn", levelSpawnData, typeof(LevelSpawnData), false);
            parentSpawn = (Transform)EditorGUILayout.ObjectField("Parent Spawn", parentSpawn, typeof(Transform), true);
            searchITems = EditorGUILayout.TextField("Search Items", searchITems);
            
            if (levelSpawnData == null) return;
            
            if (EditorGUI.EndChangeCheck())
            {
                while (parentSpawn.childCount > 0)
                {
                    DestroyImmediate(parentSpawn.GetChild(0).gameObject);
                }

                Size = null;
            
                //GetData();
                //SpawnObject();
                ShowEditorSize = false;
            }
          
            

            if (GUILayout.Button("Spawn Item"))
            {
                if (levelSpawnData != null)
                {
                   
                    GetData();
                    SpawnObject();
                    ShowEditorSize = true;
                    
                  
                   
                }
                
            }
            
            if (ShowEditorSize)
            {
              
                
                //GUILayout.BeginVertical();
                
                int index = 0;
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                EditorGUI.BeginChangeCheck();
                foreach (var item in itemDatas)
                {
                    if (item.Key != null && (item.Key.Contains(searchITems) || searchITems == ""))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(item.Key);  
                        GUILayout.BeginVertical();
                        Size[index].x = EditorGUILayout.Slider(Size[index].x, 0,10);
                        Size[index].y = EditorGUILayout.Slider(Size[index].y, 0,10);
                        Size[index].z = EditorGUILayout.Slider(Size[index].z, 0,10);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    
                    index++;
                }
                GUILayout.EndScrollView();
                if (EditorGUI.EndChangeCheck())
                {
                    ShowItemAgain();
                }
               

     
            }

            if (GUILayout.Button("Clear"))
            {
                var serializedObject = new SerializedObject(levelSpawnData);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(levelSpawnData);
                AssetDatabase.SaveAssets();
                itemDatas.Clear();
                while (parentSpawn.childCount > 0)
                {
                    DestroyImmediate(parentSpawn.GetChild(0).gameObject);
                }
                
                
                levelSpawnData = null;
                ShowEditorSize = false;

                Size = null;

            }

          
            
        }

        private void ShowItemAgain()
        {
            int index = 0;
            foreach (var item in itemDatas)
            {
                foreach (var i in item.Value)
                {
                    i.s = Size[index];
                }
                index++;
            }
            SpawnObject();
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
            Size = new List<Scale>();
            
            foreach (var listItemSpawn in levelSpawnData.listItemSpawns)
            {
                foreach (var data in listItemSpawn.listSpawnDatas)
                {
                    if (!itemDatas.ContainsKey(listItemSpawn.id))
                    {
                        itemDatas[listItemSpawn.id] = new List<ItemSpawnData>();
                        itemDatas[listItemSpawn.id].Add(data);
                        Debug.Log("Scale: " + data.s.ToVector3().ToString());
                        Size.Add(new Scale(data.s.ToVector3()));
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
            
            var serializedObject = new SerializedObject(levelSpawnData);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(levelSpawnData);
            AssetDatabase.SaveAssets();
            itemDatas = null;
            Size = null;
            while (parentSpawn.childCount > 0)
            {
                DestroyImmediate(parentSpawn.GetChild(0).gameObject);
            }
            
            parentSpawn = null;
            
            ShowEditorSize = false;
            
            levelSpawnData = null;
        }
    }
}