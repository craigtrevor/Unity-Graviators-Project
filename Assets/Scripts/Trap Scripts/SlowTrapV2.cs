using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrapV2 : MonoBehaviour {

	public bool damageTick;
	public List<GameObject> affectedList = new List<GameObject> ();
	public List<GameObject> affectedBotList = new List<GameObject> ();

	void Start () {
		damageTick = true;
	}

	void Update () {
		if (damageTick) {
			StartCoroutine (Slow ());
		}
		for (int i = 0; i < affectedList.Count; i++) {
			if (affectedList [i].GetComponent<Network_PlayerManager> ().isDead) {
				affectedList.Remove (affectedList [i]);
			}
		}
		for (int i = 0; i < affectedBotList.Count; i++) {
			if (!affectedBotList [i].gameObject.activeSelf) {
				affectedBotList.Remove (affectedBotList [i]);
			}
		}
	}

	public bool NameInList(GameObject toCheck, List<GameObject> listToCheck) {
		for (int i = 0; i < listToCheck.Count; i++) {
			if (listToCheck [i] == toCheck) {
				return true;
			}
		} 
		return false;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (collider.gameObject, affectedList)) {
				affectedList.Add (collider.gameObject);
				collider.gameObject.GetComponent<Network_CombatManager> ().SlowForSeconds (1f);
			}
		}
		if (collider.gameObject.tag == "NetBot") {
			if (!NameInList (collider.gameObject, affectedBotList)) {
				affectedBotList.Add (collider.gameObject);
				collider.gameObject.GetComponent<Network_Bot> ().Slow (1f);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (NameInList (collider.gameObject, affectedList)) {
				affectedList.Remove (collider.gameObject);
			}
		}
		if (collider.gameObject.tag == "NetBot") {
			if (NameInList (collider.gameObject, affectedBotList)) {
				affectedBotList.Remove (collider.gameObject);
			}
		}
	}

	IEnumerator Slow() {
		damageTick = false;
		for (int i = 0; i < affectedList.Count; i++) {
			affectedList[i].gameObject.GetComponent<Network_CombatManager> ().SlowForSeconds (1f);
		}
		for (int i = 0; i < affectedBotList.Count; i++) {
			affectedBotList [i].gameObject.GetComponent<Network_Bot> ().Slow (1f);
		}
		yield return new WaitForSeconds (0.1f);
		damageTick = true;
	}
}
