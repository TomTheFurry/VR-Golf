using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;  //import Player

using Photon.Realtime;  //import RoomInfo

public class Launcher : MonoBehaviourPunCallbacks {
    bool isConnected = false;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    public static Launcher Instance;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;

    [SerializeField] GameObject startGameButton;

    void Awake() {
        Instance = this;
    }

    void Setup() {
        if (!isConnected) {
            //MenuManager.Instance.OpenMenu("Loading");
            MenuManager.Instance.OpenMenu("Multiplayer");

            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
        }
        else {
            MenuManager.Instance.OpenMenu("Multiplayer");
        }
    }

    void Start() {
        Setup();
    }
        

    private void OnEnable() {
        Setup();
    }

    public void GoBack() {
        MenuManager.Instance.OpenMenu("Main");
        gameObject.SetActive(false);
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Master");
        isConnected = true;
        PhotonNetwork.JoinLobby();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby() {
        MenuManager.Instance.OpenMenu("Multiplayer");
        Debug.Log("Joined Lobby");

        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom() {
        if (string.IsNullOrEmpty(roomNameInputField.text)) {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom() {
        MenuManager.Instance.OpenMenu("Multiplayer");
        MenuManager.Instance.changeView("back");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++) {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }



    public override void OnCreateRoomFailed(short returnCode, string message) {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public void StartGame() {
        PhotonNetwork.LoadLevel(1);
    }


    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo info) {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom() {
        foreach (Transform trans in playerListContent) {
            Destroy(trans.gameObject);
        }
        MenuManager.Instance.OpenMenu("Multiplayer");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (Transform trans in roomListContent) {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

}
