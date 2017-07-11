using UnityEngine;
using System.Collections;

public class GravityAxisTextScript : MonoBehaviour {

    [SerializeField]
    GameObject playerCamera;

    [SerializeField]
    Transform gravityCamera;

    // Use this for initialization
    void Start() {

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gravityCamera = playerCamera.gameObject.transform.GetChild(0);

    }

    // Update is called once per frame
    void Update() {

        transform.rotation = gravityCamera.transform.rotation; //Set rotation of text to grav cam

    }
}
