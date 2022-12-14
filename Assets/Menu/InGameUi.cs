using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUi : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform ui;

    private void Awake() {
        ui.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            ui.rotation = player.rotation;
            ui.gameObject.SetActive(true);
        }
    }

    public void colseUi() {
        ui.gameObject.SetActive(false);
    }

    public void goToTitle() {
        SceneManager.LoadScene("Title");
    }
}
