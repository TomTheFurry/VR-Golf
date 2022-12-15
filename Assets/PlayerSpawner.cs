using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    public string XRPlayerPrefabName;
    public string PCPlayerPrefabName;

    void Start() {
        if (PhotonNetwork.InLobby || !PhotonNetwork.IsConnected) {
            PhotonNetwork.LocalPlayer.TagObject = Instantiate(Resources.Load<GameObject>(XRManager.HasXRDevices ? XRPlayerPrefabName : PCPlayerPrefabName), Vector3.zero, Quaternion.identity);
        }
        else {
            PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(XRManager.HasXRDevices ? XRPlayerPrefabName : PCPlayerPrefabName, Vector3.zero, Quaternion.identity);
        }
    }
}
