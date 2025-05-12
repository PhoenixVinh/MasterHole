using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.HACK
{
    public class MoveParabolUI : MonoBehaviour
    {
       
        [SerializeField] private GameObject target; // RectTransform của target (kéo thả từ UI)
        [SerializeField] private float height = 100f; // Độ cao đỉnh parabol
        [SerializeField] private float moveDuration = 2f; // Thời gian di chuyển
        [SerializeField] private AnimationCurve heightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f); // Đường 
        private Vector3 startPoint; // Tọa độ bắt đầu
        private Vector3 endPoint; // Tọa độ kết thúc (lấy từ target)
        private Vector3 controlPoint; //



        public Vector3 ADDPoint;
        void Start()
        {
            // Khởi tạo RectTransform của chính GameObject
           
            
            // Lưu vị trí bắt đầu ban đầu
            startPoint = this.transform.position;
            endPoint = target.transform.position;
            controlPoint = (startPoint + endPoint) / 2f + Vector3.up * height;
            Debug.Log(startPoint);
            Debug.Log(endPoint);
          
        }

        [ContextMenu("Move")]
        public void Move()
        {
            StartCoroutine(MoveWithCurve());
        }
        
        
        private IEnumerator MoveWithCurve()
        {
            Debug.Log("Starting AnimationCurve movement...");
            float currentTime = 0f;

            while (currentTime < moveDuration)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / moveDuration); // Tỷ lệ thời gian (0 -> 1)

                // Tính toán vị trí theo AnimationCurve
                Vector3 newPosition = CalculateCurvePosition(t);
                transform.position = newPosition;

                yield return null; // Chờ frame tiếp theo
            }

            // Đảm bảo vị trí cuối cùng chính xác
            transform.position = endPoint;
            Debug.Log("AnimationCurve movement completed!");
        }

        private Vector3 CalculateCurvePosition(float t)
        {
            
            
            float y = Mathf.Lerp(startPoint.y, endPoint.y, t);
            // Nội suy tuyến tính cho x và z
            //float x = Mathf.Lerp(startPoint.x, endPoint.x, t);

            // Tính y dựa trên AnimationCurve
            float curveValue = heightCurve.Evaluate(t); // Giá trị từ 0 đến 1
            float x = Mathf.Lerp(startPoint.x, endPoint.x, t) + curveValue * height;

            return new Vector3(x, y, 0);
        }

        [ContextMenu("TEST ADD POINT")]
        public void AddValue()
        {
            this.transform.position += ADDPoint;
        }
    }
}

