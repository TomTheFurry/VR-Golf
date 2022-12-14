using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameUi : MonoBehaviourPun {
    [SerializeField] Transform player;
    [SerializeField] Transform ui;

    [SerializeField] InputActionReference XRInput;

    bool isLocal;

    private void Awake() {
        isLocal = photonView.IsMine || !PhotonNetwork.IsConnected;
        ui.gameObject.SetActive(false);

        if (!isLocal) {
            enabled = false;
        }
    }

    private void Start() {
        MenuMouseClick menu = ui.GetComponent<MenuMouseClick>();
        if (menu.cam == null) {
            Camera[] cams = player.GetComponentsInChildren<Camera>();
            if (cams.Length > 0) {
                menu.cam = cams[0];
                menu.Awake();
            }
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) || XRInput.action.triggered) {
            ui.rotation = player.rotation;
            ui.gameObject.SetActive(true);
        }
    }

    public void colseUi() {
        ui.gameObject.SetActive(false);
    }

    public void goToTitle() {

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }
        else {
            SceneManager.LoadScene("Title");
        }
    }
}
