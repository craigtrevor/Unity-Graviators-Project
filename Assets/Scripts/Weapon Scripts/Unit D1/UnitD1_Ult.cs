using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitD1_Ult : NetworkBehaviour {
	
	public string _sourceID; //set the source id to the player that throws it. this is set by transform.name
	public float ultsizewanted;// takes in the player damage and uses that to generate the ult size wanted
	public float playerYVelocity; //stores the Players Y velocity to see if they are jumping
	//charging
	public double chargePercent = 0; // the amount of charge
	public double chargeMax = 100; // the amount of charge needed
	public double passiveCharge = 0.01; // the amount of charge gained passivly;

	public bool canUseUlt = false; // turns true when charge max has been reached and turns false after after UNit starts falling with and key is pressed.
	[SerializeField]
	private bool UltActive = false; // turns true when the player starts the ult off when the player lands,
	[SerializeField]
	private bool isFalling = false; // turns true when the player is in the air used to stop the player ulting on the ground


	public Rigidbody weapon; // prefab of the unitD1 ult hitbox
	public Transform ultSpawnLocationTransform; // a child of the player where the ult object is spawned




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (chargePercent < chargeMax && canUseUlt == false) {
			chargePercent += passiveCharge; // charges the ult
		} else if (chargePercent >= chargeMax) // if ult is fully charged
		{
			canUseUlt = true;
			chargePercent = 0;
		}

		if(canUseUlt = true)
		{
			playerYVelocity = this.GetComponentInChildren<PlayerController> ().velocity.y; // storing the player jump velocity
			if (playerYVelocity < 0) { // if they are falling
				isFalling = true;
			} else 
			{
				isFalling = false;
			}

		}

		if(Input.GetKeyDown(KeyCode.F)&& isFalling == true) // if f is pressed while the player is in air set is falling to true
		{
			UltActive = true;	
			canUseUlt = false;
		}

		if (UltActive == true)
		{
			//if in air store the damage but if not in air release the object

			if (isFalling == true)
			{
			//ultsizewanted = this.GetComponent<Combat_Manager> ().playerDamage; // stores the palyer damage to send off to the object 
				ultsizewanted = playerYVelocity *-1; // time by negative to make the ult size posotive
			} else 
			{
				CmdSpawnUlt (ultSpawnLocationTransform.position, ultSpawnLocationTransform.rotation, ultsizewanted);
				UltActive = false;
			}


		}
	}

	[Command]
	private void CmdSpawnUlt(Vector3 position, Quaternion rotation, float SizeMeasurement )
	{
		// create an instance of the weapon and store a reference to its rigibody
		Rigidbody weaponInstance = Instantiate (weapon, position, rotation) as Rigidbody;






		NetworkServer.Spawn(weaponInstance.gameObject);
		weaponInstance.SendMessage("SetInitialReferences", _sourceID);
		weaponInstance.SendMessage ("getUltSize", SizeMeasurement);
		Destroy (weaponInstance, 3);
	}
}
