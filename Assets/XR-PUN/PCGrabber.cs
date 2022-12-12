using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public interface IGrabber {
    [PunRPC]
    void OnGrabSuccess();
    
    [PunRPC]
    void OnGrabFail();

}


public class PCGrabber : MonoBehaviour, IGrabber {
    [PunRPC]
    public void OnGrabFail() {
    }

    [PunRPC]
    public void OnGrabSuccess() {
    }


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
