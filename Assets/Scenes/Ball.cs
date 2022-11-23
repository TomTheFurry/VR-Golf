using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rig;
    Vector3 pos;
    public float forceMin = 1800f;
    public float forceMax = 2200f;

    void Start()
    {
        pos = transform.position;
        pos.z=  0;
        transform.position = pos + Vector3.forward * Random.Range(-20, 20);
        rig = GetComponent<Rigidbody>();
        rig.AddForce(Vector3.right * -Random.Range(forceMin, forceMax));
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ball - Goal");
        transform.position = pos + Vector3.forward * Random.Range(-20, 20);
        transform.rotation = Quaternion.identity;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.AddForce(Vector3.right * -Random.Range(forceMin, forceMax));
    }
}
