using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageRange : MonoBehaviour {
	public int Damage = 5;
	public float rotateSpeed = 500;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.down, rotateSpeed * Time.deltaTime);
		
	}

	void OnTriggerEnter(Collider col)
	{

			Debug.Log("i have hit something and am now dissapering");
			Destroy (this.gameObject);
	}
}
