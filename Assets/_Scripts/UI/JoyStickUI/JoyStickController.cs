using System;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    public DynamicJoystick variableJoystick;
    
    public IMovement targetMovement;


    public void Start()
    {
        variableJoystick = GetComponent<DynamicJoystick>();
        targetMovement = HoleController.Instance.HoleMovement;
    }

    private void Update()
    {
        MoveTarget();
    }

    private void MoveTarget()
    {
        Vector2 direction = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical);
        if (direction.magnitude > 0.5f)
        {
            targetMovement.Move(direction.normalized);
        }
        else
        {
            targetMovement.Move(Vector2.zero);
        }
        
    }

 
}