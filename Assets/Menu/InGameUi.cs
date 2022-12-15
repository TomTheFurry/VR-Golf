using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class InGameUi : MonoBehaviourPun {

    [SerializeField] Transform player;
    [SerializeField] Transform ui;
    [SerializeField] Transform clearUi;
    [SerializeField] Transform clearUiMasterOnly;
    [SerializeField] TextMeshPro clearTime;

    [SerializeField] InputActionReference XRInput;

    public MenuMouseClick click;

    private void Start() {
        MenuMouseClick menu = click;
        if (menu.cam == null) {
            Camera[] cams = player.GetComponentsInChildren<Camera>();
            if (cams.Length > 0) {
                menu.cam = cams[0];
                menu.Awake();
            }
        }
        Playfield.OnPlayfieldEnter += OnPlayfieldChange;
        Playfield.OnPlayfieldComplete += OnPlayfieldComplete;
    }

    private void OnPlayfieldComplete(Playfield obj) {
        closeUi();
        openClearUi();
    }

    private void OnPlayfieldChange(Playfield obj) {
        closeUi();
    }

    private void Update() {
        click.enabled = isUiOpen;
        if (!isUiOpen && (Input.GetKeyDown(KeyCode.E) || XRInput.action.triggered)) {
            if (Playfield.ActivePlayfield.IsCleared) openClearUi();
            else openUi();
        }
    }

    private void OnDestroy() {
        Playfield.OnPlayfieldEnter -= OnPlayfieldChange;
        Playfield.OnPlayfieldComplete -= OnPlayfieldComplete;
    }

    public bool isUiOpen => ui.gameObject.activeInHierarchy || clearUi.gameObject.activeInHierarchy;

    public void openUi() {
        ui.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
        clearUi.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
        ui.gameObject.SetActive(true);
    }
    
    public void openClearUi() {
        float time = Playfield.ActivePlayfield.PlayTime;
        clearTime.text = string.Format("{0:00}:{1:00}", time / 60, time % 60);
        ui.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
        clearUi.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
        clearUi.gameObject.SetActive(true);
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) clearUiMasterOnly.gameObject.SetActive(false);
    }

    public void closeUi() {
        ui.gameObject.SetActive(false);
        clearUi.gameObject.SetActive(false);
    }

    public void goToTitle() {
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
        }
        SceneManager.LoadScene(0);
    }

    public void nextLevel() {
        Debug.Assert(PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom);
        Playfield.ActivePlayfield.AdvanceNextLevel();
    }
}
