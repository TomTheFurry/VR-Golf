using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;

    int count = 1;

    private void Update() {
        Instantiate(obj, transform).GetComponent<PlayerListItem>().Text += (count++).ToString("000");
    }
}
