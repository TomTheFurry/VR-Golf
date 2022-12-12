using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRControl : MonoBehaviour
{
    public Transform XROrigin;
    public Transform MoveHandRef;

    public InputActionReference MoveAction;

    public float moveSpeed = 10f;

    void Update()
    {
        var relMove = MoveAction.action.ReadValue<Vector2>();
        var move = MoveHandRef.TransformDirection(new Vector3(relMove.x, 0, relMove.y));
        move = Vector3.Normalize(Vector3.ProjectOnPlane(move, XROrigin.up)) * moveSpeed;
        XROrigin.position += move * Time.deltaTime;
    }
}
