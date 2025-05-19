using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    
    public Transform defaultPosition;
    protected override void Start()
    {
        base.Start();
        defaultPosition = transform.Find("DefaultPosition");
        background.position = defaultPosition.position;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.position = defaultPosition.position;
        base.OnPointerUp(eventData);
    }
}