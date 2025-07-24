using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map.TestGenerateMap;
using System;
using System.Linq;
using Unity.VisualScripting;
using System.IO;

[Serializable]
public struct RuleTile
{
    public int topleft;
    public int topRight;
    public int bottomLeft;
    public int bottomRight;
    public GameObject prefab;
}



namespace Map.TestGenerateMap
{
    public class TestGenerateMap : MonoBehaviour
    {

        [Header("Level Spawn Data")]
        [Tooltip("Assign the LevelSpawnData asset that contains the spawn data for the map.")]

        public LevelGamePlaySO levelGamePlay;



        private LevelSpawnData levelSpawnData;

        public List<Vector3> mapPositions;

        public List<RuleTile> ruleTiles;
        public int[,] matrix;

        public bool[,] visited;

        public float minX;
        public float minZ;


        public GameObject ParentItem;





        public void SpawnTileMapByMatrix(MapSO map)
        {


            // remove add Child
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }



            int width = map.width;
            int height = map.height;

            string[] value = map.mapData.Split(",");

            int[,] matrixValue = new int[width, height];
            int count = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {

                    matrixValue[i, j] = int.Parse(value[count++].Trim());
                }
            }
            Debug.Log("Count : " + count);
            //Debug.Log("Matrix: " + value.Count() + ", width: " + width + ", height: " + height);

            //matrixValue = ExpandMatrix(matrixValue);
            SpawnMap(matrixValue);
            

            transform.position = map.positionMap;
            transform.rotation = Quaternion.Euler(map.rotationMap);
            transform.localScale = Vector3.one;



        }


        public void ClearMapPositions()
        {
            mapPositions.Clear();
            DestroyImmediate(ParentItem);

            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
            transform.position = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(1f, 1f, 1f);

        }

        public void GetMapPositions()
        {

            levelSpawnData = levelGamePlay.levelSpawnData;
            mapPositions.Clear();
            foreach (var item in levelSpawnData.listItemSpawns)
            {
                foreach (var spawn in item.listSpawnDatas)
                {
                    Vector3 position = spawn.p.ToVector3();
                    mapPositions.Add(position);
                }
            }
            mapPositions.Add(new Vector3(0, 0, 0)); // Add a default position for testing

            if (ParentItem == null)
            {
                ParentItem = new GameObject("ParentItem");
                ParentItem.transform.position = Vector3.zero;

            }
            SpawnItem();

            GetMatrix();
        }

        public void GetMapPositions(MapSO map)
        {

            ClearMapPositions();

            levelSpawnData = levelGamePlay.levelSpawnData;
            mapPositions.Clear();
            foreach (var item in levelSpawnData.listItemSpawns)
            {
                foreach (var spawn in item.listSpawnDatas)
                {
                    Vector3 position = spawn.p.ToVector3();
                    mapPositions.Add(position);
                }
            }
            mapPositions.Add(new Vector3(0, 0, 0)); // Add a default position for testing

            if (ParentItem == null)
            {
                ParentItem = new GameObject("ParentItem");
                ParentItem.transform.position = Vector3.zero;

            }
            //SpawnItem();

            GetMatrix();
            transform.position = map.positionMap;
            transform.rotation = Quaternion.Euler(map.rotationMap);
        }


        public void GetMatrix()
        {
            if (mapPositions == null || mapPositions.Count == 0)
            {
                Debug.LogError("Map positions are empty. Please generate map positions first.");
                return;
            }

            //mapPositions.Add(new Vector3(0, 0, 0)); // Add a default position for testing
            float minX = mapPositions.Min(pos => pos.x);
            float minZ = mapPositions.Min(pos => pos.z);
            float maxX = mapPositions.Max(pos => pos.x);
            float maxZ = mapPositions.Max(pos => pos.z);
            this.minX = minX;
            this.minZ = minZ;


            float distanceX = maxX - minX;
            float distanceZ = maxZ - minZ;

            int width = Mathf.CeilToInt((maxX - minX + 1) / 8f) * 8; // Ensure width is a multiple of 4
            int height = Mathf.CeilToInt((maxZ - minZ + 1) / 8f) * 8; // Ensure height is a multiple of 4

            Debug.Log($"Creating matrix with dimensions: {width}x{height} based on positions from {minX},{minZ} to {maxX},{maxZ}");

            matrix = new int[width, height];
            visited = new bool[width, height];

            // Initialize the matrix with zeros
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    matrix[x, y] = 0;
                }
            }


            foreach (var pos in mapPositions)
            {
                int xIndex = Mathf.CeilToInt(pos.x - minX);
                int zIndex = Mathf.CeilToInt(pos.z - minZ);

                if (xIndex >= 0 && xIndex < width && zIndex >= 0 && zIndex < height)
                {

                    matrix[xIndex, zIndex] = 1;
                    visited[xIndex, zIndex] = true;

                }
            }
            Debug.Log($"Matrix created with dimensions: {width}x{height}");





            // Perform BFS from the first position in the matrix
            for (int x = 0; x < width; x++)
            {

                for (int y = 0; y < height; y++)
                {
                    if (matrix[x, y] == 1)
                    {
                        bool[,] visitedResult = BFS(new Vector2(x, y), matrix);
                        for (int i = 0; i < visitedResult.GetLength(0); i++)
                        {
                            for (int j = 0; j < visitedResult.GetLength(1); j++)
                            {
                                if (visitedResult[i, j])
                                {
                                    matrix[i, j] = 1;

                                }
                            }
                        }

                    }
                }
            }





            // for (int x = 0; x < width; x++)
            // {
            //     matrix[x, 0] = 0; // Set first row to 0
            //     matrix[x, height - 1] = 0; // Set last row to 0
            // }


            // for (int x = 0; x < height; x++)
            // {
            //     matrix[0, x] = 0; // Set first column to 0
            //     matrix[width - 1, x] = 0; // Set last column to 0
            // }


            matrix = ContractMatrix(matrix); // Contract the matrix to reduce size
            matrix = ContractMatrix(matrix); // Contract the matrix again to reduce size

            Debug.Log($"Matrix contracted to dimensions: {matrix.GetLength(0)}x{matrix.GetLength(1)}");
            
            //matrix = ExpandMatrix(matrix); // Expand the matrix to add borders
            width = matrix.GetLength(0);
            height = matrix.GetLength(1);
            for (int i = 0; i < width; i++)
            {

                bool checkStart = false;
                for (int j = 0; j < height; j++)
                {

                    if (matrix[i, j] == 1 && checkStart == false && j % 2 == 0 && j > 1)
                    {
                        // Start of a new path
                        checkStart = true;
                        matrix[i, j - 1] = 1; // Ensure the start of the path is marked as 1
                        // if (j % 2 == 0)
                        //     matrix[i, j - 1] = 1; // Ensure the start of the path is marked as 1
                    }
                    else if (matrix[i, j] == 0 && checkStart == true)
                    {
                        // End of a path
                        checkStart = false;
                        if (j % 2 == 0)
                           matrix[i, j] = 1;

                    }

                }

            }

            for (int i = 0; i < height; i++)
            {
                bool checkStart = false;
                for (int j = 0; j < width; j++)
                {
                    if (matrix[j, i] == 1 && checkStart == false && j % 2 == 0 && j > 1)
                    {
                        // Start of a new path
                        checkStart = true;
                        matrix[j - 1, i] = 1; // Ensure the start of the path is marked as 1
                        // if (j % 2 == 0)
                        //     matrix[j - 1, i] = 1; // Ensure the start of the path is marked as 1
                    }
                    else if (matrix[j, i] == 0 && checkStart == true)
                    {
                        // End of a path
                        checkStart = false;
                        if (j % 2 == 0)
                            matrix[j, i] = 1;

                    }
                }
            }


            




            // Spawn the map based on the matrix
            //matrix = ExpandMatrix(matrix); // Expand the matrix to add borders


            matrix = ExpandMatrixOdd(matrix); // Expand the matrix to add borders
            
            width = matrix.GetLength(0);
            height = matrix.GetLength(1);


                        for (int i = 0; i < width; i++)
            {

                bool checkStart = false;
                for (int j = 0; j < height; j++)
                {

                    if (matrix[i, j] == 1 && checkStart == false && j % 2 == 0 && j > 1)
                    {
                        // Start of a new path
                        checkStart = true;
                        matrix[i, j - 1] = 1; // Ensure the start of the path is marked as 1
                        // if (j % 2 == 0)
                        //     matrix[i, j - 1] = 1; // Ensure the start of the path is marked as 1
                    }
                    else if (matrix[i, j] == 0 && checkStart == true)
                    {
                        // End of a path
                        checkStart = false;
                        if (j % 2 == 0)
                           matrix[i, j] = 1;

                    }

                }

            }

            for (int i = 0; i < height; i++)
            {
                bool checkStart = false;
                for (int j = 0; j < width; j++)
                {
                    if (matrix[j, i] == 1 && checkStart == false && j % 2 == 0 && j > 1)
                    {
                        // Start of a new path
                        checkStart = true;
                        matrix[j - 1, i] = 1; // Ensure the start of the path is marked as 1
                        // if (j % 2 == 0)
                        //     matrix[j - 1, i] = 1; // Ensure the start of the path is marked as 1
                    }
                    else if (matrix[j, i] == 0 && checkStart == true)
                    {
                        // End of a path
                        checkStart = false;
                        if (j % 2 == 0)
                            matrix[j, i] = 1;

                    }
                }
            }
            for (int x = 0; x < width; x++)
            {
                matrix[x, 0] = 0; // Set first row to 0
                matrix[x, height - 1] = 0; // Set last row to 0
            }


            for (int x = 0; x < height; x++)
            {
                matrix[0, x] = 0; // Set first column to 0
                matrix[width - 1, x] = 0; // Set last column to 0
            }

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if (j + 2 < height && matrix[i, j] == 1 && matrix[i, j + 1] == 0 && matrix[i, j + 2] == 0)
                    {
                        matrix[i, j + 1] = 1;
                        j = j + 2; // Skip the next cell to avoid double counting
                    }
                }
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j + 2 < width && matrix[j, i] == 1 && matrix[j + 1, i] == 0 && matrix[j + 2, i] == 0)
                    {
                        matrix[j + 1, i] = 1;
                        j = j + 2; // Skip the next cell to avoid double counting
                    }
                }
            }


            for (int i = 0; i < width; i++)
            {
                int index = 0;
                for (int j = 0; j < height; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        index++;
                    }
                    else
                    {
                        if (index % 2 != 0 )
                        {

                            matrix[i, j] = 1; // Ensure the end of the path is marked as 1 

                        }
                        index = 0;
                    }
                }
            }
            for (int i = 0; i < height; i++)
            {
                int index = 0;
                for (int j = 0; j < width; j++)
                {
                    if (matrix[j, i] == 1)
                    {
                        index++;
                    }
                    else
                    {
                        if (index % 2 != 0 )
                        {

                            matrix[j , i] = 1; // Ensure the end of the path is marked as 1 

                        }
                        index = 0;
                    }
                }
            }

            // Debug.Log("Matrix dimensions: " + width + "x" + height);
            // Spawn the map based on the matrix
            for (int i = 0; i < width; i++)
            {
                string row = "";
                for (int j = 0; j < height; j++)
                {
                    row += matrix[i, j] + " ";
                }
                Debug.Log(row);
            }
            matrix = ExpandMatrix(matrix);
            

            SpawnMap(matrix);

            this.transform.localScale = new Vector3(1f, 1f, 1f); // Adjust the scale to fit the map

            //this.transform.localScale = new Vector3(1/ 128f, 1/128f, 1/ 128f) ; 
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.transform.position = new Vector3(width*4f + 64f, 0, height*-2f - 64f);

        }

        public int[,] ContractMatrix(int[,] original)
        {
            int originalRows = original.GetLength(0);
            int originalCols = original.GetLength(1);
            int newRows = originalRows / 2;
            int newCols = originalCols  / 2;

            // Create new matrix with contracted dimensions
            int[,] contracted = new int[newRows, newCols];

            int index = 0;
            // Copy original data to the new matrix
            for(int i = 0; i < originalRows; i += 2)
            {
                for (int j = 0; j < originalCols; j += 2)
                {
                    // Calculate the average of the 2x2 block
                    int sum = original[i, j] + original[i + 1, j] + original[i, j + 1] + original[i + 1, j + 1];
                    contracted[index / newCols, index % newCols] = sum >= 1 ? 1 : 0;
                    index++;
                }
            }

            return contracted;
        }


        public int[,] ExpandMatrixOdd(int[,] original)
        {
            int originalRows = original.GetLength(0);
            int originalCols = original.GetLength(1);
            if ( originalRows % 2 == 0 && originalCols % 2 == 0 && originalRows > 2 && originalCols > 2)
            {
                //Debug.LogError("Matrix dimensions must be even for expansion.");
                return original;
            }

            int targetRows = Mathf.CeilToInt(originalRows / 2f) * 2;
            int targetCols = Mathf.CeilToInt(originalCols / 2f) * 2;

            if (targetRows == 2)
            {
                targetRows = 4; // Ensure at least 4 rows
            }
            if (targetCols == 2)
            {
                targetCols = 4; // Ensure at least 4 columns
            }
            int[,] expanded = new int[targetRows, targetCols];
            // Initialize all cells to 0    
            for (int i = 0; i < targetRows; i++)
            {
                for (int j = 0; j < targetCols; j++)
                {
                    expanded[i, j] = 0;
                }
            }
            for (int i = 0; i < originalRows; i++)
            {
                for (int j = 0; j < originalCols; j++)
                {
                    expanded[i + targetRows - originalRows, j + targetCols - targetCols] = original[i, j];
                }
            }
            return expanded;
            
        }


        public int[,] ExpandMatrix(int[,] original)
        {
            int originalRows = original.GetLength(0);
            int originalCols = original.GetLength(1);
            int newRows = originalRows + 16;
            int newCols = originalCols + 16;

            // Create new matrix with expanded dimensions
            int[,] expanded = new int[newRows, newCols];

            // Initialize all cells to 1
            for (int i = 0; i < newRows; i++)
            {
                for (int j = 0; j < newCols; j++)
                {
                    expanded[i, j] = 0;
                }
            }

            // Copy original data to the new matrix
            for (int i = 0; i < originalRows; i++)
            {
                for (int j = 0; j < originalCols; j++)
                {
                    expanded[i + 8, j + 8] = original[i, j];
                }
            }

            return expanded;
        }


        private int[] GetRow(int[,] matrix, int row)
        {
            int columns = matrix.GetLength(1);
            int[] rowArray = new int[columns];
            for (int i = 0; i < columns; i++)
            {
                rowArray[i] = matrix[row, i];
            }
            return rowArray;
        }

        public int countConsecutive(int startindex, int[] array)
        {
            int count = 1;
            for (int i = startindex + 1; i < array.Length; i++)
            {
                if (array[i] == 1)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }


        public bool[,] BFS(Vector2 position, int[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);


            Queue<Vector2> queue = new Queue<Vector2>();
            queue.Enqueue(position);

            bool[,] visited = new bool[width, height];
            bool[,] result = new bool[width, height];

            // Mark the starting position as visited
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    visited[x, y] = false;
                    result[x, y] = false;
                }
            }

            visited[(int)position.x, (int)position.y] = true;

            int[] directionX = new int[] { 0, 1, 0, -1 };
            int[] directionY = new int[] { 1, 0, -1, 0 };
            // Perform BFS to explore the matrix
            while (queue.Count > 0)
            {
                Vector2 current = queue.Dequeue();
                result[(int)current.x, (int)current.y] = true;


                if (matrix[(int)current.x, (int)current.y] == 1 && current != position)
                {

                    return result; // Exit if we reach a position with value 1 that hasn't been visited
                }



                for (int i = 0; i < 4; i++)
                {
                    int newX = (int)current.x + directionX[i];
                    int newY = (int)current.y + directionY[i];

                    if (newX >= 0 && newX < width && newY >= 0 && newY < height && !visited[newX, newY])
                    {
                        visited[newX, newY] = true;
                        queue.Enqueue(new Vector2(newX, newY));
                    }
                }
            }
            return result;
        }

        public void SpawnMap(int[,] matrixValues)
        {


            int width = matrixValues.GetLength(0);
            int height = matrixValues.GetLength(1);

            Debug.Log("width: " + width+ ", height: " + height);

            
            Dictionary<int, int> valueMaps = new Dictionary<int, int>();
            valueMaps.Add(0, 2);
            valueMaps.Add(1, 1);
        

            for (int x = width - 1; x > 0; x -= 2)
            {
                for (int y = height - 1; y > 0; y -= 2)
                {
                    foreach (var item in ruleTiles)
                    {
                       
                        if (valueMaps[matrixValues[x, y]] == item.topleft && valueMaps[matrixValues[x - 1, y]] == item.topRight && valueMaps[matrixValues[x, y - 1]] == item.bottomLeft && valueMaps[matrixValues[x - 1, y - 1]] == item.bottomRight)
                        {
                            //Debug.Log($"Spawning tile at {x}, {y} with prefab {item.prefab.name}");   
                            GameObject prefab = item.prefab;
                            if (prefab != null)
                            {
                                Vector3 position = new Vector3(-x * 8f, 0, y * 8f);
                                GameObject spawnedObject = Instantiate(prefab, transform);
                                spawnedObject.transform.localPosition = position;

                                spawnedObject.transform.localScale = new Vector3(1f, 1f, 1f);
                                spawnedObject.name = $"Tile_{x}_{y}";
                            }
                            else
                            {
                                Debug.LogWarning($"Prefab for RuleTile not assigned: {item}");
                            }
                        }
                    }

                }
            }


            // for (int x = 0; x < width; x += 2)
            // {
            //     for (int y = 0; y < height; y += 2)
            //     {
            //         foreach (var item in ruleTiles)
            //         {
            //             if (valueMaps[matrixValues[x, y]] == item.topleft && valueMaps[matrixValues[x + 1, y]] == item.topRight && valueMaps[matrixValues[x, y + 1]] == item.bottomLeft && valueMaps[matrixValues[x + 1, y + 1]] == item.bottomRight)
            //             {
            //                 GameObject prefab = item.prefab;
            //                 if (prefab != null)
            //                 {
            //                     Vector3 position = new Vector3(x * 8f, 0, -y * 8f);
            //                     GameObject spawnedObject = Instantiate(prefab, transform);
            //                     spawnedObject.transform.localPosition = position;

            //                     spawnedObject.transform.localScale = new Vector3(1f, 1f, 1f);
            //                     spawnedObject.name = $"Tile_{x}_{y}";
            //                 }
            //                 else
            //                 {
            //                     Debug.LogWarning($"Prefab for RuleTile not assigned: {item}");
            //                 }
            //             }
            //         }

            //     }
            // }
        }

        
        public void SaveData()
        {
            MapSO mapSO = ScriptableObject.CreateInstance<MapSO>();
            mapSO.width = matrix.GetLength(0);
            mapSO.height = matrix.GetLength(1);
            mapSO.mapData = string.Join(",", matrix.Cast<int>());
            mapSO.positionMap = this.transform.position - new Vector3(0, 0.5f, 0);
            mapSO.rotationMap = this.transform.rotation.eulerAngles;
            string name = levelGamePlay.name.Substring(11);
            string path = $"Assets/_Data/DataItemMap/TileMap/TileMap_{name}.asset";

            if (File.Exists(path))
            {
                //AssetDatabase.DeleteAsset(path);
            }
            #if UNITY_EDITOR
            // AssetDatabase.CreateAsset(mapSO, path);
            // AssetDatabase.SaveAssets();

            #endif
            // Finding nextLevel Data 
            ClearMapPositions();
            string nextName = (int.Parse(name) + 1).ToString();
            //LevelGamePlaySO nextLevelData = AssetDatabase.LoadAssetAtPath<LevelGamePlaySO>($"Assets/Resources/DataLevelNewFixSO/Data_Level_{nextName}.asset");
            // if (nextLevelData != null)
            // {
            //     this.levelGamePlay = nextLevelData;
            //     GetMapPositions();

            // }
            // if (nextLevelData == null)
            // {
            //     Debug.LogWarning($"Next level data not found: {nextName}");
            // }
        }



        private void SpawnItem()
        {
            foreach (var item in levelSpawnData.listItemSpawns)
            {
                foreach (var spawn in item.listSpawnDatas)
                {
                    GameObject prefab = LoadPrefab(item.id, "PrefabInstance/GameObject/");
                    if (prefab != null)
                    {
                        Vector3 position = spawn.p.ToVector3();
                        GameObject spawnedObject = Instantiate(prefab, ParentItem.transform);
                        spawnedObject.transform.localPosition = position;
                        spawnedObject.transform.localRotation = Quaternion.Euler(spawn.r.ToVector3());

                    }
                    else
                    {
                        Debug.LogWarning($"Prefab not found: {item.id}");
                    }
                }
            }
        }

        public GameObject LoadPrefab(string prefabName, string searchPath = "")
        {
            // Construct the full path


            // Try to load the prefab
            string fullPath = searchPath + prefabName;
            GameObject prefab = Resources.Load<GameObject>(fullPath);

            if (prefab != null)
            {
                return prefab;
            }

            // Find Assets in the all Subforlder



            Debug.LogWarning($"Prefab '{prefabName}' not found in Resources");
            return null;
        }


    }

}





#if UNITY_EDITOR


[CustomEditor(typeof(TestGenerateMap))]
public class TestGenerateMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestGenerateMap script = (TestGenerateMap)target;
        if (GUILayout.Button("Create Matrix"))
        {
            script.GetMapPositions();
            // You can add additional logic here to display or process the matrix as needed
        }
        if (GUILayout.Button("Clear Map Positions"))
        {
            script.ClearMapPositions();
        }
        if (GUILayout.Button("Get Matrix"))
        {

            script.GetMatrix();


        }
        if (GUILayout.Button("Save Data"))
        {
            script.SaveData();
        }

        // Optionally, display the matrix in the inspector


        if (script.visited != null && script.visited.GetLength(0) > 0 && script.visited.GetLength(1) > 0)
        {
            EditorGUILayout.LabelField("Visited Matrix:");
            int width = script.visited.GetLength(0);
            int height = script.visited.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                string row = "";
                for (int y = 0; y < height; y++)
                {
                    row += (script.visited[x, y] ? "1" : "0") + " ";
                }
                EditorGUILayout.LabelField(row);
            }
        }

    }

}
       
  
#endif