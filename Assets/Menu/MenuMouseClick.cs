using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMouseClick : MonoBehaviour {
    public static Camera usingCam;
    

    private void Awake() {
        usingCam = GetComponent<Camera>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = usingCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100) && hit.transform.tag == "MenuButton") {
                MenuButton menuButton = hit.transform.GetComponent<MenuButton>();
                if (menuButton != null) {
                    Debug.Log(string.Format("Click: {0}", menuButton.text.text));
                    menuButton.triggerEvent.Invoke();
                }
            }
            InputField inputField = hit.transform.GetComponent<InputField>();
            if (inputField != null) {
                inputField.Select();
            }
        }
    }
}
