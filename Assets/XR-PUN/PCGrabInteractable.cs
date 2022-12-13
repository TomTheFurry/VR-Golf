using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.UIElements;

using static UnityEngine.GraphicsBuffer;

public class PCGrabInteractable : MonoBehaviourPun, IOnPhotonViewOwnerChange {
    public bool IsGrabbed => photonView.Owner != null;
    public bool IsGrabbedByMe => photonView.Owner == PhotonNetwork.LocalPlayer;

    public Transform CurrentLocalGrabber = null;
    public Action AsyncCallback = null;
    public Transform TriedGrabber = null;
    
    public float rotationP = 0.02f;
    public float rotationD = 0.2f;
    public float positionP = 5f;
    public float positionD = 0.2f;
    public float attachYOffset = 0.15f;
    public float changeYSpeed = 0.01f;

    public bool noGravityAfterGrab = true;

    public void TryGrabObject(Transform grabber, Action onSuccess) {
        if (IsGrabbedByMe || PhotonNetwork.InLobby || !PhotonNetwork.IsConnected) {
            // Skip sync stuff
            CurrentLocalGrabber = grabber;
            onSuccess();
        }
        else if (IsGrabbed) {
            // Return
        }
        else {
            AsyncCallback = onSuccess;
            TriedGrabber = grabber;
            photonView.RPC("RequestGrabObject", photonView.Owner);
        }

    }


    [PunRPC]
    public void RequestGrabObject() {
        if (IsGrabbed) {
            Debug.Log("Object is already grabbed");
            return;
        }
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    public void ReleaseObject() {
        if (!photonView.IsMine) {
            Debug.LogError("Object is not grabbed by this player");
            return;
        }
        CurrentLocalGrabber = null;
        if (PhotonNetwork.InLobby) {
            Debug.Log("Release object noted, and no sync needed because we are in lobby");
            return;
        }
        photonView.TransferOwnership(0);
    }

    public void OnOwnerChange(Player newOwner, Player previousOwner) {
        if (newOwner.IsLocal) {
            if (TriedGrabber != null) {
                CurrentLocalGrabber = TriedGrabber;
                AsyncCallback?.Invoke();
                AsyncCallback = null;
                TriedGrabber = null;
            }
        }
        else CurrentLocalGrabber = null;
    }

    public void FixedUpdate() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Rigidbody target = GetComponent<Rigidbody>();
        if (CurrentLocalGrabber != null) {
            
            var posdiff = CurrentLocalGrabber.position - target.position;
            var vel = target.velocity;
            var force = posdiff * positionP - vel * positionD;
            target.AddForce(force, ForceMode.VelocityChange);

            var x = Vector3.Cross(target.transform.forward, CurrentLocalGrabber.forward);
            float theta = Mathf.Asin(x.magnitude);
            if (float.IsNormal(x.sqrMagnitude)) {
                if (x.sqrMagnitude < 0.1f) {
                    // Sync roll
                    Vector3 targetUp = CurrentLocalGrabber.up;
                    Vector3 axis = Vector3.Cross(target.transform.up, targetUp);
                    float angle = Vector3.Angle(target.transform.up, targetUp) * 0.5f;
                    x += axis * angle;
                }
                var w = x.normalized * theta / Time.deltaTime;
                target.AddTorque(rotationP * w - rotationD * target.angularVelocity, ForceMode.VelocityChange);
            }
            if (noGravityAfterGrab) target.useGravity = false;

        }
        else {
            if (noGravityAfterGrab) target.useGravity = true;
        }
    }
}
