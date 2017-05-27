﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour {

	[SerializeField]
	private Rigidbody RB;


	public bool gothit = false;
	public bool cangethit= true;




	public Transform cameraRotation;
	public int thrust = 1; //change this for speed
	public int distance = 50 ; // change this for distance travalled
	private float waittime = 0.01f;// the delay between movements 




	// Use this for initialization
	void Start () {
		RB = GetComponentInChildren<Rigidbody> ();

	}

	// Update is called once per frame
	void Update () 
	{
		if (cangethit == true) // can the player get hit
		{
			if (gothit == true) // was the player hit
			{
				StartCoroutine (changeback ());
				cangethit = false;
			}
		}

		//onDamage (1);
	}

	void OnTriggerEnter (Collider col)
	{
		if (gothit == false)
		{
			if (col.tag == "MeleeWeapon")
			{
				if(col.transform.root != transform.root)
				{
					if (col.GetComponent<DamageStorage> ().Damage == this.GetComponentInChildren<DamageStorage> ().Damage && GetComponent<Attack>().Attaking == true)
					{
						knockBack();
					} else 
					{
						Debug.Log ("i am hurt");
						transform.GetComponent<Renderer> ().material.color = Color.red;
						GetComponent<Network_PlayerStats> ().maxHealth = GetComponent<Network_PlayerStats> ().maxHealth - col.GetComponent<DamageStorage> ().Damage;
						//col.GetComponentInParent<Dash> ().chargePercent += col.GetComponent<DamageStorage> ().Damage;
						gothit = true;
					}
				}
			}
			if (col.tag == "ThrowingSword") 
			{	
				Debug.Log ("i am hurt");
				transform.GetComponent<Renderer> ().material.color = Color.red;
				GetComponent<Network_PlayerStats> ().maxHealth = GetComponent<Network_PlayerStats> ().maxHealth - col.GetComponent<damageRange> ().Damage;
				gothit = true;
			}

			if (col.tag == "dashHitBox") 
			{
				if (col.transform.root != transform.root) 
				{
					GetComponent<Network_PlayerStats> ().maxHealth = GetComponent<Network_PlayerStats> ().maxHealth - col.GetComponent<DashDamage> ().damage;
				}
			}
		}
	}

	IEnumerator changeback(){
		yield return new WaitForSeconds (0.5f);
		Debug.Log ("I am ok now");
		transform.GetComponent<Renderer> ().material.color = Color.white;
		gothit = false;
		cangethit = true;
	}

	void knockBack()
	{


		StartCoroutine (tinydelay ());

		Debug.Log ("i have been knocked back");
	}

	IEnumerator tinydelay()
	{	
		GetComponent<PlayerController> ().enabled = false;
		RB.AddForce(cameraRotation.forward*-thrust);
		yield return new WaitForSeconds (waittime);
		GetComponent<PlayerController> ().enabled = true;
	}

}
