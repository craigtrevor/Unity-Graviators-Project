using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Range : MonoBehaviour {
	public GameObject RangePrefab;
	public Transform fireTransform; // a child of the player where the weapon is spawned
	public Transform secondaryFireTransform; // second position for no name
	public float force = 2000; // the force to be applied to the weapon
	public float reloadTime = 5f;
	public float rangeLife = 2.0f;
	public bool canfire = true;
	public bool reloading = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canfire == true)
		{
			fire (fireTransform.forward,force,fireTransform.position);
		}

		if (canfire == false) 
		{
			if (reloading == false) 
			{
				reloading = true;
				StartCoroutine (reload ());
			}
		}
	}

	private void fire(Vector3 forward, float launchForce, Vector3 position)
	{
		
		canfire = false;
		var rangeattack = (GameObject)Instantiate (RangePrefab, fireTransform.position, fireTransform.rotation);
		rangeattack.GetComponent<Rigidbody> ().AddForce (Vector3.forward * force *-1); // *-1 to correct direciton

		Destroy(rangeattack, rangeLife);
	}

	IEnumerator reload()
	{
		yield return new WaitForSeconds (reloadTime);
		reloading = false;
		canfire = true;

	}
}
