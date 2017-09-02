using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grav_bot : MonoBehaviour {



	public Transform [] targets;
	private int targetnumber = 0;
	public float speed = 12;
	float step =0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		step = speed * Time.deltaTime;
		rotateToTarget ();
		gotoTarget();
	}

	void gotoTarget() 
	{
	// Returns if no points have been set up

		if (targets.Length == 0) {
			return;
		}

		transform.position = Vector3.MoveTowards (transform.position, targets[targetnumber].position, step);


	// Choose the next point in the array as the destination,
	// cycling to the start if necessary.
		if (transform.position == targets [targetnumber].position) {
			targetnumber = (targetnumber + 1) % targets.Length;
		}
	}

	void rotateToTarget()
	{	
		Vector3 targetDir = targets [targetnumber].position - transform.position;
		Vector3 newDir = Vector3.RotateTowards (-transform.up, targetDir, step, 0.0f);
		Debug.DrawRay (transform.position, newDir*10, Color.red);
		transform.rotation = Quaternion.LookRotation(transform.forward,-newDir);
	}
}
