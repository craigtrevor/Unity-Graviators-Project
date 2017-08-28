using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTesterSpawner : MonoBehaviour {

	public GameObject weaponToHide;
	public GameObject weaponToHide2;

	public void RangedAttack (AnimationEvent animEvent) {
		weaponToHide.SetActive(false);
		weaponToHide2.SetActive(false);
	}

	public void NoNameShowWeapons (AnimationEvent animEvent) {
		weaponToHide.SetActive(true);
		weaponToHide2.SetActive(true);
	}

	public void ActualJump () {
		this.GetComponent<Rigidbody> ().velocity = new Vector3(0,5,0);
	}
}
