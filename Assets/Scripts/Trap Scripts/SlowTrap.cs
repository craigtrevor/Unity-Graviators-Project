using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float Forward = GetComponent<PlayerController> ().moveSettings.forwardVel;
		float Right = GetComponent<PlayerController> ().moveSettings.rightVel;
		float RotateVel = GetComponent<PlayerController> ().moveSettings.rotateVel;
		float Jump = GetComponent<PlayerController> ().moveSettings.jumpVel;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			print ("Hi");

			//float Jump / 2;
		}
	}
}
