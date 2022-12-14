using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuMouseClick : MonoBehaviour {
    public static Camera usingCam;
    public static GameObject hitObj { get; private set; }
    public Transform rightHand;
    public InputActionReference rightHandInput;

    void Update() {
        Ray ray = usingCam.ScreenPointToRay(Input.mousePosition);
        if (XRManager.HasXRDevices)
            ray = new Ray(rightHand.position, rightHand.forward);

        RaycastHit hit;
        hitObj = null;

        bool isHit = Physics.Raycast(ray, out hit, 100);
        if (isHit && hit.transform.tag == "MenuButton") {
            MenuButton menuButton = hit.transform.GetComponent<MenuButton>();
            if (menuButton != null) {
                Debug.Log(string.Format("Click: {0}", menuButton.text.text));
                if (isInput())
                    menuButton.triggerEvent.Invoke();
                hitObj = hit.transform.gameObject;
                return;
            }
        }
        //if (isHit) {
        //    InputField inputField = hit.transform.GetComponent<InputField>();
        //    if (inputField != null) {
        //        inputField.Select();
        //        return;
        //    }
        //}
    }

    bool isInput() {
        return Input.GetMouseButtonDown(0) || rightHandInput.action.triggered;
    }
}
