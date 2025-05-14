using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Map
{
    public class CreateMissionPrefab : EditorWindow
    {
        public LevelGamePlaySO dataLevel;



        public GameObject parent;
        private Vector2 scrollPosition;

        public string prefabPath = "PrefabInstance/";

        public List<string> subfolderLoad = new List<string>()
        {
            "Drink",
            "hoa qua",
            "Junk food",
            "other utensils"
        };

        public Dictionary<string, int> itemDatas;
        public bool[] checks;

        public bool showMision = false;

        public string searchName = "food_4";
        public string parentFolderPath = "Assets/TextureObjectCapture";

        [MenuItem("Tools/DataLevel/Mission")]
        public static void ShowWindow(LevelGamePlaySO data)
        {

            CreateMissionPrefab window = GetWindow<CreateMissionPrefab>("Create Mission Prefab");
            window.dataLevel = data;

            window.Show();
        }



        public void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.LabelField("Mission Prefab", EditorStyles.boldLabel);
            if (GUILayout.Button("Spawn Mission"))
            {
                if (parent == null)
                {
                    parent = new GameObject("Parent");
                }

                itemDatas = new Dictionary<string, int>();
                SpawnObject();
                checks = new bool[itemDatas.Count];
                showMision = true;


            }

            if (showMision)
            {
                int i = 0;
                int rowIndex = 0;
                int itemCount = itemDatas.Count;
                foreach (var pair in itemDatas)
                {
                    if (i % 5 == 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        rowIndex++;
                    }

                    EditorGUILayout.BeginVertical(); // Để nhóm label, int field và toggle theo cột
                    EditorGUILayout.LabelField(pair.Key, GUILayout.Width(60));
                    EditorGUILayout.IntField(pair.Value, GUILayout.Width(60));
                    checks[i] = EditorGUILayout.Toggle(checks[i], GUILayout.Width(60));
                    EditorGUILayout.EndVertical();

                    i++;

                    if (i % 5 == 0 || i == itemCount)
                    {
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.Space();


                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Finding"))
            {
                FindImagesInChildrenFolders();
            }

            EditorGUILayout.EndScrollView();
        }


        private void SpawnObject()
        {
            while (parent.transform.childCount > 0)
            {
                DestroyImmediate(parent.transform.GetChild(0).gameObject);
            }

            Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
            foreach (var item in dataLevel.levelSpawnData.listItemSpawns)
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
                    itemDatas.Add(nameItem, 0);
                    prefabInstance = spawnedObject;

                }
                else
                {
                    prefabInstance = spawnedObjects[nameItem];

                }

                // Spawner Item
                foreach (var dataspawn in item.listSpawnDatas)
                {
                    itemDatas[nameItem]++;
                    GameObject itemSpawn = Instantiate(prefabInstance, parent.transform);
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



        public GameObject LoadPrefab(string prefabName, string searchPath = "")
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
                if (prefab != null) return prefab;
            }

            Debug.LogWarning($"Prefab '{prefabName}' not found in Resources");
            return null;
        }

        [ContextMenu("Finding Image")]
        void FindImagesInChildrenFolders()
        {
            if (Directory.Exists(parentFolderPath))
            {
                string[] subFolders = Directory.GetDirectories(parentFolderPath); // Lấy danh sách các thư mục con

                if (subFolders.Length > 0)
                {
                    foreach (string subFolderPath in subFolders)
                    {
                        Debug.Log("Tìm kiếm trong thư mục con: " + subFolderPath);
                        FindImagesInFolder(subFolderPath); // Gọi hàm tìm kiếm trong từng thư mục con
                    }
                }
                else
                {
                    Debug.Log("Không có thư mục con nào trong: " + parentFolderPath);
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy thư mục cha: " + parentFolderPath);
            }
        }

        void FindImagesInFolder(string folderPath)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tiff", ".tga" };
            string[] allFiles =
                Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories); // Tìm kiếm trong cả thư mục con

            var foundImages = allFiles
                .Where(file =>
                    imageExtensions.Contains(Path.GetExtension(file).ToLower()) &&
                    Path.GetFileNameWithoutExtension(file).ToLower() == searchName.ToLower());

            if (foundImages != null)
            {
                Debug.Log($"Tìm thấy {foundImages} ảnh có tên chứa '{searchName}' trong thư mục: {folderPath}");
                foreach (string path in foundImages)
                {
                    Debug.Log(path);

                }
            }
            else
            {
                Debug.Log($"Không tìm thấy ảnh nào có tên chứa '{searchName}' trong thư mục: {folderPath}");
            }
        
        }
    }
}