using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float timeScale = 1;

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Goal");
        
    }
}
