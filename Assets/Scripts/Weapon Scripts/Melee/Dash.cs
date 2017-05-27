using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

	public Transform cameraRotation;
	public double chargePercent = 0; // this control the chrage
	public double chargeMax = 100; // the amount of charge needed
	public double passiveCharge = 0.01; // the amount of charge gained passivly;
	public bool canUseUlt = false; // turns ture when charge max has been reached and turns false after the 3 charges have been used.

	public float useTimer = 2;// time between uses before it runs out
	public int dashTesting;

	public int numberOfDashes = 0;// change this for uses

	public int thrust = 1; //change this for speed
	public int distance = 10; // change this for distance travalled
	private float waitTime = 0.01f;// the delay between movements 


	public float fireRate = 1.0f; // the smaller the faster
	private float nextFire = 1.0f; // the time between firing

	// Use this for initialization
	void Start () {
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
			GetComponent<CharacterController> ().enabled = true;
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

		StartCoroutine (tinydelay ());
		numberOfDashes -= 1;
		Debug.Log ("i have used an charge and am now at"+ numberOfDashes);
	}

	IEnumerator tinydelay()
	{	
		GetComponent<CharacterController> ().enabled = true;
		CharacterController controller = GetComponent<CharacterController> ();
		//GetComponent<TPSCharacterController> ().vertVelocity = 0;
		for(int i=0; i<distance; i++){
		controller.Move (cameraRotation.forward * thrust);
		yield return new WaitForSeconds (waitTime);
		//Debug.Log ("waited for " + waittime + " seconds");
		}
		GetComponent<CharacterController>().enabled =  false;
		Debug.Log ("i have charged and am now frozen");
		if (numberOfDashes == 2) 
		{
			StartCoroutine (setimer2 ());
		}

		if (numberOfDashes == 1)
		{
			StartCoroutine (setimer1 ());
		}




	}

	IEnumerator setimer2() // a timer to set how long the player can sit for
	{
		yield return new  WaitForSeconds (useTimer); // the time the player can sit
		if (numberOfDashes == 2) // if the number of dashes does not change
		{
			numberOfDashes = 0;
			GetComponent<CharacterController> ().enabled = true;
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
			GetComponent<CharacterController> ().enabled = true;
			Debug.Log ("i did not use my charge in time");
		} 

		Debug.Log ("i used my charge in time");
	}
}
