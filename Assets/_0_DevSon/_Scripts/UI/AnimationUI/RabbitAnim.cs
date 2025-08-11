using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace _Scripts.UI.AnimationUI
{
    public class RabbitAnim : MonoBehaviour
    {
        public SkeletonGraphic skeletonGraphic;

        public void OnEnable()
        {
            skeletonGraphic.AnimationState.SetAnimation(0, StringAnimation.START, false).Complete += ChangeAnimation;
           
        }

        private void ChangeAnimation(TrackEntry trackentry)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, StringAnimation.IDLE, true);
        }
    }
}