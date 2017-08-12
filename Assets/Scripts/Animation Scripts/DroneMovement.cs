using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour {

	public Rigidbody rbody;
	public Vector3 objVel;
	public Vector3 rotationVec;
	public GameObject innerRing;
	public GameObject outerRing;

	public Vector3 spawnPos;
	public bool finishedDelivery = false;
	public Vector3 travelToPos;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody> ();
		spawnPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, travelToPos) > 20 && !finishedDelivery) {
			
		}

		objVel = rbody.velocity;
		rotationVec = new Vector3 (Mathf.Clamp(objVel.y, -20, 20), Mathf.Clamp(objVel.x, -20, 20), 0);
		innerRing.transform.Rotate(rotationVec/3);
		outerRing.transform.Rotate(rotationVec/2);
		if (innerRing.transform.localEulerAngles.x > 20) {
			innerRing.transform.localEulerAngles = new Vector3 (20, innerRing.transform.localEulerAngles.y, innerRing.transform.localEulerAngles.z);
		}
		if (innerRing.transform.localEulerAngles.x < -20) {
			innerRing.transform.localEulerAngles = new Vector3 (-20, innerRing.transform.localEulerAngles.y, innerRing.transform.localEulerAngles.z);
		}
		if (innerRing.transform.localEulerAngles.y > 20) {
			innerRing.transform.localEulerAngles = new Vector3 (innerRing.transform.localEulerAngles.x, 20, innerRing.transform.localEulerAngles.z);
		}
		if (innerRing.transform.localEulerAngles.y < -20) {
			innerRing.transform.localEulerAngles = new Vector3 (innerRing.transform.localEulerAngles.x, -20, innerRing.transform.localEulerAngles.z);
		}

		if (outerRing.transform.localEulerAngles.x > 10) {
			outerRing.transform.localEulerAngles = new Vector3 (10, outerRing.transform.localEulerAngles.y, outerRing.transform.localEulerAngles.z);
		}
		if (outerRing.transform.localEulerAngles.x < -10) {
			outerRing.transform.localEulerAngles = new Vector3 (-10, outerRing.transform.localEulerAngles.y, outerRing.transform.localEulerAngles.z);
		}
		if (outerRing.transform.localEulerAngles.y > 10) {
			outerRing.transform.localEulerAngles = new Vector3 (outerRing.transform.localEulerAngles.x, 10, outerRing.transform.localEulerAngles.z);
		}
		if (outerRing.transform.localEulerAngles.y < -10) {
			outerRing.transform.localEulerAngles = new Vector3 (outerRing.transform.localEulerAngles.x, -10, outerRing.transform.localEulerAngles.z);
		}
	}
}
