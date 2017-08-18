using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimator : MonoBehaviour {

	public Renderer[] materials;
	private float spawnCount = 0;
	public float spawnSpeed = 1;
	public float maxHeight;

	private int length = 0;

	void Start () {
		materials = GetComponentsInChildren<Renderer> ();
	}

	void Update () {
		spawnCount += Time.time * (spawnSpeed * 0.001f);
			foreach (Renderer mat in materials) {
			mat.material.SetVector ("_PlanePoint", new Vector3(this.transform.position.x, this.transform.position.y + spawnCount, this.transform.position.z));
			}
		if (spawnCount > maxHeight) {
			//Trigger player spawning
			Destroy (this.gameObject);
		}
	}
}
