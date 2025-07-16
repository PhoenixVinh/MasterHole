using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map.TestGenerateMap;
using System;
using System.Linq;
using Unity.VisualScripting;

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

        public LevelSpawnData levelSpawnData;


        public List<Vector3> mapPositions;

        public List<RuleTile> ruleTiles;
        public int[,] matrix;

        public bool[,] visited;

        public float minX;
        public float minZ;





        public void ClearMapPositions()
        {
            mapPositions.Clear();


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
            GetMatrix();
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

            int width = Mathf.CeilToInt((maxX - minX) / 4f) * 4; // Ensure width is a multiple of 4
            int height = Mathf.CeilToInt((maxZ - minZ) / 4f) * 4; // Ensure height is a multiple of 4

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


            for (int i = 0; i < width; i++)
            {

                bool checkStart = false;
                for (int j = 0; j < height; j++)
                {

                    if (matrix[i, j] == 1 && checkStart == false)
                    {
                        // Start of a new path
                        checkStart = true;
                        if (j % 2 == 0)
                            matrix[i, j - 1] = 1; // Ensure the start of the path is marked as 1
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
                    if (matrix[j, i] == 1 && checkStart == false)
                    {
                        // Start of a new path
                        checkStart = true;
                        if (j % 2 == 0)
                            matrix[j - 1, i] = 1; // Ensure the start of the path is marked as 1
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


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (matrix[j, i] == 1)
                    {
                        int count = 1;
                        int index = j;
                        while (index < width && matrix[index, i] == 1)
                        {
                            count++;
                            index++;
                        }
                        if (count % 2 != 0)
                        {
                            matrix[j, i] = 1; // If the count is odd, set the first tile to 0
                        }
                        j = index; // Move the index to the end of the current path


                    }
                }



            }




            // Spawn the map based on the matrix
            matrix = ExpandMatrix(matrix); // Expand the matrix to add borders
            SpawnMap(matrix);

            this.transform.localScale = new Vector3(1f, 1f, 1f) * 0.18f; // Adjust the scale to fit the map

            //this.transform.localScale = new Vector3(1/ 128f, 1/128f, 1/ 128f) ; 
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
            this.transform.position = new Vector3(width , 0, height / -1.5f);
            
        }
            
        


        public  int[,] ExpandMatrix(int[,] original)
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

        public void SpawnMap(int[,] matrix)
        {
            if (matrix == null || matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0)
            {
                Debug.LogError("Matrix is empty or not initialized.");
                return;
            }

            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);



            Dictionary<int, int > valueMaps = new Dictionary<int, int>();
            valueMaps.Add(0, 2);
            valueMaps.Add(1, 1);
            for (int x = 0; x < width; x += 2)
            {
                for (int y = 0; y < height; y += 2)
                {
                    foreach (var item in ruleTiles)
                    {
                        if (valueMaps[matrix[x, y]] == item.topleft && valueMaps[matrix[x + 1, y]] == item.topRight && valueMaps[matrix[x, y + 1]] == item.bottomLeft && valueMaps[matrix[x + 1, y + 1]] == item.bottomRight)
                        {
                            GameObject prefab = item.prefab;
                            if (prefab != null)
                            {
                                Vector3 position = new Vector3(x * 8f, 0, -y * 8f);
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
                if (GUILayout.Button("Get Matrix"))
                {
                    script.GetMatrix();


                }
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

        // Draw squares at map positions in the Scene view
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawMapPositionsGizmo(TestGenerateMap script, GizmoType gizmoType)
        {
            if (script.mapPositions == null || script.mapPositions.Count == 0)
                return;


            if (script.matrix == null || script.matrix.GetLength(0) == 0 || script.matrix.GetLength(1) == 0)
            {
                return;
            }
            Gizmos.color = Color.red;
            int width = script.matrix.GetLength(0);
            int height = script.matrix.GetLength(1);


            Vector3 offset = new Vector3(script.minX - 1.5f, 0, script.minZ - 1.5f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 start = new Vector3(x, 0, y) + offset;
                    Vector3 end = new Vector3(x + 1, 0, y) + offset;
                    Gizmos.DrawLine(start, end);

                    Vector3 start2 = new Vector3(x, 0, y) + offset;
                    Vector3 end2 = new Vector3(x, 0, y + 1) + offset;
                    Gizmos.DrawLine(start2, end2);
                }
            }
            // Draw the last row and column lines to complete the grid
            for (int x = 0; x < width; x++)
            {
                Vector3 start = new Vector3(x, 0, height) + offset;
                Vector3 end = new Vector3(x + 1, 0, height) + offset;
                Gizmos.DrawLine(start, end);
            }
            for (int y = 0; y < height; y++)
            {
                Vector3 start = new Vector3(width, 0, y) + offset;
                Vector3 end = new Vector3(width, 0, y + 1) + offset;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
#endif