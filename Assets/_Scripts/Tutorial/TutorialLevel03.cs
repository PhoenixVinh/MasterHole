using System;
using _Scripts.Booster;
using _Scripts.UI.BoosterUI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.Tutorial
{
    public class TutorialLevel03 : MonoBehaviour
    {
        public GameObject Message;
        public  Button Booster;
        public BoosterUI boosterUI;

        public void OnEnable()
        {
            Message.transform.localScale = Vector3.zero;
            Message.transform.DOScale(Vector3.one, 0.5f);
            Booster.onClick.AddListener(
                FreeBooster);
        }

        public virtual void FreeBooster()
        {
            ManagerBooster.Instance.ChangeAmountBooster(0,1);
            this.gameObject.SetActive(false);
            boosterUI.UseSpecialSkill();
            
        }


        public void OnDisable()
        {
            DOTween.KillAll();
        }
    }
}