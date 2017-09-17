using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

	public float delayBeforeDeath = 5f;

    private void Start()
    {
		Destroy(gameObject, delayBeforeDeath);
    }
}
