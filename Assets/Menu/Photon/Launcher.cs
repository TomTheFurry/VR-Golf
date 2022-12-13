using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;  //import Player

using Photon.Realtime;  //import RoomInfo

public class Launcher : MonoBehaviourPunCallbacks {

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    public static Launcher Instance;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;

    [SerializeField] GameObject startGameButton;  //only host can click startGame button

    void Awake() {
        Instance = this;
    }


    void Start() {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();

        //task3
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby() {
        // MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");

        //task2
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");

    }

    public void CreateRoom() {
        if (string.IsNullOrEmpty(roomNameInputField.text)) {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        // MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom() {
        // MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //task2
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++) {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        //task3 
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    //task3 Photon Host migration when host left the room (bultin function) 
    public override void OnMasterClientSwitched(Player newMasterClient) {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }



    public override void OnCreateRoomFailed(short returnCode, string message) {
        errorText.text = "Room Creation Failed: " + message;
        // MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame() {
        PhotonNetwork.LoadLevel(1);
    }


    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        // MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info) {
        PhotonNetwork.JoinRoom(info.Name);
        // MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom() {
        foreach (Transform trans in playerListContent) {
            Destroy(trans.gameObject);
        }
        // MenuManager.Instance.OpenMenu("title");
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
