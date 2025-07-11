using UnityEditor;
using UnityEngine;

public class GenerateNewMapEditor : EditorWindow
{
    
    public MapSO mapSO;

    public string ObjectPath = "Assets/DataModelNew_01/Map/MapObjects/GameObject";

    public GameObject mapPrefab;

    [MenuItem("Tools/Map/Generate Map")]
    public static void ShowWindow()
    {
        GetWindow<GenerateNewMapEditor>("Map Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate New Map", EditorStyles.boldLabel);

        mapSO = (MapSO)EditorGUILayout.ObjectField("Map SO", mapSO, typeof(MapSO), false);

        if (GUILayout.Button("Generate Map"))
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        if(mapPrefab != null){
            DestroyImmediate(mapPrefab);
        }
        else {
            mapPrefab = new GameObject("MapPrefab");
            mapPrefab.transform.position = new Vector3(mapSO.offsetX, mapSO.offsetY, mapSO.offsetZ);
            foreach (var prefabData in mapSO.savedPrefabs)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{ObjectPath}/{prefabData.prefabName}.prefab");
                if (prefab != null)
                {
                    GameObject instance = Instantiate(prefab, mapPrefab.transform);
                    instance.transform.localPosition = new Vector3(prefabData.x*mapSO.cellSize, -1*mapSO.cellSize, prefabData.y*mapSO.cellSize);
                    instance.transform.localRotation = Quaternion.Euler(0, 0, prefabData.rotation);
                    instance.name = prefabData.prefabName;
                    

                    
                }
                else
                {
                    Debug.LogWarning($"Prefab not found: {prefabData.prefabName}");
                }
            }
        }
        
    }

    public void OnDestroy()
    {
        if (mapPrefab != null)
        {
            DestroyImmediate(mapPrefab);
        }
    }
}