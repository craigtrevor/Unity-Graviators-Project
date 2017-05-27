using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStorage : MonoBehaviour {
	public int Damage = 0;
	public float lowdamagevelocity = 0.0f;
	public float middamagevelocity = 20.0f;
	public float highdamagevelocity = 30.0f;

	[SerializeField]
	private float speed;
	[SerializeField]
	private Transform player;
	[SerializeField]
	private Vector3 oldpos;
	[SerializeField]
	private Rigidbody RB;

	// Use this for initialization
	void Start () {
		player = transform.parent.parent;
		RB = transform.parent.parent.GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 vel = (player.transform.position - oldpos) / Time.deltaTime;
		//oldpos = player.transform.position;
		//speed = vel.magnitude;
		speed = RB.velocity.magnitude;

		if (speed < lowdamagevelocity) {
			transform.GetComponent<Renderer> ().material.color = Color.green;
			Damage = 5;
		} else if (lowdamagevelocity < speed && speed< highdamagevelocity) {
			transform.GetComponent<Renderer> ().material.color = Color.yellow;
			Damage = 10;
		} else if (highdamagevelocity <speed) {
			transform.GetComponent<Renderer> ().material.color = Color.red;
			Damage = 15;
		} else
		{
			Damage = 0;
		}
	}
}
