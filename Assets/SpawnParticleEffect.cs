using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticleEffect : MonoBehaviour {
	private ParticleSystem playSparkusRanged;
	public ParticleSystem sparkusRanged;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		playParticleEffect ();
		
	}
	void playParticleEffect () {
		ParticleSystem playSparkusRanged = (ParticleSystem)Instantiate (sparkusRanged, this.transform.position, this.transform.rotation);
		playSparkusRanged.Emit (1);
	}
}
