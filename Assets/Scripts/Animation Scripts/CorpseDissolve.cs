using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseDissolve : MonoBehaviour {

	public Renderer[] materials;
	public Rigidbody[] rbodies;
	public bool dying;
	public float deathCount = 0;

	void Death (Vector3 direction) {
		dying = true;
		foreach (Rigidbody rbody in rbodies) {
			rbody.AddForce (direction);
		}
	}

	void Start () {
		materials = GetComponentsInChildren<Renderer> ();
		rbodies = GetComponentsInChildren<Rigidbody> ();

		Death(new Vector3 (0,0,0));
	}

	void Update () {
		if (dying) {
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
