using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour {
    public ParticleSystem GravityLand;
    //private ParticleSystem gravityLand;

	// Use this for initialization
	void Start () {
        GravityLand = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  /*  void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "collider")
        {
            GravityLand = Instantiate (GravityLand, transform.position, Quaternion.identity) as ParticleSystem;
           
            GravityLand.Play();
        }
       
    }*/

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            //Instantiate your particle system here.
            Instantiate(GravityLand, contact.point, Quaternion.identity);
        }
    }
}
