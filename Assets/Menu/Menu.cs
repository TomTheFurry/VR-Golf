using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public string menuName;
    public bool open;

    public Transform floor;
    public Transform right;
    public Transform left;
    public Transform ceiling;
    public Transform front;
    public Transform back;

    public void Open() {
        open = true;
        gameObject.SetActive(true);
        MenuManager.usingMenu = this;
    }

    public void Close() {
        open = false;
        gameObject.SetActive(false);
    }
}
