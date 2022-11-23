using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 RandForce => Quaternion.AngleAxis(Random.Range(angleMin, angleMax), Vector3.up) * (Vector3.left * Random.Range(forceMin, forceMax));
    Vector3 RandPos => pos + Vector3.forward * Random.Range(-20, 20);

    Rigidbody rig;
    Vector3 pos;
    public float forceMin = 1800f;
    public float forceMax = 2200f;
    public float angleMin = -30f;
    public float angleMax = 30f;

    void Start()
    {
        pos = transform.position;
        pos.z=  0;
        transform.position = RandPos;
        rig = GetComponent<Rigidbody>();
        rig.AddForce(RandForce);
    }

    private void Update() {
        if (rig.IsSleeping()) {
            rig.AddForce(RandForce);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ball - Goal");
        transform.position = RandPos;
        transform.rotation = Quaternion.identity;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.AddForce(RandForce);
    }
}
