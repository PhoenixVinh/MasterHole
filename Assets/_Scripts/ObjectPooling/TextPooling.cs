using UnityEngine;

namespace _Scripts.ObjectPooling
{
    public class TextPooling : ObjectPoolingBase<TextPooling>
    {

        public override void Awake()
        {
            base.Awake();
        }
        public void SpawnText(string text, Vector3 position, Quaternion rotation)
        {
            
        }


        public void SpawnText(Vector3 position, int score)
        {
            Vector3 RandomPos = new Vector3(position.x + Random.Range(-1f,1f), position.y+2f, position.z + Random.Range(-1f,1f));
            GameObject textspawn = this.GetPooledObject();
            textspawn.GetComponent<Score>().SetData(5f,1f,score);
            textspawn.transform.position = RandomPos;
            textspawn.SetActive(true);

        }
        
        
        
        
        
        
        
    }
}