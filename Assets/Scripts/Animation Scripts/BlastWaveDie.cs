using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWaveDie : MonoBehaviour {
	
	public Animator playerAnimator;

	void Update () {
		if (playerAnimator.GetCurrentAnimatorStateInfo (0).IsName ("End")) {
			Destroy(this.gameObject);
		}
	}
}
