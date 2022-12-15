using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPlayfieldTracked {
    void OnEnterPlayfield(Playfield f);
    void OnExitPlayfield(Playfield f);
}

public class Playfield : MonoBehaviourPun
{
    public static int LevelToStartOnSceneLoad = 1; // Master client only

    public static Playfield ActivePlayfield => FindObjectOfType<Ball>().activePlayfield;
    public static Ball Ball => FindObjectOfType<Ball>();
    public static event Action<Playfield> OnPlayfieldEnter;
    public static event Action<Playfield> OnPlayfieldComplete;

    public int levelNumber = 1;
    public Transform SpawnPoint;
    public Transform PlayerSpawnPoint;
    public float StartTime;
    public float PlayTime = -1;
    public bool IsCleared = false;
    public Playfield? NextLevel;

    private bool delayInit = true;

    public void Update() {
        if (delayInit) {
            delayInit = false;
            Debug.Log($"Initializing playfield {levelNumber}...");
            if (PhotonNetwork.IsMasterClient && levelNumber == LevelToStartOnSceneLoad) {
                photonView.RPC("StartLevel", RpcTarget.AllBuffered, Time.time);
            }
            else if (!PhotonNetwork.InRoom) {
                StartLevel(Time.time);
            }
        }
    }
    
    public Dictionary<IPlayfieldTracked, int> TrackedObjects = new Dictionary<IPlayfieldTracked, int>();

    [PunRPC]
    public void StartLevel(float time) {
        Debug.Log("Starting level " + levelNumber);
        IsCleared = false;
        PlayTime = -1;
        StartTime = time;
        Ball.activePlayfield = this;
        Ball.ResetBall(SpawnPoint.position);
        (PhotonNetwork.LocalPlayer.TagObject as GameObject).transform.position = PlayerSpawnPoint.position;
        (PhotonNetwork.LocalPlayer.TagObject as GameObject).transform.rotation = PlayerSpawnPoint.rotation;
        OnPlayfieldEnter?.Invoke(this);
    }

    public void NotifyTrackedEnterSubSection(IPlayfieldTracked tracked) {
        if (TrackedObjects.ContainsKey(tracked)) {
            TrackedObjects[tracked]++;
        }
        else {
            TrackedObjects.Add(tracked, 1);
            tracked.OnEnterPlayfield(this);
        }
    }

    public void NotifyTrackedExitSubSection(IPlayfieldTracked tracked) {
        if (TrackedObjects.ContainsKey(tracked)) {
            TrackedObjects[tracked]--;
            if (TrackedObjects[tracked] == 0) {
                TrackedObjects.Remove(tracked);
                tracked.OnExitPlayfield(this);
            }
        }
    }

    public void LevelCleared() {
        Debug.Log("Level cleared " + levelNumber);
        PlayTime = Time.time - StartTime;
        if (!PhotonNetwork.InRoom) {
            OnLevelCleared(PlayTime);
            return;
        }
        else if (photonView.IsMine)
        {
            Debug.Assert(photonView.IsMine);
            photonView.RPC("OnLevelCleared", RpcTarget.AllBuffered, PlayTime);
        }
    }

    [PunRPC]
    public void OnLevelCleared(float playTime) {
        IsCleared = true;
        PlayTime = playTime;
        OnPlayfieldComplete?.Invoke(this);
    }

    public void AdvanceNextLevel() {
        Debug.Log("Advancing to next level from " + levelNumber);
        Debug.Assert(PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom);
        if (NextLevel != null) {
            if (!PhotonNetwork.InRoom) NextLevel.StartLevel(Time.time);
            else NextLevel.photonView.RPC("StartLevel", RpcTarget.AllBuffered, Time.time);
        }
        else {
            int scenelevel = SceneManagerHelper.ActiveSceneBuildIndex;
            if (scenelevel < SceneManager.sceneCount - 1) {
                if (!PhotonNetwork.InRoom) SceneManager.LoadScene(scenelevel + 1);
                else PhotonNetwork.LoadLevel(scenelevel + 1);
            }
            else {
                if (!PhotonNetwork.InRoom) OnRoomClosed();
                else photonView.RPC("OnRoomClosed", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void OnRoomClosed() {
        Debug.Log("Room closed");
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
