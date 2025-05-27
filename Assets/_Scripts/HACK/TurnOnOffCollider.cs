using UnityEngine;

namespace _Scripts.HACK
{
    public class TurnOnOffCollider : MonoBehaviour
    {
        public bool isOn = false;
        public MeshCollider meshCollider;

        public void ChangeStatusCollider()
        {
            isOn = !isOn;
            meshCollider.convex = isOn;
        }
    }
}