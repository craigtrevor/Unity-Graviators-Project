using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour {

	public GameObject weaponPrefab;
	public Transform cameraRotation;
	public Transform weaponSpawn;
	[SerializeField]
	private int numberofWeapons = 3;
	public int weaponLimit = 3;
	[SerializeField]
	private bool canfire = true;

	public int thrust = 2000;

	//firerate and bool to control firerate
	public float firerate = 0.5f; // the smaller the faster
	private float nextFire = 1.0f;
	public float Life = 2.0f;


	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () {

        if (UI_PauseMenu.IsOn == true)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time >nextFire)
		{
			nextFire = Time.time + firerate;
			if (numberofWeapons > 0 && canfire == true) {
				Fire ();
			}
		}
	}

	IEnumerator spawnDelay()
	{
		for (int i = 0; i < weaponLimit; i++) 
		{
			yield return new WaitForSeconds (1);
			Spawn ();
		}	

		canfire = true;

	}

	void Spawn()
	{
		//var meelee = (GameObject)Instantiate (weaponPrefab, weaponSpawn.position, weaponSpawn.rotation);
		//var meelee = (GameObject)Instantiate(weaponPrefab,weaponSpawn.position,weaponSpawn.rotation,this.gameObject.transform); // undo this to get a visual to spawn

		numberofWeapons += 1;
		Debug.Log ("i have gained a weapon and am now at"+ numberofWeapons);

	}

	void Fire()
	{
		var meelee = (GameObject)Instantiate (weaponPrefab, weaponSpawn.position, cameraRotation.rotation);
		meelee.GetComponent<Rigidbody>().AddForce (cameraRotation.forward * thrust);



			numberofWeapons -= 1;
			Debug.Log ("i have fired a weapon and am now at"+ numberofWeapons);
			Destroy (meelee, Life);

		if (numberofWeapons == 0)
		{
			canfire = false;
			StartCoroutine (spawnDelay ());	
		}
	}
}
