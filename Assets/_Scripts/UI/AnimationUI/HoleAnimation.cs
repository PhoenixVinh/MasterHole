using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets._Scripts.UI.AnimationUI
{
    public class HoleAnimation : MonoBehaviour
    {
        public List<Transform> fruits;
        public Transform target;

        public float moveDuration = 1.5f;
        public AnimationCurve moveCurve;

        public Transform Hole;

        public Transform target02;
        public float moveDuration02 = 1.5f;

        public float circleRadius = 2f;
        public float circleDuration = 1.5f;

        // Store initial positions and scales for reset
        private readonly List<Vector3> initialFruitPositions = new();
        private readonly List<Vector3> initialFruitScales = new();
        private Vector3 initialHoleScale;

        private void Awake()
        {
            CacheInitialTransforms();
        }

        private void CacheInitialTransforms()
        {
            initialFruitPositions.Clear();
            initialFruitScales.Clear();
            foreach (var fruit in fruits)
            {
                initialFruitPositions.Add(fruit.position);
                initialFruitScales.Add(fruit.localScale);
            }
            if (Hole != null)
                initialHoleScale = Hole.localScale;
        }

        public void ResetFruitsAndHole()
        {
            for (int i = 0; i < fruits.Count; i++)
            {
                fruits[i].position = initialFruitPositions[i];
                fruits[i].localScale = initialFruitScales[i];
                DOTween.Kill(fruits[i]);
            }
            if (Hole != null)
            {
                Hole.localScale = initialHoleScale;
                DOTween.Kill(Hole);
            }
        }

        public void MoveFruitsInCircleThenToTarget()
        {
            ResetFruitsAndHole();
            if (Hole != null)
            {
                Hole.DOScale(new Vector3(10f, 0.04f,10f), 0.5f)
                    .SetEase(Ease.InOutQuad)
                    .SetUpdate(true);
            }
            // Animate fruits in a circle around MainHole (Hole), with radius = distance from MainHole to each fruit
            Sequence circleSequence = DOTween.Sequence().SetUpdate(true);

            Vector3 center = Hole != null ? Hole.position : Vector3.zero;
            int numCircles = 3;
            float circleTime = circleDuration;

            for (int i = 0; i < fruits.Count; i++)
            {
                Transform fruit = fruits[i];
                float radius = Vector3.Distance(new Vector3(center.x, 0, center.z), new Vector3(fruit.position.x, 0, fruit.position.z));
                float startAngle = Mathf.Atan2(fruit.position.z - center.z, fruit.position.x - center.x);
                float originalHeight = fruit.position.y;

                // Animate along a circular path for 3 full circles
                Tween tween = DOTween.To(
                    () => 0f,
                    t =>
                    {
                        float angle = startAngle + t * 2 * Mathf.PI * numCircles;
                        Vector3 pos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                        pos.y = originalHeight;
                        fruit.position = pos;
                    },
                    0.5f,
                    circleTime
                ).SetEase(Ease.InOutSine).SetUpdate(true);

                circleSequence.Join(tween);
            }

            // After circle animation, move fruits to target
            circleSequence.OnComplete(() =>
            {
                MoveFruitsToTarget();
            });
        }

        public void MoveFruitsToTarget()
        {
            // Null checks to prevent errors
            if (fruits == null || fruits.Count == 0)
            {
                Debug.LogWarning("Fruits list is empty or null.");
                return;
            }
            if (target == null)
            {
                Debug.LogWarning("Target transform is not assigned.");
                return;
            }
            if (target02 == null)
            {
                Debug.LogWarning("Target02 transform is not assigned.");
                return;
            }

            // Create a sequence to track all fruit animations
            Sequence fruitSequence = DOTween.Sequence().SetUpdate(UpdateType.Normal, true);

            float delay = 0;
            foreach (var fruit in fruits)
            {
                if (fruit == null) continue;

                Vector3 startPos = fruit.position;
               
                Vector3 endPos = target.position;
                Vector3 midPoint = (startPos + endPos) / 2 + Vector3.up * 2f; // Curve upwards

                fruit.DOComplete(); // Ensure no conflicting tweens

                var tween = fruit.DOPath(
                        new Vector3[] { startPos, midPoint, endPos },
                        moveDuration,
                        PathType.CatmullRom
                    )
                    .SetLookAt(0.01f) // Prevents unwanted rotation
                    .SetUpdate(true)
                    .SetDelay(delay);
                
                

                float initialScale = 1f;
                float targetScale = 0.5f;
                tween.OnUpdate(() =>
                {
                    float progress = tween.ElapsedPercentage();
                    fruit.localScale = Vector3.one * Mathf.Lerp(initialScale, targetScale, progress);
                });

                if (moveCurve != null)
                {
                    tween.SetEase(moveCurve);
                }
                else
                {
                    tween.SetEase(Ease.InOutQuad);
                }

                tween.OnComplete(() =>
                {
                    fruit.localScale = Vector3.one * targetScale;
                });

                fruitSequence.Join(tween);
                delay += 0.03f;
            }

            // After all fruits complete their animation, move them to target02
            fruitSequence.OnComplete(() =>
            {
                foreach (var fruit in fruits)
                {
                    if (fruit == null) continue;
                    fruit.DOMove(target02.position, moveDuration02).SetEase(Ease.InOutQuad).SetUpdate( true);
                }
            });
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(HoleAnimation))]
        public class HoleAnimationEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                HoleAnimation script = (HoleAnimation)target;
                if (GUILayout.Button("Move Fruits To Target"))
                {
                    script.MoveFruitsInCircleThenToTarget();
                }
               
            }
        }
#endif
    }
}