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
        Array.ForEach(list, t => {
            if (!(t.InitOnly && t._isInited) &&
                (
                    t.Toggle == ToggleMode.Toggle
                    || (t.enabled && t.Toggle == ToggleMode.EnableOnly)
                    || (!t.enabled && t.Toggle == ToggleMode.EnableOnly)
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
        Destory,
    }

    public abstract ToggleMode Toggle { get; }
    
    protected abstract bool ShouldEnable();

    protected void UpdateState()
    {
        _isInited = true;
        gameObject.SetActive(ShouldEnable());
    }

    public XRToggler()
    {
    }

    // OnAwake
    void Awake()
    {
        if (!_isInited)
        {
            UpdateState();
        }
    }
}
