using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor
{
    public class CircleEditor : BaseShapeEditor
    {
        
        public float radius = 5f;
        public int amountItem = 10;
        
        
        
        [MenuItem("Tools/Shape Editor/Circle/Normal Circle")]
        public static void ShowWindow()
        {
            GetWindow<CircleEditor>("Circle Editor");
        }
        
        
        
    
        
        protected virtual void OnGUI()
        {
            GUILayout.Label("Circle Shape", EditorStyles.boldLabel);
            
            prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", prefabToSpawn, typeof(GameObject), false);
            amountItem = EditorGUILayout.IntField("Amount Item", amountItem);
            // Adding new Field for chile 
            
            
            
            
            float newRadius = EditorGUILayout.Slider("Radius", radius, 0.1f, 100f); // Added slider with min/max
        
            if (newRadius != radius)
            {
                radius = newRadius;
                UpdatePreview();
            }
            parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true);

            if (GUILayout.Button("Generate Objects"))
            {
                if (prefabToSpawn != null)
                {
                    GenerateObjectsInCircle();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please assign a prefab to spawn.", "OK");
                }
            }
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
    }

    protected virtual void GenerateObjectsInCircle()
    {
        if (prefabToSpawn == null) return;

        Vector3 centerPosition = parentObject.transform.position;



        for (int i = 0; i < amountItem; i++)
        {
            float angle = i * (360f / amountItem);
            float radians = angle * Mathf.Deg2Rad;

            float x = centerPosition.x + radius * Mathf.Cos(radians);
            float z = centerPosition.z + radius * Mathf.Sin(radians);
            Vector3 position = new Vector3(x, centerPosition.y, z);

            GameObject newObject = Instantiate(prefabToSpawn, position, Quaternion.identity);

            if (parentObject != null)
            {
                newObject.transform.SetParent(parentObject);
            }
        }

        Debug.Log($"Generated {amountItem} objects in a circle with radius {radius}. Parent: {(parentObject != null ? parentObject.name : "None")}");

        // Clean up the preview after generation
        OnDisable();
        OnEnable(); // Re-enable for future previews
    }
        
        
    }
}