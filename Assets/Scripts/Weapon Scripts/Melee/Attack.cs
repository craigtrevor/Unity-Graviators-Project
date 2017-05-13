using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	public GameObject meleehitboxObject;
	public Transform MeleeSpawn;

	public float AttackWait = 2.0f;
	public float MeleeTime = 1.0f;
	private bool CanAttack = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0) && CanAttack == true) 
		{
			Debug.Log ("i have started my attack");
			startAttack ();
		}

		
	}

	void startAttack()
	{
		var melee = (GameObject)Instantiate (meleehitboxObject, MeleeSpawn.position, MeleeSpawn.rotation,this.gameObject.transform);
		CanAttack = false;
		Destroy (melee, MeleeTime);

		StartCoroutine (attackDelay());
	}

	IEnumerator attackDelay()
	{
		yield return new WaitForSeconds (AttackWait);
		CanAttack = true;
	}
}
