using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class PunPlayer : MonoBehaviourPun
{
    void Start() {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby && PhotonNetwork.InRoom) {
            photonView.Owner.TagObject = gameObject;
        }
    }
}
