using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TeslaCoil : NetworkBehaviour {

	private string sourceID;

	public GameObject StartCoil;
	public GameObject EndCoil;
	public LineRenderer lineRend;
	public LightningLine lightningLine;
	public BoxCollider boxCollider;
	public bool Shocking;
	public bool damageTick;
	public List<GameObject> affectedList = new List<GameObject> ();
	public List<GameObject> affectedBotList = new List<GameObject> ();

	void SetInitialReferences(string _sourceID)
	{
		sourceID = _sourceID;
	}
		
	void Start () {
		lightningLine.target = StartCoil.transform.localPosition;
		boxCollider.enabled = false;
	}

	void Update () {
		if (!Shocking) {
			StartCoroutine(Shock());
		}
		if (damageTick) {
			StartCoroutine (SlowDamage ());
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

	IEnumerator Shock() {
		Shocking = true;
		damageTick = true;
		yield return new WaitForSeconds (Random.Range(3f,10f));
		lightningLine.target = EndCoil.transform.localPosition;
		boxCollider.enabled = true;
		yield return new WaitForSeconds (5f);
		Shocking = false;
		damageTick = false;
		lightningLine.target = StartCoil.transform.localPosition;
		boxCollider.enabled = false;
		StopAllCoroutines ();
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
				CmdTakeDamage (collider.gameObject.name, 5, sourceID);
			}
		}
		if (collider.gameObject.tag == "NetBot") {
			if (!NameInList (collider.gameObject, affectedBotList)) {
				affectedBotList.Add (collider.gameObject);
				collider.gameObject.GetComponent<Network_Bot>().TakeTrapDamage(5);
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

	IEnumerator SlowDamage() {
		damageTick = false;
		yield return new WaitForSeconds (0.2f);
		damageTick = true;
		for (int i = 0; i < affectedList.Count; i++) {
			CmdTakeDamage (affectedList[i].name, 5, sourceID);
		}
		for (int i = 0; i < affectedBotList.Count; i++) {
			affectedBotList[i].GetComponent<Network_Bot>().TakeTrapDamage(5);
		}
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
		networkPlayerStats.RpcTakDamageByTrap(_damage, _sourceID);
	}
}
