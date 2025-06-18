using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Hole
{
    public class CleanGradeManager : MonoBehaviour
    {
        private Queue<GameObject> objectsToDestroy = new Queue<GameObject>();
        
        private int destroyBatchSize = 10;   //


        public static CleanGradeManager Instance;
        private float timeDelay = 1f;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        public void FixedUpdate()
        {
            if (objectsToDestroy.Count > 0 && timeDelay >= 1)
            {
               
                int destroyCount = Mathf.Min(destroyBatchSize, objectsToDestroy.Count);
                for (int i = 0; i < destroyCount; i++)
                {
                    if (objectsToDestroy.Count > 0)
                    {
                        GameObject obj = objectsToDestroy.Dequeue();
                        Destroy(obj);
                        timeDelay = 0;
                    }
                }
            
            }
            else
            {
                timeDelay += Time.fixedDeltaTime;
            }
        }

        public void AddObject(GameObject obj)
        {
            obj.SetActive(false);
            objectsToDestroy.Enqueue(obj);
            
        }

        public void CleanQueue()
        {
            objectsToDestroy.Clear();
        }
    }
}