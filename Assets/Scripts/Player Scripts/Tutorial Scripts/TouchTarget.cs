using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTarget : MonoBehaviour {

	public GameObject tutManager;

	void OnTriggerEnter () {
		tutManager.GetComponent<TutorialManager> ().tutProgression += 1;
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
