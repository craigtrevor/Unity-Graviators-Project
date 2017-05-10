using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {

	public Transform cameraRotation;
	int numberofdashes = 3;// change this for uses
	public int thrust = 1; //change this for speed
	public int distance = 10; // change this for distance travalled
	private float waittime = 0.1f;// the delay between movements 


	public float firerate = 1.0f; // the smaller the faster
	private float nextFire = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F) && Time.time >nextFire)
		{
			nextFire = Time.time + firerate;
			if (numberofdashes > 0) {
				charge ();
			}
		}
		
	}

	void charge()
	{


			StartCoroutine (tinydelay ());
		numberofdashes -= 1;
		Debug.Log ("i have used an charge and am now at"+ numberofdashes);
	}

	IEnumerator tinydelay()
	{	
		CharacterController controller = GetComponent<CharacterController> ();
		for(int i=0; i<distance; i++){
		controller.Move (cameraRotation.forward * thrust);
		yield return new WaitForSeconds (waittime);
		Debug.Log ("waited for " + waittime + " seconds");
		}

	}
}
