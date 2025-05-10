
using System.Collections.Generic;

using UnityEngine;


namespace _Scripts.Effects
{
    public class HoleScaleEffect : MonoBehaviour
    {
        public List<Transform> ParticleElement;


        private ParticleSystem mainPS;

        private void OnEnable()
        {
            mainPS = GetComponent<ParticleSystem>();
        }

        public async void PlayEffect(float size)
        {
            mainPS.Stop(true);
            foreach (var particle in ParticleElement)
            {
                particle.localScale = Vector3.one * size;
            }
            mainPS.Play();
            await System.Threading.Tasks.Task.Delay(400);
            this.gameObject.SetActive(false);
        }
    }
}