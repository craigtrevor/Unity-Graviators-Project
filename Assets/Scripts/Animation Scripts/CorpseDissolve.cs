using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseDissolve : MonoBehaviour {

	public Renderer[] materials;
	public Rigidbody[] rbodies;
	public bool dying;
	public int health = 1;
	public float deathCount = 0.25f;

	private int length = 0;

	void Death (Vector3 direction) {
		dying = true;

		foreach (Rigidbody rbody in rbodies) {
			rbody.AddForce (direction*100);
		}
	}

	void Start () {
		materials = GetComponentsInChildren<Renderer> ();
		rbodies = GetComponentsInChildren<Rigidbody> ();
		int length = rbodies.Length;
		Death(new Vector3 (0,1,0));
	}

	void Update () {
		if (health <= 0) 
		{
			Death(new Vector3 (0,0,0));			
		}
		if (dying) {
			rbodies = GetComponentsInChildren<Rigidbody> ();
			deathCount += Time.time * 0.001f;
			foreach (Renderer mat in materials) {
				mat.material.SetFloat ("_Cutoff", deathCount);
			}
		}
		if (deathCount >= 1) {
			Destroy (this.gameObject);
		}
	}
		
}
