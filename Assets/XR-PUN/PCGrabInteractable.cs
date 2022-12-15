using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

using static UnityEngine.GraphicsBuffer;

public class PCGrabInteractable : MonoBehaviourPun {
    public int ownerID = -1;
    public bool IsGrabbed => ownerID != -1;
    public bool IsGrabbedByMe => IsGrabbed && ownerID == PhotonNetwork.LocalPlayer.ActorNumber;

    public Transform CurrentLocalGrabber = null;
    public Action AsyncCallback = null;
    public Transform TriedGrabber = null;
    
    public float rotationP = 0.4f;
    public float rotationD = 0.4f;
    public float positionP = 8f;
    public float positionD = 0.5f;
    public float attachYOffset = 0.15f;
    public float changeYSpeed = 0.01f;

    public bool noGravityAfterGrab = true;

    public UnityEvent<Transform> OnGrabbed;

    public void TryGrabObject(Transform grabber, Action onSuccess) {
        if (IsGrabbedByMe || PhotonNetwork.InLobby || !PhotonNetwork.IsConnected) {
            // Skip sync stuff
            CurrentLocalGrabber = grabber;
            OnGrabbed.Invoke(grabber);
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
    public void NotifyChangeOwner(Photon.Realtime.Player player, bool isReleasing) {
        if (isReleasing) {
            ownerID = -1;
            return;
        }
        ownerID = player.ActorNumber;
        
        if (player.TagObject != null) OnGrabbed.Invoke((player.TagObject as GameObject).GetComponent<Control>().PlayerTransform);
        
        if (player.IsLocal) {
            if (TriedGrabber != null) {
                Debug.Log("Grab successful");
                CurrentLocalGrabber = TriedGrabber;
                AsyncCallback?.Invoke();
                AsyncCallback = null;
                TriedGrabber = null;
            }
            else {
                Debug.Log("Unknown grab received");
                if (photonView.IsMine) {
                    photonView.TransferOwnership(PhotonNetwork.MasterClient);
                    photonView.RPC("NotifyChangeOwner", RpcTarget.OthersBuffered, PhotonNetwork.MasterClient, true);
                }

            }
        }
        else CurrentLocalGrabber = null;
    }

    [PunRPC]
    public void NotifyFailedGrab(string message) {
        Debug.Log("Grab failed: " + message);
    }

    [PunRPC]
    public void RequestGrabObject(PhotonMessageInfo info) {
        if (IsGrabbed) {
            Debug.Log("Object is already grabbed");
            photonView.RPC("NotifyFailedGrab", info.Sender, "Object is already grabbed");
            return;
        }
        Debug.Log("Grab request received by " + info.Sender.ActorNumber);
        photonView.TransferOwnership(info.Sender);
        photonView.RPC("NotifyChangeOwner", RpcTarget.AllBuffered, info.Sender, false);
    }

    public void ReleaseObject() {
        if (!photonView.IsMine) {
            Debug.LogError("Object is not grabbed by this player");
            return;
        }
        CurrentLocalGrabber = null;
        if (!PhotonNetwork.InRoom) {
            Debug.Log("Release object noted, and no sync needed because we are in lobby");
            return;
        }
        photonView.TransferOwnership(PhotonNetwork.MasterClient);
        photonView.RPC("NotifyChangeOwner", RpcTarget.AllBuffered, PhotonNetwork.MasterClient, true);
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
                    float angle = Vector3.Angle(target.transform.up, targetUp) * 1f;
                    x += axis * angle;
                }
                var w = x.normalized * theta / Time.deltaTime;
                target.AddTorque(rotationP * w - rotationD * target.angularVelocity, ForceMode.VelocityChange);
            }
            if (noGravityAfterGrab) target.useGravity = false;
        }
        else if (IsGrabbed) {
            if (noGravityAfterGrab) target.useGravity = false;
        }
        else {
            if (noGravityAfterGrab) target.useGravity = true;
        }
    }
}
