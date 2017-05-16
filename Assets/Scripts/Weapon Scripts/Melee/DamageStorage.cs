using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStorage : MonoBehaviour {
	public int Damage = 0;
	public float lowdamagevelocity = 0.0f;
	public float middamagevelocity = 15.0f;
	public float highdamagevelocity = 25.0f;

	[SerializeField]
	private float speed;
	private Transform player;
	private Vector3 oldpos;

	// Use this for initialization
	void Start () {
		player = transform.parent.parent;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = (player.transform.position - oldpos) / Time.deltaTime;
		oldpos = player.transform.position;
		speed = vel.magnitude;

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
