using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;

public class PCGrabInteractable : MonoBehaviourPun, IOnPhotonViewOwnerChange {
    public bool IsGrabbed => photonView.Owner != null;
    public bool IsGrabbedByMe => photonView.Owner == PhotonNetwork.LocalPlayer;

    public Transform CurrentLocalGrabber = null;
    public Action AsyncCallback = null;
    public Transform TriedGrabber = null;

    public void TryGrabObject(Transform grabber, Action onSuccess) {
        if (IsGrabbedByMe || PhotonNetwork.InLobby) {
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

    public void Update() {
        if (CurrentLocalGrabber != null) {
            transform.position = CurrentLocalGrabber.position;
            transform.rotation = CurrentLocalGrabber.rotation;
        }
    }
}
