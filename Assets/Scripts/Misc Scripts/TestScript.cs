using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Application.LoadLevel("Lobby_Scene");
        SceneManager.LoadScene("JoinLAN_Scene");
    }

    void Every3Seconds(GameObject thisObject)
    {
        thisObject.transform.localScale += Vector3.one*0.1f;
    }
}
