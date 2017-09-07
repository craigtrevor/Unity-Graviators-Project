using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeCollider : MonoBehaviour {

	public Network_CombatManager combatManager;
	private Collider thisCollider;
	public Vector3 hitPoint;
	public bool airStrike;

	void Start() {
		combatManager = transform.root.GetComponent<Network_CombatManager> ();
		if (GetComponent<BoxCollider> () != null) {
			thisCollider = GetComponent<BoxCollider> ();
		}
		if (GetComponent<SphereCollider> () != null) {
			thisCollider = GetComponent<SphereCollider> ();
		}
	}
		
	void OnTriggerEnter(Collider other) {
			if (other.transform.root.gameObject != this.transform.root.gameObject) {
				hitPoint = thisCollider.ClosestPointOnBounds (other.transform.position);
				combatManager.weaponCollide (other, hitPoint, airStrike);	
			}
		}
}
