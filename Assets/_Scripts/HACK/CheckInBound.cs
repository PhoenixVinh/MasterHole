using UnityEngine;

namespace _Scripts.HACK
{
    public class CheckInBound : MonoBehaviour
    {
        public Collider colliderCheck;


        [ContextMenu("Check In Bound")]
        public void CheckInItemBound()
        {
           
                Vector3 targetPositionXZ = new Vector3(transform.position.x, 0f, transform.position.z);
                Vector3 closestXZ = colliderCheck.ClosestPoint(targetPositionXZ);

                // Gán lại giá trị Y của targetPoint cho điểm gần nhất
                //Vector3 closestWithTargetY = new Vector3(closestXZ.x, transform.position.y, closestXZ.z);

                

                float distanceXZ = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(closestXZ.x, closestXZ.z));
                Debug.Log($"Khoảng cách XZ gần nhất: {distanceXZ}");
            
     
           
        }
    }
}