using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public TextMeshPro text;
    public UnityEvent triggerEvent;

    Renderer rend;
    ColorHSV orignalColor;
    ColorHSV selectedColor;

    public Image image;
    public Sprite normalTexture;
    public Sprite sellectedTexture;

    private void Awake() {
        rend = GetComponent<Renderer>();
        selectedColor = orignalColor = rend.material.color;
        selectedColor.s /= 2f;
        if (image != null)
            image.sprite = normalTexture;
    }
    
    public void onSelect() {
        rend.material.color = selectedColor;
        if (image != null)
            image.sprite = sellectedTexture;
    }

    public void onDeselect() {
        rend.material.color = orignalColor;
        if (image != null)
            image.sprite = normalTexture;
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
