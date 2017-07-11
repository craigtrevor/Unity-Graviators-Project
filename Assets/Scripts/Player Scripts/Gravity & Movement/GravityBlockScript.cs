using UnityEngine;
using System.Collections;

public class GravityBlockScript : MonoBehaviour {

    [SerializeField]
    GameObject controller;

	// Use this for initialization
	void Start () {

        controller = GameObject.FindGameObjectWithTag("PlayerController");	
	}

    //Update position to same as controller position
    public void UpdatePosition() {

        transform.position = controller.transform.position;

    }
}
