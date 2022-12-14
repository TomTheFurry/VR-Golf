using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTexture : MonoBehaviour
{
    [SerializeField] Texture texture;
    Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
    }

    private void Start() {
        rend.material.mainTexture = texture;
    }
}
