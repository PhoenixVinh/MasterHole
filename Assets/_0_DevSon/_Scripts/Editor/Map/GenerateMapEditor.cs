using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor.Map
{
    public class GenerateMapEditor : EditorWindow
    {



        public string timerData = "Assets/DataModelNew_01/DataMissionText/Datatimer";
        public string missionData = "Assets/DataModelNew_01/DataMissionText/DataMission";
        public string itemMapData = "Assets/_Data/DataItemMap/DataMap2";
        public string saveMisionPath = "Assets/_Data/MissionNewSO";
        public string saveDataPath = "Assets/Resources/DataLevelNewSO";
        
        
            
        
      
        [MenuItem("Tools/Generate Data Map")]
        public static void ShowWindow()
        {
            GetWindow<GenerateMapEditor>("Generate Data Map");
        }
        
        private void OnGUI() {
            GUILayout.Label("Generate Map", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            timerData = EditorGUILayout.TextField(timerData);
            if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục time", timerData, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    timerData = "Assets" + selectedPath.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            
            // Trường để chọn thư mục lưu ScriptableObject
            GUILayout.Label("Thư mục mision:");
            EditorGUILayout.BeginHorizontal();
            missionData = EditorGUILayout.TextField(missionData);
            if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục mission", missionData, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    missionData = "Assets" + selectedPath.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            
            // Trường để chọn thư mục lưu ScriptableObject
            GUILayout.Label("Thư mục dataitem:");
            EditorGUILayout.BeginHorizontal();
            itemMapData = EditorGUILayout.TextField(itemMapData);
            if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục data item", itemMapData, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    itemMapData = "Assets" + selectedPath.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            
            // Trường để chọn thư mục lưu mission
            GUILayout.Label("Thư mục lưu mision:");
            EditorGUILayout.BeginHorizontal();
            saveMisionPath = EditorGUILayout.TextField(saveMisionPath);
            if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục luu mission", saveMisionPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    saveMisionPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            
            
            // Trường để chọn thư mục lưu map
            GUILayout.Label("Thư mục lưu map:");
            EditorGUILayout.BeginHorizontal();
            saveDataPath = EditorGUILayout.TextField(saveDataPath);
            if (GUILayout.Button("Chọn thư mục", GUILayout.Width(100)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Chọn thư mục luu map", saveDataPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    saveDataPath = "Assets" + selectedPath.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();
            


            if (GUILayout.Button("Create Data")) {
                CreateData();
            }
        }

        private void CreateData()
        {
            if (string.IsNullOrEmpty(timerData) || string.IsNullOrEmpty(missionData) || string.IsNullOrEmpty(itemMapData) || string.IsNullOrEmpty(saveMisionPath))
            {
                Debug.LogError("Vui lòng điền đầy đủ thông tin.");
                return;
            }
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { itemMapData });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                LevelSpawnData so = AssetDatabase.LoadAssetAtPath<LevelSpawnData>(assetPath);
                int index = int.Parse(so.name.Substring(so.name.LastIndexOf("_") + 1));
                string timerName = $"timer_{index:D3}.txt";
                float contenttimmer = float.Parse(File.ReadAllText(Path.Combine(timerData, timerName)));
                string missionName = $"mission_data_{index:D3}.txt";
                string contentMission = File.ReadAllText(Path.Combine(missionData, missionName));
                string[] missionDataArray = contentMission.Split(',');
               
                MissionSO mission = CreateMission(index.ToString(), missionDataArray, so);
                
                
                
                
                string SavePath = saveDataPath + $"/Data_Level_{index}.asset";
                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                }
                
                LevelGamePlaySO dataLevel = ScriptableObject.CreateInstance<LevelGamePlaySO>();
                dataLevel.name  = $"Data_Level_{index}";
                dataLevel.missionData = mission;
                dataLevel.timeToComplete = contenttimmer;
                dataLevel.levelSpawnData = so;
                
                

                //Debug.Log(index);
                //Debug.Log($"Loaded SO: {so.name.Substring(so.name.LastIndexOf("_"))} at {assetPath}");

                AssetDatabase.CreateAsset(dataLevel, SavePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"Created LevelGamePlaySO: {dataLevel.name} at {SavePath}");
               

            }
            
         
        }





        private MissionSO CreateMission(string name, string[] itemMission, LevelSpawnData dataitem)
        {
            MissionSO mission = ScriptableObject.CreateInstance<MissionSO>();



            mission.name = $"MissionLevel_{name}";


            string assetPath = "Assets/_Data/MissionNewSO/" + $"{mission.name}.asset";



            if (File.Exists(assetPath))
            {
                AssetDatabase.DeleteAsset(assetPath);
            }


            mission.misstionsData = new List<MissionData>();

            foreach (var item in dataitem.listItemSpawns)
            {
                if (itemMission.Contains(item.id))
                {
                    mission.misstionsData.Add(
                        new MissionData(
                            item.id,
                            item.listSpawnDatas.Count,
                            FindSprite(item.id)
                        )
                    );  
                }
            }
            
            AssetDatabase.CreateAsset(mission, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return mission;
          
            
           
        }
        
        
        
        
        
        public string parentFolderPath = "Assets/TextureObjectCapture";
        public string parentFolderPathOther = "Assets/Model/Texture2D";
        public string prefabFolderPathOther2 = "Assets/DataModelNew_01/Sprite";
        
        
        [ContextMenu("Finding Image")]
        public Sprite  FindSprite(string searchName)
        {
            string pathFinding = "";
         
            string[] subFolders = Directory.GetDirectories(parentFolderPath); // Lấy danh sách các thư mục con

            if (subFolders.Length > 0)
            {
                foreach (string subFolderPath in subFolders)
                {
                    
                    var result = FindImagesInFolder(subFolderPath, searchName); // Gọi hàm tìm kiếm trong từng thư mục con
                    if (result != "")
                    {
                        pathFinding = result;
                    }
                }
            }
            else
            {
                Debug.Log("Không có thư mục con nào trong: " + parentFolderPath);
            }
            
            
            // Finding in other Folder 
            if (pathFinding == "")
            {
                var result = findingOtherFolder(searchName);
                if (result != "")
                {
                    pathFinding = result;
                }
            }

            if (pathFinding == "")
            {
                Debug.LogError($"Could not find Images in folder {parentFolderPath}");
                return null;
            }
            else
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pathFinding);
                return sprite;
            }
            
            
        }
        
        public string findingOtherFolder(string searchName)
        {
           
            string[] allFiles =
                Directory.GetFiles(parentFolderPathOther, "*.png"); // Tìm kiếm trong cả thư mục con
         
            var foundImages = allFiles
                .Where(file =>
                    Path.GetFileName(file).ToLower() == $"{searchName.ToLower()}.png");

            if (foundImages != null)
            {
               
                foreach (string path in foundImages)
                {
                    return path;
                }
               
            }
          
            string[] allFiles2 =
                Directory.GetFiles(prefabFolderPathOther2, "*.asset"); // Tìm kiếm trong cả thư mục con
            var foundImages2 = allFiles2
                .Where(file =>
                    Path.GetFileName(file).ToLower() == $"{searchName.ToLower()}.asset");
            if (foundImages2 != null)
            {
                foreach (string path in foundImages2)
                {
                    return path;
                }
            }
            
            
            
            return "";
            
        }

        public string FindImagesInFolder(string folderPath, string searchName)
        {
            string[] imageExtensions = { ".png"};
            string[] allFiles =
                Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories); // Tìm kiếm trong cả thư mục con
         
            var foundImages = allFiles
                .Where(file =>
                  Path.GetFileName(file).ToLower() == $"{searchName.ToLower()}.png");

            if (foundImages != null)
            {
                Debug.Log($"Tìm thấy {foundImages} ảnh có tên chứa '{searchName}' trong thư mục: {folderPath}");
                foreach (string path in foundImages)
                {
                    return path;
                 
                  
                   

                }
               
            }

            return "";

        }

    }
}