using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameUi : MonoBehaviourPun {
    static List<InGameUi> instances = new List<InGameUi>();
    public bool isClear = false;

    [SerializeField] Transform player;
    [SerializeField] Transform ui;
    [SerializeField] Transform clearUi;
    [SerializeField] TextMeshPro clearTime;

    [SerializeField] InputActionReference XRInput;

    bool isLocal;

    private void Awake() {
        isLocal = photonView.IsMine || !PhotonNetwork.IsConnected;
        closeUi();

        if (!isLocal) {
            enabled = false;
        }
    }

    private void OnEnable() {
        if (!instances.Contains(this))
            instances.Add(this);
    }
    private void OnDisable() {
        if (instances.Contains(this))
            instances.Remove(this);
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
        if (isClear) {
            if (!isUiOpen)
                openClearUi();
        }
        else if (Input.GetKeyDown(KeyCode.E) || XRInput.action.triggered) {
            openUi();
        }

    }

    public bool isUiOpen => ui.gameObject.activeInHierarchy && clearUi.gameObject.activeInHierarchy;

    public void openUi() {
        ui.rotation = player.rotation;
        ui.gameObject.SetActive(true);
    }
    public void openClearUi() {
        // ** set time
        clearUi.gameObject.SetActive(true);
    }

    public void closeUi() {
        ui.gameObject.SetActive(false);
        clearUi.gameObject.SetActive(false);
    }
    public static void closeAllUi() {
        foreach (InGameUi instance in instances) {
            instance.closeUi();
        }
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

    public void nextLevel() {
        isClear = false;
        closeAllUi();
    }
}
