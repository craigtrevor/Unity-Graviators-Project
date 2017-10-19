using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTesterSpawner : MonoBehaviour {

	public AnimTesterController animTestController;

	public GameObject weaponToHide;
	public GameObject weaponToHide2;
	public MeleeWeaponTrail trailToHide;
	public MeleeWeaponTrail trailToHide2;

	public GameObject reloadBall;
	public bool noName;
	public bool sparkus;
	public bool d1;

	public void RangedAttack (AnimationEvent animEvent) {
		if (noName) {
			weaponToHide.SetActive (false);
			weaponToHide2.SetActive (false);
			trailToHide.enabled = false;
			trailToHide2.enabled = false;
		}
		if (sparkus) {
			reloadBall.SetActive (false);
		}
		animTestController.SpawnRanged ();
	}

	public void NoNameShowWeapons (AnimationEvent animEvent) {
		weaponToHide.SetActive(true);
		weaponToHide2.SetActive(true);
		trailToHide.enabled = true;
		trailToHide2.enabled = true;
	}

	public void ActualJump () {
		if (animTestController.d1Ulting) {
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 7, 0);
		} else {
			this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 5, 0);
		}
		//this.GetComponent<Rigidbody> ().AddForce(new Vector3(0,30,0));
	}
}
