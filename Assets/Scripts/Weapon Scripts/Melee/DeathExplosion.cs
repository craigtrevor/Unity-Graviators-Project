using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathExplosion : MonoBehaviour {
	public GameObject death;
	private GameObject deathexplosion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
		{
			GameObject deathexplosion =  (GameObject) Instantiate (death,this.transform.position,this.transform.rotation); 
			Destroy (deathexplosion, 5);
			Destroy (this.gameObject);

		}
		
	}

	void OnTriggerEnter (Collider col)
	{
		GameObject deathexplosion =  (GameObject) Instantiate (death,this.transform.position,this.transform.rotation); 
		Destroy (deathexplosion, 5);
		Destroy (this.gameObject);
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
