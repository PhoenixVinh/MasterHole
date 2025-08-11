using System;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;

using UnityEngine;

namespace _Scripts.UI.AnimationUI
{
    public class TextFloating : MonoBehaviour
    {

        public float addingPosition;
        [SerializeField] private TMP_Text text;
        public void OnEnable()
        {
            Anim();
        }


        public async void Anim()
        {
            
            Vector3 positonStart = transform.position;
            text.alpha = 1;
            
            DOTween.Sequence()
                .SetUpdate(true)
                .SetId("TextFloating")
                .Append(transform.DOMove(new Vector3(positonStart.x, positonStart.y + addingPosition, positonStart.z),
                    .7f))
                .Join(text.DOFade(0, 0.7f));
            await Task.Delay(1400);
            this.gameObject.SetActive(false);

        }
        
        
        public void OnDisable()
        {
            DOTween.KillAll();
        }
    }
}