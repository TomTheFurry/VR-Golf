using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    public Transform startPos;

    public Camera cam;

    public Menu usingMenu;

    private void Awake() {
        Instance = this;
    }

    public void startEvent() {
        stopRotate();
        cam.transform.position = startPos.position;
        cam.transform.rotation = startPos.rotation;
    }

    public void openMenu(Menu menu) {
        usingMenu.gameObject.SetActive(false);
        usingMenu = menu;
        usingMenu.gameObject.SetActive(true);
        changeView("front");
    }

    public void changeView(string targetWall) {
        Transform target;
        switch (targetWall) {
            case "Floor": case "floor": target = usingMenu.floor; break;
            case "Right": case "right": target = usingMenu.right; break;
            case "Left": case "left": target = usingMenu.left; break;
            case "Ceiling": case "ceiling": target = usingMenu.ceiling; break;
            default:
            case "Front":
            case "front": target = usingMenu.front; break;
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
}
