using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;

    int count = 1;

    float time = 0;

    

    private void Update() {
        if (Time.time - time > 0.1f) {
            time = Time.time;
            Instantiate(obj, transform).GetComponent<PlayerListItem>().Text = (count++).ToString("000");
        }
    }
}
