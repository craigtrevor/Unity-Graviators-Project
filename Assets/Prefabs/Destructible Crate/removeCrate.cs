using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeCrate : MonoBehaviour {


	public Renderer[] materials;
	public Rigidbody[] rbodies;
	public bool dying;
	public float health = 1;
	public float deathCount = 0.25f;

	[SerializeField] private float fadePS = 0.5f;



	private int length = 0;

	void Death (Vector3 direction) {
		dying = true;

		foreach (Rigidbody rbody in rbodies) {
			rbody.useGravity = false;
			rbody.AddForce (direction*100);

			//rbody.detectCollisions = false;
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
			Destroy (this.gameObject, 5f);
		}
		if (dying) {
			rbodies = GetComponentsInChildren<Rigidbody> ();
			deathCount += Time.time * 0.0001f;
			health = health - 0.05f;
			foreach (Renderer mat in materials) {
				var mattochange = mat.GetComponent<Renderer> ().material;
				var color = mattochange.color;
				mattochange.color = new Color (color.r, color.g, color.b, color.a - (fadePS * Time.deltaTime));
					
			}
		}
	}

}
