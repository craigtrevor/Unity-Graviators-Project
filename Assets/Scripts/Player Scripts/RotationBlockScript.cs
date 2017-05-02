using UnityEngine;
using System.Collections;

public class RotationBlockScript : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("PlayerModel");
	
	}
	
	// Update is called once per frame
	void Update () {

        //transform.position = player.transform.position; //Move rotation block to player position
	
	}

    //Update position to same as player position
    public void UpdatePosition() {

        transform.position = player.transform.position;

    }
}
