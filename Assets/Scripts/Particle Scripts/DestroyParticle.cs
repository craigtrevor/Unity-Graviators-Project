using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    private void Start()
    {
		Destroy(gameObject, 5f);
        //Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}
