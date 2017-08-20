using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneParticles : MonoBehaviour {

	public ParticleSystem healthPadParticle;
	private ParticleSystem playHealthPadParticle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		PlayHealthParticle ();

}
	void PlayHealthParticle () {
		ParticleSystem playHealthPadParticle = (ParticleSystem)Instantiate(healthPadParticle,this.transform.position, this.transform.rotation);
		playHealthPadParticle.Emit(1);
}
}
