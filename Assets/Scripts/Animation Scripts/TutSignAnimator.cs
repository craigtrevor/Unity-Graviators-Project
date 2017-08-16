using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutSignAnimator : MonoBehaviour {

	public Material tutSignMat;
	public List<Texture> textureList = new List<Texture>();
	public int currentTexture = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.E)) {
			if (currentTexture < textureList.Count) {
				currentTexture += 1;
			}
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			if (currentTexture != 0) {
				currentTexture -= 1;
			}
		}
		tutSignMat.mainTexture = textureList [currentTexture];
	}
}
