using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCControl : MonoBehaviour
{
    public Rigidbody target;
    public Transform grabSource;
    public PCGrabInteractable grabbedObject = null;
    public new Camera camera;
    
    public InputActionReference MoveAction;
    public InputActionReference LookAction;
    public InputActionReference GrabAction;

    public float moveForce = 2;
    public float moveTargetSpeed = 10;

    public float grabRange = 100;
    public float grabSphere = 1f;

    void Update()
    {
        var move = MoveAction.action.ReadValue<Vector2>();
        var look = LookAction.action.ReadValue<Vector2>();

        var relVel = target.transform.InverseTransformDirection(target.velocity);
        var targetVel = Vector3.Normalize(new Vector3(move.x, 0, move.y)) * moveTargetSpeed;
        float maxAccel = moveForce * Time.deltaTime;
        var accel = Vector3.ClampMagnitude(targetVel - relVel, maxAccel);
        target.AddRelativeForce(accel, ForceMode.VelocityChange);

        target.transform.rotation *= Quaternion.Euler(0, look.x, 0);
        

        float pitch = camera.transform.localEulerAngles.x;
        pitch = (pitch + 180) % 360 - 180;
        pitch = Mathf.Clamp(pitch - look.y, -90, 90);
        camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        grabSource.localEulerAngles = new Vector3(pitch, 0, 0);

        if (GrabAction.action.triggered) {

            if (grabbedObject != null) {
                grabbedObject.ReleaseObject();
                grabbedObject = null;
            }
            else {
                // Do raycast
                var ray = new Ray(camera.transform.position, camera.transform.forward);
                if (Physics.SphereCast(ray, grabSphere, out var hit, grabRange)){
                    if (hit.rigidbody != null && hit.rigidbody.TryGetComponent<PCGrabInteractable>(out var grab)) {
                        grab.TryGrabObject(grabSource, () => {
                            grabbedObject = grab;
                            Debug.Log("Grabbed");
                        });
                    }
                }
            }
        }
    }
}
