using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class ObjectSpawnerEditor : EditorWindow
{

    public Transform parentTransforms;
    
    private string jsonFilePath = "Assets/ConvertedData.json";
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Vector2 scrollPosition;

    [MenuItem("Tools/Object Spawner")]
    public static void ShowWindow()
    {
        GetWindow<ObjectSpawnerEditor>("Object Spawner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Object Spawner", EditorStyles.boldLabel);

        // Input field for JSON file path
        GUILayout.Label("JSON File Path:");
        jsonFilePath = EditorGUILayout.TextField(jsonFilePath);
        parentTransforms = EditorGUILayout.ObjectField("Parent Transforms", parentTransforms, typeof(Transform), true) as Transform; 

        // Button to spawn objects
        if (GUILayout.Button("Spawn Objects"))
        {
            SpawnObjects();
        }

        // Button to clear spawned objects
        if (GUILayout.Button("Clear Spawned Objects"))
        {
            ClearSpawnedObjects();
        }

        // Display spawned objects count
        if (spawnedObjects.Count > 0)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            GUILayout.Label($"Spawned Objects: {spawnedObjects.Count}", EditorStyles.boldLabel);
            EditorGUILayout.EndScrollView();
        }
    }

    private void SpawnObjects()
    {
       
            // Clear existing spawned objects
            ClearSpawnedObjects();

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogError($"JSON file not found at: {jsonFilePath}");
                return;
            }

            // Read JSON file
            string jsonContent = File.ReadAllText(jsonFilePath);

            // Deserialize JSON to LevelData
            
            
            
            LevelConfig datas = JsonConvert.DeserializeObject<LevelConfig>(jsonContent);

            if (datas == null)
            {
                Debug.LogError("Failed to deserialize JSON or InstantiateData is null.");
                return;
            }
            
            

            List<InstantiateData> listObjectData = datas.InstantiateData.ToList();
            foreach (var spawner in datas.SpawnerData)
            {
                if (string.IsNullOrEmpty(spawner.pfbId))
                {
                    Debug.LogWarning("Skipping invalid InstantiateData entry.");
                    continue;
                }
                
                GameObject itemspawner = new GameObject("ItemSpawner");
                itemspawner.transform.SetParent(parentTransforms);
                var dataRef = listObjectData.FirstOrDefault( e => e.itemId  == spawner.pfbId);
                if(dataRef == null) continue;
                string prefabPath = $"Data/{dataRef.itemId}";
                GameObject prefab = Resources.Load<GameObject>(prefabPath);
                if (prefab == null) continue;
                foreach (var item in dataRef.itemData)
                {
                    if (item.pos == null || item.rot == null)
                    {
                        Debug.LogWarning($"Skipping invalid ItemData for itemId: {dataRef.itemId}");
                        continue;
                    }

                    // Convert position and rotation to Unity Vector3
                    Vector3 position = new Vector3(item.pos.x, item.pos.y, item.pos.z);
                    Vector3 rotation = new Vector3(item.rot.x, item.rot.y, item.rot.z);

                    // Instantiate the prefab
                    GameObject spawnedObject = Instantiate(prefab, itemspawner.transform);
                    spawnedObject.transform.position = position;
                    spawnedObject.transform.localEulerAngles = rotation;
                    spawnedObject.name = $"{dataRef.itemId}_{spawnedObjects.Count}";
                    
                    spawnedObjects.Add(spawnedObject);
                }
                itemspawner.transform.position = new Vector3(spawner.Pos.x, spawner.Pos.y, spawner.Pos.z);
                itemspawner.transform.rotation = Quaternion.Euler(new Vector3(spawner.Rot.x, spawner.Rot.y, spawner.Rot.z));
                itemspawner.transform.localScale = new Vector3(spawner.Size.x, spawner.Size.y, spawner.Size.z);
            }
            

            //Debug.Log($"Successfully spawned {spawnedObjects.Count} objects.");
       
       
    }

    private void ClearSpawnedObjects()
    {
        try
        {
            foreach (var obj in spawnedObjects)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }
            spawnedObjects.Clear();
            Debug.Log("Cleared all spawned objects.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error clearing spawned objects: {ex.Message}");
        }
    }
}