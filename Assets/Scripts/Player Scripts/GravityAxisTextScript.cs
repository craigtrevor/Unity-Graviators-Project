using UnityEngine;
using System.Collections;

public class GravityAxisTextScript : MonoBehaviour {

    public GameObject gravCam;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        transform.rotation = gravCam.transform.rotation; //Set rotation of text to grav cam

    }
}
