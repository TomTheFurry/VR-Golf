using UnityEngine;

public class XRSmoothAttach : MonoBehaviour
{
    public Rigidbody target;

    public float rotationP = 0.02f;
    public float rotationD = 0.2f;
    public float positionP = 0.02f;
    public float positionD = 0.2f;

    void Update()
    {
        var posdiff = transform.position - target.position;
        var vel = target.velocity;
        var force = posdiff * positionP - vel * positionD;
        
        target.AddForce(force, ForceMode.VelocityChange);

        var x = Vector3.Cross(target.transform.forward, transform.forward);
        float theta = Mathf.Asin(x.magnitude);
        var w = x.normalized * theta / Time.deltaTime;
        var q = transform.rotation * target.inertiaTensorRotation;
        var t = q * Vector3.Scale(target.inertiaTensor, Quaternion.Inverse(q) * w);
        target.AddTorque(rotationP * t - rotationD * target.angularVelocity, ForceMode.Impulse);
        
    }
}
