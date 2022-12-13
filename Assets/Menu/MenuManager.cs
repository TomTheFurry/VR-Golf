using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    public static Menu usingMenu;

    public Camera cam;

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
        OpenMenu(usingMenu);
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

        Quaternion rotateTarget = Quaternion.LookRotation((target.position - cam.transform.position).normalized);
        startRotate(rotateTarget);
    }

    private Coroutine coroutineRotate;
    private float startTime;
    IEnumerator RotateCamera(Quaternion rotateTarget, float moveSpeed = 0.15f) {
        startTime = Time.time;

        while (Quaternion.Angle(cam.transform.rotation, rotateTarget) > moveSpeed) {
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, rotateTarget, moveSpeed * (Time.time - startTime));
            yield return null;
        }
        cam.transform.rotation = rotateTarget;
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


    //public void startEvent() {
    //    stopRotate();
    //    cam.transform.position = startPos.position;
    //    cam.transform.rotation = startPos.rotation;
    //}
}
