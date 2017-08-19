using UnityEngine;
using System.Collections;

public class GravityAxisTextScript : MonoBehaviour {

    public GameObject gravCam;
    public GameObject head;

    // Use this for initialization
    void Start() {

    }

    void Update() {

        if (this.gameObject == head) {
            transform.rotation = Quaternion.Euler(gravCam.transform.eulerAngles.x-5f, gravCam.transform.eulerAngles.y, gravCam.transform.eulerAngles.z); //Set rotation of text to grav cam
        }
        else {
            transform.rotation = Quaternion.Lerp(transform.rotation, gravCam.transform.rotation, Time.deltaTime * 10); //Set rotation of text to grav cam
        }

    }
}
