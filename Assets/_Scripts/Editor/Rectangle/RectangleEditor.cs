using _Scripts.Editor;
using UnityEditor;
using UnityEngine;

public class RectangleEditor : BaseShapeEditor
{

    public float distance = 0.1f;
    public int row = 6;
    public int col = 5;
    
    
    
    [MenuItem("Tools/Shape Editor/Rectangle/Normal Rectangle")]
    public static void ShowWindow()
    {
        GetWindow<RectangleEditor>("Rectangle Editor");
    }
    
    
    protected virtual void OnGUI()
    {
        GUILayout.Label("Rectangle Shape", EditorStyles.boldLabel);
            
        prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", prefabToSpawn, typeof(GameObject), false);
        row = EditorGUILayout.IntField("Amount Row", row);
        col = EditorGUILayout.IntField("Amount Col", col);
       
       
        // Adding new Field for chile 
            
            
            
            
        float newDistance = EditorGUILayout.Slider("distance", distance, 0.1f, 20f); // Added slider with min/max
        
        if (newDistance != distance)
        {
            distance = newDistance;
            UpdatePreview();
        }
        parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true);

        if (GUILayout.Button("Generate Objects"))
        {
            if (prefabToSpawn != null)
            {
                GenerateObjectsInRectangle();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a prefab to spawn.", "OK");
            }
        }
    }

    protected virtual void GenerateObjectsInRectangle()
    {
        if (prefabToSpawn == null) return;

        Vector3 centerPosition = parentObject.transform.position;
        
        var z = row % 2 == 0 ? distance * (row /2f - 0.5) : distance * Mathf.FloorToInt(row /2f) ;
        var x = col % 2 == 0 ? distance * (col /2f - 0.5) : distance * Mathf.FloorToInt(col /2f) ;
        Vector3 distanceCenter = new Vector3((float)x , 0, (float)z);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Vector3 position = new Vector3(centerPosition.x + distance * j, centerPosition.y,
                    centerPosition.z + distance * i) - distanceCenter;  
                                 
                
                
                GameObject newObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                if (parentObject != null)
                {
                    newObject.transform.SetParent(parentObject);
                }
                
            }
        }


    
        Debug.Log($"Generated {row * col } objects in a triangle with distanve {distance}. Parent: {(parentObject != null ? parentObject.name : "None")}");

        // Clean up the preview after generation
        OnDisable();
        OnEnable(); // Re-enable for future previews
    }


    protected virtual void UpdatePreview()
    {
        if (prefabToSpawn == null) return;

        // Clear existing preview objects
        foreach (GameObject obj in previewObjects)
        {
            if (obj != null)
            {
                DestroyImmediate(obj);
            }
        }
        previewObjects.Clear();

        Vector3 centerPosition = parentObject.transform.position;
        // Calculate distance From (0,0) to the center of rectangle 
        
        var z = row % 2 == 0 ? distance * (row /2f - 0.5) : distance * Mathf.FloorToInt(row /2f) ;
        var x = col % 2 == 0 ? distance * (col /2f - 0.5) : distance * Mathf.FloorToInt(col /2f) ;
        Vector3 distanceCenter = new Vector3((float)x , 0, (float)z);
        

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Vector3 position = new Vector3(centerPosition.x + distance * j, centerPosition.y,
                    centerPosition.z + distance * i); 
                                 
                
                
                GameObject previewObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
                
                if (previewObject != null)
                {
                    previewObject.transform.position = position - distanceCenter;
                    previewObject.transform.SetParent(previewParent.transform);
                    previewObject.hideFlags = HideFlags.HideAndDontSave; // Hide in hierarchy and don't save
                    previewObjects.Add(previewObject);
                }
                
            }
        }
        
        

        
    }
}