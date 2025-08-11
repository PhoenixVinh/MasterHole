using System;
using _Scripts.ObjectPooling;

using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.SpecialSkillUI
{
    public class DirectionItemUI : MonoBehaviour
    {
        public Transform mainHole;
        
        
        public GameObject arrow;
        public GameObject hightlight;
        private void FixedUpdate()
        {
            SpawnObjectTracking();
        }


        public void OnDestroy()
        {
            
            hightlight.SetActive(false);
            arrow.SetActive(false);
            
        }


        private void Awake()
        {
            arrow = DirectionItemUIPooling.Instance.GetPooledObject();
            hightlight = HightLightItemUIPooling.Instance.GetPooledObject();
            arrow.SetActive(false);
            hightlight.SetActive(false);
        }



        public void SetData(Transform mainHole, Sprite sprite)
        {
            this.mainHole = mainHole;
            arrow.SetActive(true);
            arrow.GetComponent<DirectionItem>().SetContent(sprite);
            
            Vector3 screenPoint = UnityEngine.Camera.main.WorldToViewportPoint(this.transform.position);
            Vector3 display = new Vector3(
                Mathf.Clamp(screenPoint.x, 0.1f, 0.9f),
                Mathf.Clamp(screenPoint.y, 0.2f, 0.9f),
                0);
           
            
        }


        public bool CheckOutScreen()
        {
            
            Vector3 screenpoint = UnityEngine.Camera.main.WorldToScreenPoint(this.transform.position);
            
            var height = UnityEngine.Camera.main.pixelHeight;
            var width = UnityEngine.Camera.main.pixelWidth;


            if (screenpoint.x < 0 || screenpoint.x > width || screenpoint.y > height || screenpoint.y < 0)
            {
                Debug.Log("Screen point is out of range");
                return true;
            }
            else
            {
                Debug.Log("Screen point is in range");
                return false;
            }
        }

        private void OnDisable()
        {
            this.arrow.SetActive(false);
            this.hightlight.SetActive(false);
        }


        [ContextMenu("Show Arrow")]
        public void SpawnObjectTracking()
        {
            var height = UnityEngine.Camera.main.pixelHeight;
            var width = UnityEngine.Camera.main.pixelWidth;
            Vector3 screenPoint = UnityEngine.Camera.main.WorldToViewportPoint(this.transform.position);
            
            if(screenPoint.x < 0 || screenPoint.x >= 1 || screenPoint.y < 0 || screenPoint.y >= 1)
            {
                arrow.gameObject.SetActive(true);
                hightlight.SetActive(false);
                Vector3 direction = transform.position - HoleController.Instance.transform.position;
                direction.Normalize();
                float Angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                
                Vector3 display = new Vector3(
                    Mathf.Clamp(screenPoint.x, 0.1f, 0.9f),
                    Mathf.Clamp(screenPoint.y, 0.2f, 0.9f),
                    0);
                arrow.gameObject.transform.position = new Vector3(display.x*width, display.y*height, 0);
               
                arrow.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angle));
               
                
            }
            else
            {
                arrow.gameObject.SetActive(false);
                hightlight.transform.position = UnityEngine.Camera.main.WorldToScreenPoint(this.transform.position);
                hightlight.SetActive(true);
                
                float distance = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(this.mainHole.position.x, this.mainHole.position.z));
            
                if (distance <= 2f)
                {
                    hightlight.SetActive(false);
                }
            }
            
        }
        
        
    }
}