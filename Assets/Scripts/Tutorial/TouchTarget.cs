using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTarget : MonoBehaviour {

	public GameObject tutManager;

	// Use this for initialization
	void OnCollisonEnter () {
		tutManager.GetComponent<TutorialManager> ().tutProgression += 1;
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
