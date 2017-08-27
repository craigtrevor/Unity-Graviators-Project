using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPassalong : MonoBehaviour {

	public GameObject parentObject;

	void RangedAttack (AnimationEvent animEvent) {
		parentObject.GetComponent<WeaponSpawn> ().RangedAttack (animEvent);
	}

	void NoNameShowWeapons (AnimationEvent animEvent) {
		parentObject.GetComponent<WeaponSpawn> ().NoNameShowWeapons (animEvent);
	}
}
