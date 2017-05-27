using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	public GameObject meleehitboxObject;
	public Transform MeleeSpawn;
	public bool Attaking = false;

	public float AttackWait = 0.0f;
	public float MeleeTime = 1.0f;
	private bool CanAttack = true;

	private GameObject melee;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0) && CanAttack == true) 
		{
			Attaking = true;
			Debug.Log ("i have started my attack");
			startAttack ();
		}

		melee.transform.position = MeleeSpawn.transform.position;


	}

	void startAttack()
	{
		//var melee = (GameObject)Instantiate (meleehitboxObject, MeleeSpawn.position, MeleeSpawn.rotation,this.gameObject.transform);
		//melee =  (GameObject) Instantiate (meleehitboxObject,MeleeSpawn.position,MeleeSpawn.rotation); 
		melee =  (GameObject) Instantiate (meleehitboxObject,MeleeSpawn.position,MeleeSpawn.rotation,this.gameObject.transform); 
		CanAttack = false;
		Attaking = false;
		Destroy (melee, MeleeTime);

		StartCoroutine (attackDelay());
	}

	IEnumerator attackDelay()
	{
		yield return new WaitForSeconds (AttackWait);
		CanAttack = true;
	}
}
