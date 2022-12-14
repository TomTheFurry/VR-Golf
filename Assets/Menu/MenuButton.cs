using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public TextMeshPro text;
    public UnityEvent triggerEvent;

    Renderer rend;
    ColorHSV orignalColor;
    ColorHSV selectedColor;

    private void Awake() {
        rend = GetComponent<Renderer>();
        selectedColor = orignalColor = rend.material.color;
        selectedColor.s /= 2f;
    }
    
    public void onSelect() {
        rend.material.color = selectedColor;
    }

    public void onDeselect() {
        rend.material.color = orignalColor;
    }

    private void Update() {
        if (MenuMouseClick.hitObj == gameObject) {
            onSelect();
        }
        else {
            onDeselect();
        }
    }

    public void Trigger() {
        triggerEvent.Invoke();
    }
}
