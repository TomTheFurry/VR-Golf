using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun.Demo.Cockpit.Forms;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public abstract class XRToggler : MonoBehaviour
{              
    private static void UpdateAllInstances()
    {
        var list = Resources.FindObjectsOfTypeAll<XRToggler>();
        Debug.Log($"Found {list.Length} XRToggler instances");
        Array.ForEach(list, t => {
            if (!(t.InitOnly && t._isInited) &&
                (
                    t.Toggle == ToggleMode.Toggle
                    || (t.enabled && t.Toggle == ToggleMode.DisableOnly)
                    || (!t.enabled && t.Toggle == ToggleMode.EnableOnly)
                    || (t.enabled && t.Toggle == ToggleMode.DestoryOnly)
                    || t.Toggle == ToggleMode.EnableOrDestry
                )
               )
            t.UpdateState(); 
        });
    }

    static XRToggler() {
        XRManager.OnXRDevicesChanged += UpdateAllInstances;
    }

    protected bool _isInited = false;
    public abstract bool InitOnly { get; }
    
    public enum ToggleMode
    {
        Toggle,
        EnableOnly,
        DisableOnly,
        EnableOrDestry,
        DestoryOnly,
    }

    public abstract ToggleMode Toggle { get; }
    
    protected abstract bool ShouldEnable();

    protected void UpdateState()
    {
        _isInited = true;
        Debug.Log($"Updating object {gameObject}...");
        bool isActive = gameObject.activeSelf;
        bool target = ShouldEnable();
        if (isActive != target) {
            if (!target && (Toggle == ToggleMode.DestoryOnly || Toggle == ToggleMode.EnableOrDestry)) {
                Debug.Log($"Destorying object {gameObject}...");
                Destroy(gameObject);
            }
            else {
                Debug.Log($"Setting active state of {gameObject} to {target}...");
                gameObject.SetActive(target);
            }
        }
    }

    public XRToggler()
    {
    }

    // OnAwake
    void Awake()
    {
        if (!_isInited && XRManager.IsInitialized)
        {
            UpdateState();
        }
    }
}
