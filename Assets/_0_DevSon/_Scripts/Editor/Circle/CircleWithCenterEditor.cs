using _Scripts.Editor;
using UnityEditor;
using UnityEngine;

public class CircleWithCenterEditor : CircleEditor
{
    public GameObject prefabCenterToSpawn;

    
    
    [MenuItem("Tools/Shape Editor/Circle/Circle With Center")]
    public static void ShowWindow()
    {
        GetWindow<CircleEditor>("Circle With Center Editor");
    }
    protected override void UpdatePreview()
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
        for (int i = 0; i < amountItem; i++)
        {
            float angle = i * (360f / amountItem);
            float radians = angle * Mathf.Deg2Rad;

            float x = centerPosition.x + radius * Mathf.Cos(radians);
            float z = centerPosition.z + radius * Mathf.Sin(radians);
            Vector3 position = new Vector3(x, centerPosition.y, z);

            // Instantiate a temporary preview object
            GameObject previewObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
            if (previewObject != null)
            {
                previewObject.transform.position = position;
                previewObject.transform.SetParent(previewParent.transform);
                previewObject.hideFlags = HideFlags.HideAndDontSave; // Hide in hierarchy and don't save
                previewObjects.Add(previewObject);
            }
        }

        
        GameObject centerObject = PrefabUtility.InstantiatePrefab(prefabCenterToSpawn) as GameObject;
        if (centerObject != null)
        {
            centerObject.transform.position = centerPosition;
            centerObject.transform.SetParent(previewParent.transform);
            centerObject.hideFlags = HideFlags.HideAndDontSave; // Hide in hierarchy and don't save
            previewObjects.Add(centerObject);
        }
        
        Debug.Log(this.previewObjects.Count);
      
    
    }

   
    protected override void GenerateObjectsInCircle()
    {
        
        if (prefabCenterToSpawn == null) return;
        
        GameObject newObject = Instantiate(prefabCenterToSpawn, parentObject.transform.position, Quaternion.identity);
        newObject.transform.SetParent(parentObject);
        base.GenerateObjectsInCircle();
    }

    protected override void OnGUI()
    {
        prefabCenterToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab Center to Spawn", prefabCenterToSpawn, typeof(GameObject), false);
        base.OnGUI();
        
        Repaint();
    }
    
    
}