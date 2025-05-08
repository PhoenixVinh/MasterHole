using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Triangle
{
    public class TriangleEditor: BaseShapeEditor
    {
        public int amountRows; 
        public float distance;
        
        
        
        [MenuItem("Tools/Shape Editor/Triangle/Normal Triangle")]
        public static void ShowWindow()
        {
            GetWindow<TriangleEditor>("Triangle Editor");
        }


        protected void OnGUI()
        {
            GUILayout.Label("Triangle Shape", EditorStyles.boldLabel);
            
            prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", prefabToSpawn, typeof(GameObject), false);
            amountRows = EditorGUILayout.IntField("Amount of Rows", amountRows);
       
       
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
                    GenerateObjectsInTriangle();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please assign a prefab to spawn.", "OK");
                }
            }
        }

        protected virtual void GenerateObjectsInTriangle()
        {
            if (prefabToSpawn == null) return;

            Vector3 centerPosition = parentObject.transform.position;
            for (int i = 0; i < amountRows; i++)
            {
                float start = -(i / 2f);
                float end = (i/2f);

                do
                {
                   
                    Vector3 position = new Vector3(centerPosition.x + start*distance , centerPosition.y , -i*0.5f*distance + centerPosition.z);
                    GameObject newObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                    if (parentObject != null)
                    {
                        newObject.transform.SetParent(parentObject);
                    }
                    start += 1f;
                    
                    
                }while (start <= end);
            }
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

            for (int i = 0; i < amountRows; i++)
            {
                float start = -(i / 2f);
                float end = (i/2f);

                do
                {
                    GameObject previewObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
                    Vector3 position = new Vector3(centerPosition.x + start*distance , centerPosition.y , -i*0.5f*distance + centerPosition.z);
                    if (previewObject != null)
                    {
                      
                        previewObject.transform.SetParent(previewParent.transform);
                        previewObject.transform.position = position;
                        previewObject.hideFlags = HideFlags.HideAndDontSave; // Hide in hierarchy and don't save
                        previewObjects.Add(previewObject);
                    }
                    start += 1f;
                    
                    
                }while (start <= end);
            }
        }
        
        
        
    }
}