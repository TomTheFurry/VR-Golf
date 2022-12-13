using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public TextMeshPro text;
    public UnityEvent triggerEvent;

    private void Awake() {
        
    }

    public void Trigger() {
        triggerEvent.Invoke();
    }
}
