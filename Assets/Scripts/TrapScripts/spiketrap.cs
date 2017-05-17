using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiketrap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			print ("Ouch!");
			Destroy (other.gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
