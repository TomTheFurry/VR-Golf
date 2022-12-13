using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuMouseClick : MonoBehaviour {
    public static Camera usingCam;
    public Transform rightHand;
    public InputActionReference rightHandInput;
    public Transform leftHand;
    public InputActionReference leftHandInput;

    void Update() {
        if (Input.GetMouseButtonDown(0) || rightHandInput.action.triggered || leftHandInput.action.triggered) {
            Ray ray = usingCam.ScreenPointToRay(Input.mousePosition);
            if (rightHandInput.action.triggered)
                ray = new Ray(rightHand.position, rightHand.forward);
            else if (leftHandInput.action.triggered)
                ray = new Ray(leftHand.position, leftHand.forward);

            RaycastHit hit;

            bool isHit = Physics.Raycast(ray, out hit, 100);
            if (isHit && hit.transform.tag == "MenuButton") {
                MenuButton menuButton = hit.transform.GetComponent<MenuButton>();
                if (menuButton != null) {
                    Debug.Log(string.Format("Click: {0}", menuButton.text.text));
                    menuButton.triggerEvent.Invoke();
                    return;
                }
            }
            if (isHit) {
                InputField inputField = hit.transform.GetComponent<InputField>();
                if (inputField != null) {
                    inputField.Select();
                    return;
                }
            }
        }
    }
}
