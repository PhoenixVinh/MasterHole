using System;
using UnityEngine;

namespace _Scripts.Test
{
    public class CheckItemInBound : MonoBehaviour
    {
        private Collider _collider;

        public Transform _targetTransfrom;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }
        
        [ContextMenu("Check ITem In Bound")]
        public void CheckTargetInBound()
        {
            var result =  _collider.bounds.Contains(new Vector3(_targetTransfrom.position.x, 0.1f, _targetTransfrom.position.z));
            Debug.Log(result);
        }
        
        
        
    }
}