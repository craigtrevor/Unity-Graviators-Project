using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour {

	public RectTransform rectTransform;
	public float speed = 5f;
	
	// Update is called once per frame
	void Update () {
		rectTransform.position += new Vector3 (0, speed * Time.deltaTime, 0);
	}
}
