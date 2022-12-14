using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class PunToggler : MonoBehaviourPun
{
    public Component[] LocalOnlyComponents;
    public GameObject[] LocalOnlyGameObjects;

    private void Awake() {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby && PhotonNetwork.InRoom && !photonView.IsMine) {
            foreach (var component in LocalOnlyComponents) {
                Destroy(component);
            }
            foreach (var gameObject in LocalOnlyGameObjects) {
                Destroy(gameObject);
            }
        }
    }
}
