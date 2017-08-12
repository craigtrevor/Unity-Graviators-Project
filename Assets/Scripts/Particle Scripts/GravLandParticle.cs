using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravLandParticle : MonoBehaviour {

	private ParticleSystem playGravLand;
	public ParticleSystem gravLandParticle;
	bool gravParticleSystemPlayed = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter (Collider col)
	{
		if(col.tag == "collider"){
			ParticleSystem playGravLand = (ParticleSystem)Instantiate(gravLandParticle,this.transform.position + Vector3.down, this.transform.rotation);

			if (!gravParticleSystemPlayed)
			{
				{
					playGravLand.Emit(1);
					gravParticleSystemPlayed = true;
					//Debug.Log("gravPlayed");
				}
				if (gravParticleSystemPlayed == true)
				{
					Destroy(playGravLand);
					//Debug.Log("gravDead");

				}
		}

}
}
}