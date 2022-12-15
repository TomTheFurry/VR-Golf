using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    public string XRPlayerPrefabName;
    public string PCPlayerPrefabName;
    public GameObject LocalObj;

    void Start() {
        if (PhotonNetwork.InLobby || !PhotonNetwork.IsConnected) {
            LocalObj = Instantiate(Resources.Load<GameObject>(XRManager.HasXRDevices ? XRPlayerPrefabName : PCPlayerPrefabName), Vector3.zero, Quaternion.identity);
            if (PhotonNetwork.LocalPlayer != null)
                PhotonNetwork.LocalPlayer.TagObject = LocalObj;
        }
        else {
            LocalObj = PhotonNetwork.Instantiate(XRManager.HasXRDevices ? XRPlayerPrefabName : PCPlayerPrefabName, Vector3.zero, Quaternion.identity);
            PhotonNetwork.LocalPlayer.TagObject = LocalObj;
        }
    }
}
