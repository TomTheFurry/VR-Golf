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
        Debug.Log($"Move {relMove}");
        var yawRot = Quaternion.Euler(0, MoveHandRef.eulerAngles.y, 0);
        var moveDir = yawRot * new Vector3(relMove.x, 0, relMove.y);
        var move = Vector3.Normalize(moveDir) * moveSpeed;
        XROrigin.position += move * Time.deltaTime;
    }
}
