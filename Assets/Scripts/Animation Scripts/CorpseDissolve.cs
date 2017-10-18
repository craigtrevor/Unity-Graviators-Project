using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseDissolve : MonoBehaviour {

	public Renderer[] materials;
	public Rigidbody[] rbodies;
    public Collider[] colliders;
	public bool dying;
	public int health = 1;
	public float deathCount = 0.25f;

	private int length = 0;

	void Death () {
		dying = true;
        health = 0;

        if (gameObject.activeSelf)
        {
            foreach (Rigidbody rbody in rbodies)
            {
                rbody.AddExplosionForce(100f, Vector3.zero, 10f);
            }

            foreach (Collider collider in colliders)
            {
                Physics.IgnoreLayerCollision(11, 13);
                Physics.IgnoreLayerCollision(11, 14);
            }
        }
	}

	void Start () {
		materials = GetComponentsInChildren<Renderer> ();
		rbodies = GetComponentsInChildren<Rigidbody> ();
		int length = rbodies.Length;
		Death();
	}

	void Update () {
		if (health <= 0) 
		{
			Death();
			Destroy (this.gameObject, 10f);
		}
		if (dying) {
			rbodies = GetComponentsInChildren<Rigidbody> ();
			deathCount += Time.time * 0.0001f;
			foreach (Renderer mat in materials) {
				mat.material.SetFloat ("_Cutoff", deathCount);
			}
		}
	}
		
}
