using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IPlayfieldTracked
{
    Rigidbody rig;
    Vector3 lastStablePos;

    public Playfield activePlayfield = null;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (rig.IsSleeping()) {
            lastStablePos = transform.position;
            rig.WakeUp();
        }
    }

    public void ResetBall(Vector3 f) {
        transform.position = f;
        transform.rotation = Quaternion.identity;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal")) {
            Debug.Log("Ball - Goal");
            ResetBall(activePlayfield?.SpawnPoint.position ?? lastStablePos);
        }
    }

    public void OnEnterPlayfield(Playfield f) {

    }

    public void OnExitPlayfield(Playfield f) {
        if (activePlayfield != null && activePlayfield == f) {
            Debug.Log("Ball - Outside playfield! RESET!");
            ResetBall(lastStablePos);
        }
    }
}
