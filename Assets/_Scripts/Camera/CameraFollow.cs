
using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]private Transform _target;
    [SerializeField]private float _offsetY;
    public Vector3 offsetAdd;
    private void Start()
    {
        _camera = GetComponent<Camera>(); 
    }



    public void Update()
    {
        FollowTheTarget();
    }

    private void FollowTheTarget()
    {
        Vector3 postion = new Vector3(_target.position.x, _offsetY, _target.position.z) + offsetAdd;
        _camera.transform.position = postion;
    }
}
