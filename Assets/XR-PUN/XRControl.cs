using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControl : MonoBehaviour
{
    public Transform XROrigin;
    public Transform MoveHandRef;

    public InputActionReference MoveAction;
    public InputActionReference GrabMoveAction;

    public XRRayInteractor Interactor;

    public float moveSpeed = 10f;

    void Update()
    {
        var relMove = MoveAction.action.ReadValue<Vector2>();
        Debug.Log($"Move {relMove}");
        var yawRot = Quaternion.Euler(0, MoveHandRef.eulerAngles.y, 0);
        var moveDir = yawRot * new Vector3(relMove.x, 0, relMove.y);
        var move = Vector3.Normalize(moveDir) * moveSpeed;
        XROrigin.position += move * Time.deltaTime;

        var list = Interactor.interactablesSelected;
        if (list.Count != 0 && list[0] is XRGrabInteractable grabbed) {
            Vector3 anchorOffset = grabbed.attachTransform.localPosition;
            Vector3 euler = grabbed.attachTransform.localEulerAngles;
            
            var grabMove = GrabMoveAction.action.ReadValue<Vector2>();
            anchorOffset.z += grabMove.y * Time.deltaTime;
            euler.z += grabMove.x * Time.deltaTime;
            grabbed.attachTransform.localPosition = anchorOffset;
            grabbed.attachTransform.localEulerAngles = euler;
        }
    }
}
