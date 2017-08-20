using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class streamVideo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject camera = GameObject.Find ("Raw Image");
		var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer> ();
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
		videoPlayer.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
