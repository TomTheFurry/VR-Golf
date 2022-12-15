using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    public static Menu usingMenu;

    public Camera PCCam;
    public Camera XRCam;

    Camera cam;
    Camera Cam {
        get => cam;
        set {
            canvas.worldCamera = value;
            cam = value;
        }
    }
    public Canvas canvas;

    [SerializeField] Menu[] menus;

    void Awake() {
        Instance = this;
        usingMenu = menus[0];
        for (int i = 0; i < menus.Length; i++) {
            if (menus[i].open) {
                usingMenu = menus[i];
            }
            else {
                CloseMenu(menus[i]);
            }
        }
        
    }

    private void Start() {
        Cam = PCCam;
        if (XRManager.HasXRDevices)
            Cam = XRCam;
        FindObjectOfType<MenuMouseClick>().cam = Cam;

        OpenMenu(usingMenu);
    }

    private void FixedUpdate() {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void OpenMenu(string menuName) {
        for (int i = 0; i < menus.Length; i++) {
            if (menus[i].menuName == menuName) {
                menus[i].Open();
            }
            else if (menus[i].open) {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu) {
        for (int i = 0; i < menus.Length; i++) {
            if (menus[i].open) {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
        changeView("front");
    }

    public void CloseMenu(Menu menu) {
        menu.Close();
    }

    public void changeView(string targetWall) {
        Transform target;
        switch (targetWall) {
            case "Floor": case "floor": target = usingMenu.floor; break;
            case "Right": case "right": target = usingMenu.right; break;
            case "Left": case "left": target = usingMenu.left; break;
            case "Ceiling": case "ceiling": target = usingMenu.ceiling; break;
            default:
            case "Front": case "front": target = usingMenu.front; break;
            case "Back": case "back": target = usingMenu.back; break;
        }

        Quaternion rotateTarget = Quaternion.LookRotation((target.position - Cam.transform.position).normalized);
        startRotate(rotateTarget);
    }

    private Coroutine coroutineRotate;
    private float startTime;
    IEnumerator RotateCamera(Quaternion rotateTarget, float moveSpeed = 0.15f) {
        startTime = Time.time;

        while (Quaternion.Angle(Cam.transform.rotation, rotateTarget) > moveSpeed) {
            Cam.transform.rotation = Quaternion.Slerp(Cam.transform.rotation, rotateTarget, moveSpeed * (Time.time - startTime));
            yield return null;
        }
        Cam.transform.rotation = rotateTarget;
        coroutineRotate = null;
        yield return null;
    }

    void startRotate(Quaternion rotateTarget) {
        stopRotate();
        coroutineRotate = StartCoroutine(RotateCamera(rotateTarget));
    }

    void stopRotate() {
        if (coroutineRotate != null) {
            StopCoroutine(coroutineRotate);
            coroutineRotate = null;
        }
    }


    public void startEvent(string level) {
        stopRotate();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.LoadLevel(level);
        else
            SceneManager.LoadScene(level);
    }

    public void startEvent(int level) {
        stopRotate();
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.LoadLevel(level);
        else
            SceneManager.LoadScene(level);
    }
}
