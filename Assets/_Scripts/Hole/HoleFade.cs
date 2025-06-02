using System;
using UnityEngine;

namespace _Scripts.Hole
{
    public class HoleFade : MonoBehaviour
    {
        public void OnEnable()
        {
            HoleEvent.OnUpdateFade += UpdateScale;
        }

        public void OnDisable()
        {
            HoleEvent.OnUpdateFade -= UpdateScale;
        }

        private void UpdateScale(float scale)
        {
            this.transform.localScale = new Vector3(1, 1, scale);
        }
    }
}