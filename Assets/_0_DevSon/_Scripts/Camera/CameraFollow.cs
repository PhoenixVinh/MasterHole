
using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Nhân vật mà camera sẽ theo
    public float smoothSpeed = 0.125f; // Tốc độ mượt của camera (0.0f - 1.0f)
    public Vector3 offset; // Khoảng cách ban đầu giữa camera và nhân vật
    public float cameraDistance = 5f; // Khoảng cách camera đến nhân vật
    public float minDistance = 2f; // Khoảng cách gần nhất của camera
    public float maxDistance = 10f; // Khoảng cách xa nhất của camera

    private Vector3 desiredPosition;

    void Start()
    {
        // Khởi tạo offset ban đầu
        offset = transform.position - target.position;
        // Đảm bảo cameraDistance nằm trong giới hạn
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);
    }

    void Update()
    {
        // Đảm bảo cameraDistance luôn trong khoảng min-max
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);

        // Tính toán vị trí mong muốn của camera
        desiredPosition = target.position + offset.normalized * cameraDistance;

        // Di chuyển camera mượt mà tới vị trí mong muốn
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Camera luôn hướng về nhân vật
        transform.LookAt(target);
    }

    // Hàm public để set khoảng cách từ bên ngoài (nếu cần)
    public void SetCameraDistance(float newDistance)
    {
        cameraDistance = newDistance;
    }
}
