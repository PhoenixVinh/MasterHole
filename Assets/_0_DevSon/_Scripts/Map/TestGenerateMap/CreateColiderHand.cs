using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateColiderHand : MonoBehaviour
{
    public enum ColliderType
    {
        Torus,
    }

    public ColliderType colliderType;

    public float radius = 3f;   // bán kính ngoài
    public float tubeRadius = 0.5f; // bán kính ống donut
    public int segments = 12;   // số capsule quanh vòng

    public void CreateCapsule()
    {
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            GameObject capObj = new GameObject("DonutSegment_" + i);
            capObj.transform.SetParent(transform);
            capObj.transform.localPosition = pos;
            capObj.transform.LookAt(transform.position);
            capObj.transform.localRotation *= Quaternion.Euler(90, 0, 0);

            CapsuleCollider cap = capObj.AddComponent<CapsuleCollider>();
            cap.radius = tubeRadius;
            cap.height = radius * 2f * Mathf.Sin(Mathf.PI / segments);
            cap.direction = 2; // z-axis
        }
    }
    

}
