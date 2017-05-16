﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOrbit : MonoBehaviour {


	public Transform target;
	public float orbitDistance = 10.0f;
	public float orbitDegreesPerSec = 180.0f;
	public Vector3 relativeDistance = Vector3.zero;

	// Use this for initialization
	void Start () {
		target = transform.parent.parent;


		if(target != null) 
		{
			relativeDistance = transform.position - target.position;
		}
	}

	void Orbit()
	{
		if(target != null)
		{
			// Keep us at the last known relative position
			transform.position = target.position + relativeDistance;
			transform.RotateAround(target.position, Vector3.up, orbitDegreesPerSec * Time.deltaTime);
			// Reset relative position after rotate
			relativeDistance = transform.position - target.position;
		}
	}

	void LateUpdate () 
	{
		Orbit();
	}
}
