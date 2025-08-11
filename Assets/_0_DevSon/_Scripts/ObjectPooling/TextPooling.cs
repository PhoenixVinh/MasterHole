using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.ObjectPooling




{


    public class TextPooling : ObjectPoolingBase<TextPooling>
    {


        private float timeLimit = 0.5f;


        private int request = 0;
        
        public override void Awake()
        {
            base.Awake();
        }
        public void SpawnText(string text, Vector3 position, Quaternion rotation)
        {
            
        }


        public void SpawnText(Vector3 position, int score)
        {

            request++;
            if (request <= 10)
            {
               SpawnTextPool(position, score);
            }
            else
            {
                if ((request - 10) % 5 == 0)
                {
                    SpawnTextPool(position, score);
                }
            }
            

        }

        public void SpawnTextPool(Vector3 position, int score)
        {
            Vector3 RandomPos = new Vector3(position.x + Random.Range(-1f,1f), position.y+2f, position.z + Random.Range(-1f,1f));
            GameObject textspawn = this.GetPooledObject();
            textspawn.GetComponent<Score>().SetData(5f,1f,score);
            textspawn.transform.position = RandomPos;
            textspawn.SetActive(true);
        }
        
        
        public void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0)
            {
                request = 0;
                timeLimit = 0.5f;

            }
            
        }
        
       
     
        
        
        
        
        
        
    }
}