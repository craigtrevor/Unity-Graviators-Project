using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dash : NetworkBehaviour {

	// objects
	private Rigidbody playerRigidBody; // the rigibody of the player. Is assigned in start
	public Transform cameraRotation; // the camerasrotation, assign in game editor
	[SerializeField]
	private Transform PR; // player Rotation
	public GameObject visualAngleObject;// the object to grab the player model angle when they are dashing
	private Transform dashAngle; // the angle to match the camera when dashing

	//stuff for dashHitBox
	public Transform MeleeSpawn;
	private GameObject dash;

	// numbers 
	//charging
	public float chargePercent = 0; // the amount of charge
	public float chargeMax = 100; // the amount of charge needed
	public float passiveCharge = 0.01f; // the amount of charge gained passivly;
	public int numberOfDashes = 0;// change this for uses

	//stats
	public int thrust = 9000; //change this for speed
	public float waitTime = 0.25f;// the time the dash goes before the dash stops;
	public float fireRate = 0.25f; // the smaller the faster
	private float nextFire = 1.00f; // the time between firing
	public float useTimer = 2;// time between uses before it runs out

	//dash damage storage
	[SerializeField]
	private float dashDamage = 500;
	private Collider[] hitColliders;
	private Vector3 attackOffset;
	private float attackRadius;
	private const string PLAYER_TAG = "Player";

	//stuff to stop player colliding with walls
	[SerializeField]
	private LayerMask mask;
	public Transform headSensor;
	public Transform legSensor;
	public Transform leftSensor;
	public Transform rightSensor;

	//bools
	public bool canUseUlt = false; // turns true when charge max has been reached and turns false after the 3 charges have been used.
	[SerializeField]
	private bool isDashing = false; // turns true when the player dashes and off when the player stops, used for raycast collision detection to avoid getting stuck in walls
	[SerializeField]
	private bool dashingRotate = false;
    private bool dashPause = false;

    // Scripts
    Network_PlayerManager networkPlayerManager;

    // Use this for initialization
    void Start () {
		playerRigidBody = GetComponent<Rigidbody> (); // when on parent object
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();
        PR = GetComponent<Transform> ();
		attackRadius = 5;
        chargePercent = 0;
        canUseUlt = false;

    }

	// Update is called once per frame
	void Update () 
	{
		DashAttack ();
		if (isDashing == true) {
			DashDamageing();
		}

		dashAngle = visualAngleObject.transform;
		if (isDashing == false && canUseUlt == true){
			PR.localEulerAngles = new Vector3 (0, 0, 0);// stops the player from rotating wildly
		}

		if (dashingRotate == true) {
			dashAngle.eulerAngles = cameraRotation.transform.eulerAngles;
		}

		Vector3 front = cameraRotation.forward; // used to deterine forward

		Debug.DrawRay (headSensor.position, front *2, Color.green); // debungging raycast to see direction
		Debug.DrawRay (legSensor.position, front *2, Color.green); // debungging raycast to see direction
		Debug.DrawRay (leftSensor.position, front *2, Color.green); // debungging raycast to see direction
		Debug.DrawRay (rightSensor.position, front *2, Color.green); // debungging raycast to see direction
		if (isDashing == true) 
		{

			if(Physics.Raycast(headSensor.position,front,2,mask)||Physics.Raycast(legSensor.position,front,2,mask)||Physics.Raycast(leftSensor.position,front,2,mask)||Physics.Raycast(rightSensor.position,front,2,mask)) // is ray hits an object
			{

				print("There is an object in Front of me");
				playerRigidBody.constraints = RigidbodyConstraints.FreezePosition; // does not allow the player to collide with object
				dashingRotate = true;
			}

		}
			
		if (chargePercent < chargeMax && numberOfDashes == 0) {
			chargePercent += passiveCharge;
            CmdChargeUltimate(passiveCharge);
        }
        else if (chargePercent >= chargeMax)
		{
			numberOfDashes = 3;
			canUseUlt = true;
			chargePercent = 0;
		}

//		if(Input.GetKeyDown(KeyCode.F) && Time.time >nextFire)
//		{
//			nextFire = Time.time + fireRate;
//			if (numberOfDashes > 0 && canUseUlt == true) {
//				StartCoroutine (charge());
//				numberOfDashes -= 1; // use a dash
//			}
//		}

	}

	[Client]
	void DashAttack()
	{
		if(Input.GetKeyDown(KeyCode.F) && Time.time >nextFire)
		{
			nextFire = Time.time + fireRate;
			if (numberOfDashes > 0 && canUseUlt == true) {
				StartCoroutine (charge());
				numberOfDashes -= 1; // use a dash
			}
		}
	}

	[Client]
	public void DashDamageing()
	{
		hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

		foreach (Collider hitCol in hitColliders)
		{
			if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG)
			{
				Debug.Log("Hit Player!");

				CmdTakeDamage(hitCol.gameObject.name, dashDamage, transform.name);
			}
		}
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}

	[Command]
	protected void CmdPlayerAttacked(string _playerID, float _damage, string _sourceID)
	{
		Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		if (isDashing)
		{
			networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
		}
	}

    [Command]
    void CmdChargeUltimate(float _ultimatePoints)
    {
        networkPlayerManager.RpcUltimateCharging(_ultimatePoints);
    }

    IEnumerator charge()
	{
        dashPause = true; // alex put dash start here
		Vector3 front = cameraRotation.forward; // used to deterine forward
		if (Physics.Raycast (headSensor.position, front, 2,mask) || Physics.Raycast (legSensor.position, front, 2,mask) || Physics.Raycast (leftSensor.position, front, 2,mask) || Physics.Raycast (rightSensor.position, front, 2,mask)) { // is ray hits an object
			print ("i cannot go forward as i will stuff up");
			playerRigidBody.constraints = RigidbodyConstraints.FreezePosition; // does not allow the player to collide with object
			dashingRotate = true;
		}else {
            dashPause = false; // alex end dash pause
			isDashing = true;
			//dash =  (GameObject) Instantiate (dashHitBoxObject,MeleeSpawn.position,MeleeSpawn.rotation,this.gameObject.transform); 
			playerRigidBody.constraints = RigidbodyConstraints.None; // turn off constraints
			GetComponentInChildren<PlayerController> ().enabled = false; // turn off player controls
			playerRigidBody.AddForce (cameraRotation.forward * thrust);// push the player forward
			yield return new WaitForSeconds (waitTime); // wait timer for distance of dash
			GetComponentInChildren<PlayerController> ().enabled = true; // turn on player controls
			playerRigidBody.constraints = RigidbodyConstraints.FreezePosition; // freeze the players location
			dashingRotate = true;
			Destroy (dash, waitTime);
			isDashing = false; // alex end dash 
            dashPause = true; // alex start dash pause
		}


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
