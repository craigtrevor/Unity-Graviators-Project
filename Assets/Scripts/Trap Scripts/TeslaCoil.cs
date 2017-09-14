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
				CmdTakeDamage (collider.gameObject.name, 5, sourceID);
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

	IEnumerator SlowDamage() {
		damageTick = false;
		yield return new WaitForSeconds (0.2f);
		damageTick = true;
		for (int i = 0; i < affectedList.Count; i++) {
			CmdTakeDamage (affectedList[i].name, 5, sourceID);
		}
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
		networkPlayerStats.RpcTakeTrapDamage(_damage, _sourceID);
	}
}
