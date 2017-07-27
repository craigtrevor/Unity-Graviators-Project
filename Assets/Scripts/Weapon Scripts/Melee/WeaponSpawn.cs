using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSpawn : NetworkBehaviour {

	public int m_PlayerNumber = 1;
	public Rigidbody weapon; // prefab of the weapon
	public Transform fireTransform; // a child of the player where the weapon is spawned
	public float force = 2000; // the force to be applied to the weapon
	public float reloadTime = 1f;
	[SyncVar]
	public int m_localID;

	//private string m_FireButton; // the input axis that is used for firing
	private Rigidbody m_Rigidbody; // reference to the rigidbody
	[SerializeField]
	private bool m_Fired = false;          // Whether or not the weapon has been launched with this button press.

//	public GameObject weaponPrefab;
//	public Transform cameraRotation;
//	public Transform weaponSpawn;
//	[SerializeField]
//	private int numberofWeapons = 3;
//	public int weaponLimit = 3;
//	[SerializeField]
//	private bool canfire = true;
//
//	public int thrust = 2000;
//
//	//firerate and bool to control firerate
//	public float firerate = 0.5f; // the smaller the faster
//	private float nextFire = 1.0f;
//	public float Life = 2.0f;

	private void Awake()
	{
		//set up the refences.
		m_Rigidbody = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start () 
	{

	}

	[ClientCallback]
	private void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (Input.GetKeyDown (KeyCode.Mouse1) && m_Fired == false) 
		{
			Fire ();
			StartCoroutine (reload ());
		} 
	}

	private void Fire()
	{
		m_Fired = true; // set the fire flag so that fire is only called once

		CmdFire(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
	}

	[Command]
	private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
	{
		// create an instance of the weapon and store a reference to its rigibody
		Rigidbody weaponInstance = Instantiate (weapon, position, rotation) as Rigidbody;
		// Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
		Vector3 velocity = rigidbodyVelocity + launchForce * forward;

		// Set the shell's velocity to this velocity.
		weaponInstance.velocity = velocity;

		NetworkServer.Spawn(weaponInstance.gameObject);
		Destroy (weaponInstance, 3);
	}

	IEnumerator reload()
	{
		// delay before the player can fire agian
		yield return new  WaitForSeconds (reloadTime); 
		m_Fired = false;
	}
//	// Update is called once per frame
//	void Update () {
//
//        if (UI_PauseMenu.IsOn == true)
//            return;
//
//        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time >nextFire)
//		{
//			nextFire = Time.time + firerate;
//			if (numberofWeapons > 0 && canfire == true) {
//				Fire ();
//			}
//		}
//	}
//
//	IEnumerator spawnDelay()
//	{
//		for (int i = 0; i < weaponLimit; i++) 
//		{
//			yield return new WaitForSeconds (1);
//			Spawn ();
//		}	
//
//		canfire = true;
//
//	}
//
//	void Spawn()
//	{
//		//var meelee = (GameObject)Instantiate (weaponPrefab, weaponSpawn.position, weaponSpawn.rotation);
//		//var meelee = (GameObject)Instantiate(weaponPrefab,weaponSpawn.position,weaponSpawn.rotation,this.gameObject.transform); // undo this to get a visual to spawn
//
//		numberofWeapons += 1;
//		Debug.Log ("i have gained a weapon and am now at"+ numberofWeapons);
//
//	}
//
//	void Fire()
//	{
//		var meelee = (GameObject)Instantiate (weaponPrefab, weaponSpawn.position, cameraRotation.rotation);
//		meelee.GetComponent<Rigidbody>().AddForce (cameraRotation.forward * thrust);
//
//
//
//			numberofWeapons -= 1;
//			Debug.Log ("i have fired a weapon and am now at"+ numberofWeapons);
//			Destroy (meelee, Life);
//
//		if (numberofWeapons == 0)
//		{
//			canfire = false;
//			StartCoroutine (spawnDelay ());	
//		}
//	}
}
