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
	private float healAmount = 1f;
	private float chargeAmount = 15f;

	public ParticleManager particleManager;

	public List<GameObject> affectedList = new List<GameObject> ();
	public List<GameObject> blackList = new List<GameObject> ();

	public float healTimeOut = 10f;
	public float blackListTimeOut = 10f;
	public bool heal;
	public bool playParticle;

	void Start () {
		heal = true;
		playParticle = true;
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
	}

	void Update () {
		if (heal) {
			StartCoroutine (SlowHeal ());
		}
		if (playParticle) {
			StartCoroutine (ParticlePlay ());
		}

	}
			
	public bool NameInList(List<GameObject> listToCheck, GameObject toCheck) {
		for (int i = 0; i < listToCheck.Count; i++) {
			if (listToCheck [i] == toCheck) {
				return true;
			}
		} 
		return false;
	}
	
	[Client]
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (affectedList, collider.gameObject) && !NameInList (blackList, collider.gameObject)) {
				affectedList.Add (collider.gameObject);
				BlackListTimer (collider.gameObject);
			}
		}
	}

	[Client]
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (NameInList (affectedList, collider.gameObject)) {
				affectedList.Remove (collider.gameObject);
			}
		}
	}

	IEnumerator BlackListTimer (GameObject player) {
		yield return new WaitForSeconds (healTimeOut);
		blackList.Add (player);
		if (NameInList (affectedList, player)) {
			affectedList.Remove (player);
		} 
		yield return new WaitForSeconds (blackListTimeOut);
		if (NameInList (blackList, player)) {
			blackList.Remove (player);
		} 


	}

	IEnumerator SlowHeal() {
		heal = false;
		for (int i = 0; i < affectedList.Count; i++) {
			if (gameObject.tag == HEALTHREGEN_TAG) {
				CmdHealthRegen (affectedList [i].name, healAmount);
			}
			if (gameObject.tag == ULTCHARGER_TAG) {
				CmdUltRegen (affectedList [i].name, chargeAmount);
			}
		}
		yield return new WaitForSeconds (0.2f);
		heal = true;
	}

	IEnumerator ParticlePlay() {
		playParticle = false;
		for (int i = 0; i < affectedList.Count; i++) {
			if (gameObject.tag == HEALTHREGEN_TAG) {
				GameObject temp = Instantiate(particleManager.GetParticle("healthPadParticle"), affectedList[i].transform.position, affectedList[i].transform.rotation);
				temp.transform.SetParent(affectedList[i].gameObject.transform);
			}
			if (gameObject.tag == ULTCHARGER_TAG) {
				GameObject temp = Instantiate(particleManager.GetParticle("ultPadParticle"), affectedList[i].transform.position, affectedList[i].transform.rotation);
				temp.transform.SetParent(affectedList[i].gameObject.transform);
			}
		}
		yield return new WaitForSeconds (0.5f);
		playParticle = true;
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
