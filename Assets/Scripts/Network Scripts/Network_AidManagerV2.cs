using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_AidManagerV2 : NetworkBehaviour {

	//GameObject tags
	private const string PLAYER_TAG = "Player";
	private const string HEALTHREGEN_TAG = "HealthRegen";
	private const string ULTCHARGER_TAG = "UltCharger";

	//Scripts
	Network_PlayerManager networkPlayerManager;

	[SerializeField]
	private float healAmount = 1;
	private float chargeAmount = 15f;

	public GameObject healthPadParticle;
	public GameObject ultPadParticle;

	public List<GameObject> affectedList = new List<GameObject> ();
	public bool heal;
	public bool playParticle;

	void Start () {
		heal = true;
		playParticle = true;
	}

	void Update () {
		if (heal) {
			StartCoroutine (SlowHeal ());
		}
		if (playParticle) {
			StartCoroutine (ParticlePlay ());
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
	
	[Client]
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (collider.gameObject)) {
				Debug.Log ("added");
				affectedList.Add (collider.gameObject);
			}
		}
	}

	[Client]
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (NameInList (collider.gameObject)) {
				Debug.Log ("removed");
				affectedList.Remove (collider.gameObject);
			}
		}
	}

	IEnumerator SlowHeal() {
		heal = false;
		yield return new WaitForSeconds (0.2f);
		heal = true;
		for (int i = 0; i < affectedList.Count; i++) {
			if (gameObject.tag == HEALTHREGEN_TAG) {
				CmdHealthRegen (affectedList [i].name, 5);
			}
			if (gameObject.tag == ULTCHARGER_TAG) {
				CmdUltRegen (affectedList [i].name, 5);
			}
		}
	}

	IEnumerator ParticlePlay() {
		playParticle = false;
		yield return new WaitForSeconds (0.5f);
		playParticle = true;
		for (int i = 0; i < affectedList.Count; i++) {
			if (gameObject.tag == HEALTHREGEN_TAG) {
				GameObject temp = Instantiate(healthPadParticle, affectedList[i].transform.position, Quaternion.identity);
				temp.transform.SetParent(affectedList[i].gameObject.transform);
			}
			if (gameObject.tag == ULTCHARGER_TAG) {
				GameObject temp = Instantiate(ultPadParticle, affectedList[i].transform.position, Quaternion.identity);
				temp.transform.SetParent(affectedList[i].gameObject.transform);
			}
		}
	}

	[Command]
	void CmdHealthRegen(string _playerID, float _heal)
	{
		//Debug.Log(_playerID + "is regenerating.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcHealthRegenerate(_heal, transform.name);
	}

	[Command]
	void CmdUltRegen(string _playerID, float _heal)
	{
		//Debug.Log(_playerID + "is charging.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcUltimateCharging(_heal, transform.name);
	}
}
