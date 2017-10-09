using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ResetVideo : MonoBehaviour {

	public VideoPlayer videoPlayer;
	public GameObject button;

	void Update () {
		if (videoPlayer.time == videoPlayer.clip.length) {
			Reset ();
		}
	}

	void Reset () {
		videoPlayer.Stop();
		button.SetActive(true);
	}
}
