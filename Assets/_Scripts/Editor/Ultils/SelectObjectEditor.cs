using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Ultils
{
    public class SelectObjectEditor : EditorWindow
    {
        public GameObject parentObject;
        public LevelSpawnData itemSpawnData;

        [MenuItem("Tools/Select Object Editor")]
        public static void ShowWindow()
        {
            GetWindow<SelectObjectEditor>("Select Object Editor");
        }
        private bool selectionChangedEnabled = false;
        private void OnGUI()
        {
            GUILayout.Label("Select Object Editor", EditorStyles.boldLabel);
            parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);
            itemSpawnData = (LevelSpawnData)EditorGUILayout.ObjectField("Item Spawn Data", itemSpawnData, typeof(LevelSpawnData), false);

            if (GUILayout.Button("Refresh Selection"))
            {
                OnSelectionChanged();
            }
          

            GUILayout.Space(10);

           
            if (GUILayout.Button("Turn On Selection Changed"))
            {
             
                selectionChangedEnabled = true;
            }
            
           
            if (GUILayout.Button("Turn Off Selection Changed"))
            {
               
                selectionChangedEnabled = false;
            }
           
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            // Check if parentObject and itemSpawnData are assigned
            if (!selectionChangedEnabled) return;
            if (parentObject == null || itemSpawnData == null) return;

            var selected = Selection.activeGameObject;
            if (selected == null || selected.transform.parent != parentObject.transform) return;

            foreach (var child in itemSpawnData.listItemSpawns)
            {
              
                if (child.id == selected.name )
                {
                    int index = 0;
                    foreach(var item in child.listSpawnDatas)
                    {
                       
                        if (selected.transform.position == item.p.ToVector3())
                        {
                            EditorUtility.DisplayDialog(
                                "Item Selected",
                                $"Name: {child.id}\nPosition: {index}",
                                "OK"
                            );
                        }
                        index++;
                       
                       
                        
                    }

                 
                    break;
                }
            }
        }
        
    }
}