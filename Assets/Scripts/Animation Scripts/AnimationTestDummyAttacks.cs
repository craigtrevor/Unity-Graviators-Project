using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestDummyAttacks : MonoBehaviour {

	public bool spin;
	public bool spinReverse;
	public bool move;
	public float fowardSpeed = 30f;
	public bool laserFix;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update () {
		if (spin) {
			if (spinReverse) {
				transform.eulerAngles -= new Vector3 (0, 10, 0);
			} else {
				transform.eulerAngles += new Vector3 (0, 10, 0);
			}
			transform.position -= new Vector3 (0, 0, 0.5f);
		}
		if (move) {
			transform.Translate (Vector3.back * (fowardSpeed * Time.deltaTime));
		}
		if (laserFix) {
			transform.eulerAngles = new Vector3 (0, 180, 0);
		}
	}
}
