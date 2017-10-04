using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_StrikeColider : MonoBehaviour {
	public SinglePlayer_CombatManager combatManager;
	public Collider thisCollider;
	public Vector3 hitPoint;
	public bool airStrike;
	// Use this for initialization
	void Awake() {
		if (transform.root.tag == "Player") {
			combatManager = transform.root.GetComponent<SinglePlayer_CombatManager> ();
		}
		if (GetComponent<BoxCollider> () != null) {
			thisCollider = this.gameObject.GetComponent<BoxCollider> ();
		}
		if (GetComponent<SphereCollider> () != null) {
			thisCollider = GetComponent<SphereCollider> ();
		}
	}

	void OnTriggerEnter(Collider other) {
		//if (transform.root.tag == "Player") {
			if (other.transform.root.gameObject != this.transform.root.gameObject && other.gameObject.tag != "ThrowingSword" || other.gameObject.tag != "Sparkus_RangedWeapon" || other.gameObject.tag != "UnitD1_RangedWeapon") {
				hitPoint = thisCollider.ClosestPointOnBounds (other.transform.position);
//				Debug.Log ("Weapon hit");
				combatManager.weaponCollide (other, hitPoint, airStrike);	
			}
		//}
	}
}
