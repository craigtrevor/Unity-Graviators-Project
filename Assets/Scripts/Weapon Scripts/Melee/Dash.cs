using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {
	[SerializeField]
	private Rigidbody RB;

	//stuff for dashHitBox
	public GameObject dashHitBoxObject;
	public Transform MeleeSpawn;

	private GameObject dash;


	public Transform cameraRotation;
	public double chargePercent = 0; // the amount of charge
	public double chargeMax = 100; // the amount of charge needed
	public double passiveCharge = 0.01; // the amount of charge gained passivly;
	public bool canUseUlt = false; // turns true when charge max has been reached and turns false after the 3 charges have been used.

	public float useTimer = 2;// time between uses before it runs out

	public int numberOfDashes = 0;// change this for uses

	public int thrust = 9000; //change this for speed
	public float waitTime = 0.25f;// the delay before the dash stops;


	public float fireRate = 0.25f; // the smaller the faster
	private float nextFire = 1.00f; // the time between firing

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
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
		GetComponent<PlayerController> ().enabled = false;
		RB.AddForce(cameraRotation.forward*thrust);
		yield return new WaitForSeconds (waitTime);
		GetComponent<PlayerController> ().enabled = true;
		RB.constraints = RigidbodyConstraints.FreezePosition;
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

		}






	}

	IEnumerator setimer2() // a timer to set how long the player can sit for
	{
		yield return new  WaitForSeconds (useTimer); // the time the player can sit
		if (numberOfDashes == 2) // if the number of dashes does not change
		{
			numberOfDashes = 0;
			RB.constraints = RigidbodyConstraints.None;
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
			Debug.Log ("i did not use my charge in time");
		} 

		Debug.Log ("i used my charge in time");
	}
}
