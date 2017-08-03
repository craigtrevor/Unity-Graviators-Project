using UnityEngine;
using System.Collections;

public class GravityAxisTextScript : MonoBehaviour {

    public GameObject gravCam;

    // Use this for initialization
    void Start() {

    }
        
    void Update() {

        transform.rotation = Quaternion.Lerp(transform.rotation, gravCam.transform.rotation, Time.deltaTime*10); //Set rotation of text to grav cam

    }
}
