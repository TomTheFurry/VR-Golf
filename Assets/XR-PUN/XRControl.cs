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

    public ActionBasedController LeftHand;
    public XRRayInteractor LeftHandInteractor;
    public InputActionReference LeftHandMove;
    public ActionBasedController RightHand;
    public XRRayInteractor RightHandInteractor;
    public InputActionReference RightHandMove;

    public float moveSpeed = 10f;
    public Vector2 grabMoveSpeed = new(1, 0.1f);
    public Vector2 zRange = new(-0.2f, 1f);

    private void UpdateGrabTransform(Vector2 input, Transform target) {
        Vector3 anchorOffset = target.localPosition;
        Vector3 euler = target.localEulerAngles;
        anchorOffset.z += input.y;
        anchorOffset.z = Math.Clamp(anchorOffset.z, zRange.x, zRange.y);
        euler.z += input.x;
        target.localPosition = anchorOffset;
        target.localEulerAngles = euler;
    }

    void FixedUpdate() {
        var moveDir = Vector3.zero;
        float divider = 0;

        {
            var leftSelected = LeftHandInteractor.interactablesSelected;
            var leftMove = LeftHandMove.action.ReadValue<Vector2>();
            if (leftSelected.Count != 0 && leftSelected[0] is XRGrabInteractable grabbed) {
                UpdateGrabTransform(leftMove * grabMoveSpeed, grabbed.attachTransform);
            }
            else {
                var yawRot = Quaternion.Euler(0, LeftHand.transform.eulerAngles.y, 0);
                moveDir += yawRot * new Vector3(leftMove.x, 0, leftMove.y);
                divider++;
            }
        }

        {
            var rightSelected = RightHandInteractor.interactablesSelected;
            var rightMove = RightHandMove.action.ReadValue<Vector2>();
            if (rightSelected.Count != 0 && rightSelected[0] is XRGrabInteractable grabbed) {
                UpdateGrabTransform(rightMove * grabMoveSpeed, grabbed.attachTransform);
            }
            else {
                var yawRot = Quaternion.Euler(0, RightHand.transform.eulerAngles.y, 0);
                moveDir += yawRot * new Vector3(rightMove.x, 0, rightMove.y);
                divider++;
            }
        }
        if (divider == 0) return;

        var move = moveDir / divider * moveSpeed;
        XROrigin.position += move * Time.fixedDeltaTime;
    }

    public void Teleport(Vector3 pos) {
        XROrigin.position = pos;
    }
    
    public Transform GetTransform() => XROrigin;
}
