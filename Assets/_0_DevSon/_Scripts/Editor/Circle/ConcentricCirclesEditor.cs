

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace _Scripts.Editor
{
    public class ConcentricCirclesEditor : BaseShapeEditor
    {
        public GameObject prefabToSpawn;
        public float radiusIncrement = 2f;
        public int numberIncreaseInCircle = 5;
        public int numbersCircle = 3;
      
       
        [MenuItem("Tools/Shape Editor/Circle/Concentric Circles")]
        public static void ShowWindow()
        {
            GetWindow<ConcentricCirclesEditor>("Concentric Circles");
        }

        protected virtual void OnGUI()
        {
            GUILayout.Label("Concentric Circles Generator", EditorStyles.boldLabel);
            prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", prefabToSpawn, typeof(GameObject), false);
            radiusIncrement = EditorGUILayout.FloatField("Radius Increment", radiusIncrement);
            numberIncreaseInCircle = EditorGUILayout.IntField("Number of Circles", numberIncreaseInCircle);
            parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true);

            if (GUILayout.Button("Generate Objects"))
            {
                if (prefabToSpawn != null)
                {
                    GenerateConcentricCircles();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please assign a prefab to spawn.", "OK");
                }
            }

            if (GUI.changed)
            {
                UpdatePreview();
            }
        }

        protected virtual void UpdatePreview()
        {
            if (prefabToSpawn == null) return;
            if(parentObject == null) return;
            // Clear existing preview objects
            foreach (GameObject obj in previewObjects)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }
            previewObjects.Clear();
            
           

            Vector3 centerPosition = parentObject.position;
        
            
            
            for (int i = 0; i < numbersCircle; i++)
            {
                
                float currentRadius =  i * radiusIncrement;
                for (int j = 0; j < 1 + numberIncreaseInCircle * i; j++)
                {
                    float angle = j * (360f / 1 + numberIncreaseInCircle * i) * Mathf.Deg2Rad;
                    float x = centerPosition.x + currentRadius * Mathf.Cos(angle);
                    float z = centerPosition.z + currentRadius * Mathf.Sin(angle);
                    Vector3 position = new Vector3(x, centerPosition.y, z);

                    GameObject previewObject = PrefabUtility.InstantiatePrefab(prefabToSpawn) as GameObject;
                    if (previewObject != null)
                    {
                        previewObject.transform.SetParent(previewParent.transform);
                        previewObject.transform.position = position;
                        previewObject.transform.rotation = Quaternion.identity; // Or any desired rotation
                        previewObject.hideFlags = HideFlags.HideAndDontSave;
                        previewObjects.Add(previewObject);
                    }
                }
            }
        }

        protected virtual void GenerateConcentricCircles()
        {
            if (prefabToSpawn == null) return;

            Vector3 centerPosition = parentObject != null ? parentObject.position : Vector3.zero;

            
            OnDisable();
            OnEnable();
        }

        protected virtual void OnEnable()
        {
            base.OnEnable();
            UpdatePreview();
        }

        protected virtual void OnDisable()
        {
            base.OnDisable();
            // Clean up preview objects
            foreach (GameObject obj in previewObjects)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }
            previewObjects.Clear();

            if (previewParent != null)
            {
                DestroyImmediate(previewParent.gameObject);
                previewParent = null;
            }
        }
    }
}