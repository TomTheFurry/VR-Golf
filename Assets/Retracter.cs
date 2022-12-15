using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Retracter : MonoBehaviour
{
    public Transform LastOwner = null;
    public float MaxRange;

    public void Update() {
        if (LastOwner == null) {
            var obj = FindObjectOfType<PlayerSpawner>().LocalObj;
            LastOwner = (XRManager.HasXRDevices ? obj.GetComponent<XRControl>().GetTransform() : obj.GetComponent<PCControl>().GetTransform());
        }
    }

    // By XR
    public void OnSelectEntered(SelectEnterEventArgs args) {
        LastOwner = args.interactorObject.transform;
    }

    // By Custom script
    public void OnGrabbed(Transform interactor) {
        LastOwner = interactor;
    }

    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (LastOwner != null && Vector3.Distance(rb.position, LastOwner.position) > MaxRange) {
            Debug.Log("Club Resetted.");
            rb.position = LastOwner.position + Vector3.up;
            rb.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
