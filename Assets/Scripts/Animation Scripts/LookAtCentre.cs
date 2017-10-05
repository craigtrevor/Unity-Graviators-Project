using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCentre : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (Vector3.zero);
	}
}
