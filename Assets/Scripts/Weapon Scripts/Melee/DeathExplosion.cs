using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeathExplosion : MonoBehaviour {
	public GameObject death;
	private GameObject deathExplosion;
    private ParticleSystem playdDeathParticle;
    public ParticleSystem deathParticle;
    bool particleSystemPlayed = false;
	// Use this for initialization
	void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
        testExplosion();
     
        /*if(Input.GetKeyDown(KeyCode.P))
		{
			GameObject deathExplosion =  (GameObject) Instantiate (death,this.transform.position,this.transform.rotation);
            ParticleSystem playParticle = (ParticleSystem) Instantiate(deathParticle, this.transform.position, this.transform.rotation);
            playParticle.Play();
            Destroy (deathExplosion, 5);
			Destroy (this.gameObject);


		}*/

    }
  void testExplosion() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject deathExplosion = (GameObject)Instantiate(death, this.transform.position, this.transform.rotation);
            ParticleSystem playDeathParticle = (ParticleSystem)Instantiate(deathParticle, this.transform.position, this.transform.rotation);
            if (!particleSystemPlayed) { 
                playDeathParticle.Emit(0);
                particleSystemPlayed = true;
        }
            if (particleSystemPlayed == true)
            {
                Destroy(playDeathParticle);
            }
        Destroy(deathExplosion, 5);
        Destroy(this.gameObject);
}
        
}

void OnTriggerEnter (Collider col)
	{
		GameObject deathExplosion =  (GameObject) Instantiate (death,this.transform.position,this.transform.rotation); 
		Destroy (deathExplosion, 5);
		Destroy (this.gameObject);
        ParticleSystem playDeathParticle = (ParticleSystem)Instantiate(deathParticle, this.transform.position, this.transform.rotation);
        playDeathParticle.Emit(0);
        particleSystemPlayed = true;

        //		if (col.tag == "dashHitbox") 
        //		{
        //			GameObject deathexplosion =  (GameObject) Instantiate (death,this.transform.position,this.transform.rotation); 
        //			Destroy (deathexplosion, 5);
        //			Destroy (this.gameObject);
        //
        //		}
        //
        //		if (col.tag == "ThrowingSword") 
        //		{
        //			GameObject deathexplosion =  (GameObject) Instantiate (death,this.transform.position,this.transform.rotation); 
        //			Destroy (deathexplosion, 5);
        //			Destroy (this.gameObject);
        //		}
    }
}
