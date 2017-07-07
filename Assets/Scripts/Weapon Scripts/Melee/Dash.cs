using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {
	[SerializeField]
	private Rigidbody RB;
	[SerializeField]
	private Transform PR; // player Rotation


	public float aim;

	public GameObject visualAngleObject;// the object to grab the player model angle when they are dashing
	private Transform dashAngle; // the angle to match the camera when dashing

	//stuff for dashHitBox
	public GameObject dashHitBoxObject;
	public Transform MeleeSpawn;

	private GameObject dash;


	public Transform cameraRotation;

	public double chargePercent = 0; // the amount of charge
	public double chargeMax = 100; // the amount of charge needed
	public double passiveCharge = 0.01; // the amount of charge gained passivly;
	public bool canUseUlt = false; // turns true when charge max has been reached and turns false after the 3 charges have been used.

	[SerializeField]
	private bool isDashing = false; // turns true when the player dashes and off when the player stops, used for raycast collision detection to avoid getting stuck in walls
	[SerializeField]
	private bool dashingRotate = false;

	public float useTimer = 2;// time between uses before it runs out

	public int numberOfDashes = 0;// change this for uses

	public int thrust = 9000; //change this for speed
	public float waitTime = 0.25f;// the delay before the dash stops;


	public float fireRate = 0.25f; // the smaller the faster
	private float nextFire = 1.00f; // the time between firing

	// Use this for initialization
	void Start () {
		//RB = GetComponentInParent<Rigidbody> ();//this if in child
		RB = GetComponent<Rigidbody> (); // when on parent object
		PR = GetComponent<Transform> ();

	}

	// Update is called once per frame
	void Update () {
		dashAngle = visualAngleObject.transform;

		aim = cameraRotation.transform.eulerAngles.y;
		if (isDashing == false){
			PR.localEulerAngles = new Vector3 (0, 0, 0);// stops the player from rotating wildly
		}

		if (dashingRotate == true) {
			dashAngle.eulerAngles = cameraRotation.transform.eulerAngles;

			//PR.localEulerAngles = cameraRotation.transform.eulerAngles; // 


			// get the noname rigidbody
			// rotate to mach the cameraRotation
		}
		Vector3 front = cameraRotation.forward; // used to deterine forward
		Debug.DrawRay (MeleeSpawn.position, front *3, Color.green); // debungging raycast to see direction
		if (isDashing == true) 
		{
			//Vector3 front = transform.TransformDirection (Vector3.forward);

			if(Physics.Raycast(MeleeSpawn.position,front,3)) // is ray hits an object
			{

				print("There is an object in Front of me");
				RB.constraints = RigidbodyConstraints.FreezePosition; // does not allow the player to collide with object
			}
			//check in a object is in front
			// check if object is a player
			//if not a player freeze momentum
		}
		if (chargePercent < chargeMax && numberOfDashes == 0) {
			chargePercent += passiveCharge;
		} else if (chargePercent >= chargeMax)
		{
			numberOfDashes = 3;
			canUseUlt = true;
			chargePercent = 0;
		}

		if (numberOfDashes == 0) {
			canUseUlt = false;
		}


		if(Input.GetKeyDown(KeyCode.F) && Time.time >nextFire)
		{
			nextFire = Time.time + fireRate;
			if (numberOfDashes > 0 && canUseUlt == true) {
				dashingRotate = true;
				charge ();
			}
		}

	}

	void charge()
	{

		RB.constraints = RigidbodyConstraints.None;
		StartCoroutine (tinydelay ());
		numberOfDashes -= 1;
		Debug.Log ("i have used an charge and am now at"+ numberOfDashes);
	}

	IEnumerator tinydelay()
	{	
		dash =  (GameObject) Instantiate (dashHitBoxObject,MeleeSpawn.position,MeleeSpawn.rotation,this.gameObject.transform); 
		GetComponentInChildren<PlayerController> ().enabled = false; // for when in parent
		//GetComponent<PlayerController> ().enabled = false; // for when in child
		isDashing = true;
		RB.AddForce(cameraRotation.forward*thrust);
		yield return new WaitForSeconds (waitTime);
		GetComponentInChildren<PlayerController> ().enabled = true; // for when in parent
		//GetComponent<PlayerController> ().enabled = true; for when in child
		RB.constraints = RigidbodyConstraints.FreezePosition;
		isDashing = false;

		Destroy (dash, waitTime);
		Debug.Log ("i have charged and am now frozen");
		if (numberOfDashes == 2) 
		{
			StartCoroutine (setimer2 ());
		}

		if (numberOfDashes == 1)
		{
			StartCoroutine (setimer1 ());
		}

		if (numberOfDashes == 0) 
		{
			yield return new WaitForSeconds (waitTime);
			RB.constraints = RigidbodyConstraints.None;
			dashingRotate = false;

		}
	}

	IEnumerator setimer2() // a timer to set how long the player can sit for
	{
		yield return new  WaitForSeconds (useTimer); // the time the player can sit
		if (numberOfDashes == 2) // if the number of dashes does not change
		{
			numberOfDashes = 0;
			RB.constraints = RigidbodyConstraints.None;
			dashingRotate = false;
			Debug.Log ("i did not use my charge in time");
		} 

		Debug.Log ("i used my charge in time");
	}

	IEnumerator setimer1() // a timer to set how long the player can sit for
	{
		yield return new  WaitForSeconds (useTimer); // the time the player can sit
		if (numberOfDashes == 1) // if the number of dashes does not change
		{
			numberOfDashes = 0;
			RB.constraints = RigidbodyConstraints.None;
			dashingRotate = false;
			Debug.Log ("i did not use my charge in time");
		} 

		Debug.Log ("i used my charge in time");
	}
}
