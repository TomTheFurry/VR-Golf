using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retracter : MonoBehaviour
{
    public Transform OwnerXR;
    public Transform OwnerPC;
    public float MaxRange;

    private Transform getOwner() {
        return XRManager.HasXRDevices ? OwnerXR : OwnerPC;
    }

    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Vector3.Distance(rb.position, getOwner().position) > MaxRange) {
            Debug.Log("Club Resetted.");
            rb.position = getOwner().position + Vector3.up;
            rb.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
