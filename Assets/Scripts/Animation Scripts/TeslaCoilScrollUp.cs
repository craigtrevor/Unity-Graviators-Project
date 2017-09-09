using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoilScrollUp : MonoBehaviour {

	public Renderer rend;
	public Renderer rend2;
	public float scroll;

	void Update () {
		scroll -= Time.deltaTime;
		if (rend != null) {
			rend.material.SetTextureOffset ("_MainTex", new Vector2 (0, scroll));
		}
		if (rend2 != null) {
			rend2.material.SetTextureOffset ("_MainTex", new Vector2 (0, scroll - Time.deltaTime));
		}
	}
}
