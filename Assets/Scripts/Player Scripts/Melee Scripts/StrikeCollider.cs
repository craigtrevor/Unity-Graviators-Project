using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeCollider : MonoBehaviour {

	public Network_CombatManager combatManager;
	public Network_Bot netBot;
	public Collider thisCollider;
	public Vector3 hitPoint;
	public bool airStrike;

	void Awake() {
		if (transform.root.tag == "Player") {
			combatManager = transform.root.GetComponent<Network_CombatManager> ();
		}
		if (transform.root.tag == "NetBot") {
			netBot = transform.root.GetComponent<Network_Bot> ();	
		}
			
		if (GetComponent<BoxCollider> () != null) {
			thisCollider = this.gameObject.GetComponent<BoxCollider> ();
		}
		if (GetComponent<SphereCollider> () != null) {
			thisCollider = GetComponent<SphereCollider> ();
		}
	}

	void OnCollisionEnter (Collision other) {
		
	}

	void OnTriggerEnter(Collider other) {
		if (transform.root.tag == "Player") {
			if (other.transform.root.gameObject != this.transform.root.gameObject && other.gameObject.tag != "ThrowingSword" && other.gameObject.tag != "Sparkus_RangedWeapon" || other.gameObject.tag != "UnitD1_RangedWeapon") {
				hitPoint = thisCollider.ClosestPointOnBounds (other.transform.position);
				combatManager.weaponCollide (other, hitPoint, airStrike);	
			}
		}
		if (transform.root.tag == "NetBot") {
			if (other.transform.root.gameObject != this.transform.root.gameObject && other.gameObject.tag != "ThrowingSword" && other.gameObject.tag != "Sparkus_RangedWeapon" || other.gameObject.tag != "UnitD1_RangedWeapon") {
				hitPoint = thisCollider.ClosestPointOnBounds (other.transform.position);
				netBot.weaponCollide (other, hitPoint, airStrike);	
			}
		}
	}
}
