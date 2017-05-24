using UnityEngine;
using System.Collections;

public class GravityBlockScript : MonoBehaviour {

    public GameObject controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //transform.position = player.transform.position; //Move rotation block to player position
	
	}

    //Update position to same as controller position
    public void UpdatePosition() {

        if (UI_PauseMenu.isOn)
            return;

        transform.position = controller.transform.position;

    }
}
