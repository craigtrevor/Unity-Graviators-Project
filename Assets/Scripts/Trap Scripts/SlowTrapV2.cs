using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrapV2 : MonoBehaviour {

	public bool damageTick;
	public List<GameObject> affectedList = new List<GameObject> ();

	void Start () {
		damageTick = true;
	}

	void Update () {
		if (damageTick) {
			StartCoroutine (Slow ());
		}
	}

	public bool NameInList(GameObject toCheck) {
		for (int i = 0; i < affectedList.Count; i++) {
			if (affectedList [i] == toCheck) {
				return true;
			}
		} 
		return false;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (collider.gameObject)) {
				affectedList.Add (collider.gameObject);
				collider.gameObject.GetComponent<Network_CombatManager> ().SlowForSeconds (1f);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (NameInList (collider.gameObject)) {
				affectedList.Remove (collider.gameObject);
			}
		}
	}

	IEnumerator Slow() {
		damageTick = false;
		for (int i = 0; i < affectedList.Count; i++) {
			affectedList[i].gameObject.GetComponent<Network_CombatManager> ().SlowForSeconds (1f);
		}
		yield return new WaitForSeconds (0.1f);
		damageTick = true;
	}
}
