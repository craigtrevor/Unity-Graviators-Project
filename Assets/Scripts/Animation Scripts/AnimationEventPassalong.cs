using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPassalong : MonoBehaviour {

	public GameObject parentObject;
	public GameObject controllerObject;

	public bool AnimTester = false;

	void RangedAttack (AnimationEvent animEvent) {
		if (!AnimTester) {
			parentObject.GetComponent<WeaponSpawn> ().RangedAttack (animEvent);
		} else {
			this.GetComponent<AnimTesterSpawner>().RangedAttack (animEvent);
		}
	}

	void NoNameShowWeapons (AnimationEvent animEvent) {
		if (!AnimTester) {
		parentObject.GetComponent<WeaponSpawn> ().NoNameShowWeapons (animEvent);
		} else {
			this.GetComponent<AnimTesterSpawner>().NoNameShowWeapons (animEvent);
		}
	}

	void ActualJump (AnimationEvent animEvent) {
		if (!AnimTester) {
			controllerObject.GetComponent<PlayerController> ().ActualJump ();
		} else {
			this.GetComponent<AnimTesterSpawner>().ActualJump ();
		}
	}
}
