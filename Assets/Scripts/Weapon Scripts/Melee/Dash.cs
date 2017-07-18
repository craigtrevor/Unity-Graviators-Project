using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

	// objects
	private Rigidbody playerRigidBody; // the rigibody of the player. Is assigned in start
	public Transform cameraRotation; // the camerasrotation, assign in game editor
	[SerializeField]
	private Transform PR; // player Rotation
	public GameObject visualAngleObject;// the object to grab the player model angle when they are dashing
	private Transform dashAngle; // the angle to match the camera when dashing

	//stuff for dashHitBox
	public GameObject dashHitBoxObject;
	public Transform MeleeSpawn;
	private GameObject dash;

	// numbers 
	//charging
	public double chargePercent = 0; // the amount of charge
	public double chargeMax = 100; // the amount of charge needed
	public double passiveCharge = 0.01; // the amount of charge gained passivly;
	public int numberOfDashes = 0;// change this for uses

	//stats
	public int thrust = 9000; //change this for speed
	public float waitTime = 0.25f;// the time the dash goes before the dash stops;
	public float fireRate = 0.25f; // the smaller the faster
	private float nextFire = 1.00f; // the time between firing
	public float useTimer = 2;// time between uses before it runs out


	//bools
	public bool canUseUlt = false; // turns true when charge max has been reached and turns false after the 3 charges have been used.
	[SerializeField]
	private bool isDashing = false; // turns true when the player dashes and off when the player stops, used for raycast collision detection to avoid getting stuck in walls
	[SerializeField]
	private bool dashingRotate = false;


	// Use this for initialization
	void Start () {
		playerRigidBody = GetComponent<Rigidbody> (); // when on parent object
		PR = GetComponent<Transform> ();

	}

	// Update is called once per frame
	void Update () {
		dashAngle = visualAngleObject.transform;
		if (isDashing == false && canUseUlt == true){
			PR.localEulerAngles = new Vector3 (0, 0, 0);// stops the player from rotating wildly
		}

		if (dashingRotate == true) {
			dashAngle.eulerAngles = cameraRotation.transform.eulerAngles;
		}

		Vector3 front = cameraRotation.forward; // used to deterine forward
		Debug.DrawRay (MeleeSpawn.position, front *3, Color.green); // debungging raycast to see direction
		if (isDashing == true) 
		{

			if(Physics.Raycast(MeleeSpawn.position,front,3)) // is ray hits an object
			{

				print("There is an object in Front of me");
				playerRigidBody.constraints = RigidbodyConstraints.FreezePosition; // does not allow the player to collide with object
				dashingRotate = true;
			}

		}

		if (chargePercent < chargeMax && numberOfDashes == 0) {
			chargePercent += passiveCharge;
		} else if (chargePercent >= chargeMax)
		{
			numberOfDashes = 3;
			canUseUlt = true;
			chargePercent = 0;
		}

//		if (numberOfDashes == 0) {
//			playerRigidBody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
//			playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
//			canUseUlt = false;
//			dashingRotate = false;
//			// place code here to reset player rotation
//			visualAngleObject.transform.localRotation = Quaternion.Euler(0,0,0);
//		}


		if(Input.GetKeyDown(KeyCode.F) && Time.time >nextFire)
		{
			nextFire = Time.time + fireRate;
			if (numberOfDashes > 0 && canUseUlt == true) {
				StartCoroutine (charge());
				numberOfDashes -= 1; // use a dash
			}
		}

	}

	IEnumerator charge()
	{
		isDashing = true;
		dash =  (GameObject) Instantiate (dashHitBoxObject,MeleeSpawn.position,MeleeSpawn.rotation,this.gameObject.transform); 
		playerRigidBody.constraints = RigidbodyConstraints.None; // turn off constraints
		GetComponentInChildren<PlayerController> ().enabled = false; // turn off player controls
		playerRigidBody.AddForce(cameraRotation.forward*thrust);// push the player forward
		yield return new WaitForSeconds (waitTime); // wait timer for distance of dash
		GetComponentInChildren<PlayerController> ().enabled = true; // turn on player controls
		playerRigidBody.constraints = RigidbodyConstraints.FreezePosition; // freeze the players location
		dashingRotate = true;
		Destroy (dash, waitTime);
		isDashing = false;

		if (numberOfDashes == 2) 
		{
			yield return new  WaitForSeconds (useTimer); // the time the player can sit
			if (numberOfDashes == 2) // if the number of dashes does not change
			{
				numberOfDashes = 0;
				playerRigidBody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
				playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
			} 
		}

		if (numberOfDashes == 1) 
		{
			yield return new  WaitForSeconds (useTimer); // the time the player can sit
			if (numberOfDashes == 1) // if the number of dashes does not change
			{
				numberOfDashes = 0;
				playerRigidBody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
				playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
			} 
		}

		if (numberOfDashes == 0) {
			playerRigidBody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
			playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
			canUseUlt = false;
			dashingRotate = false;
			// place code here to reset player rotation
			visualAngleObject.transform.localRotation = Quaternion.Euler(0,0,0);
		}
	}
}
