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

	public ParticleManager particleManager;

	public List<GameObject> affectedList = new List<GameObject> ();

	public float cooldownTime = 10f;
	public float maxHeal = 50f;
	public float healCounter = 0;
	public bool heal;
	public bool playParticle;
	public bool cooling;

	public GameObject ringObj;
	public GameObject baseObj;

	void Start () {
		heal = true;
		playParticle = true;
		healCounter = 0;
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
	}

	void Update () {
		if (healCounter > maxHeal && !cooling) {
			StartCoroutine (Cooldown ());
		}

		if (heal && !cooling) {
			StartCoroutine (SlowHeal ());
		}
		if (playParticle && !cooling) {
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
	void OnTriggerStay(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (affectedList, collider.gameObject)) {
				affectedList.Add (collider.gameObject);
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

	IEnumerator SlowHeal() {
		heal = false;
		for (int i = 0; i < affectedList.Count; i++) {
			if (gameObject.tag == HEALTHREGEN_TAG) {
				CmdHealthRegen (affectedList [i].name, healAmount);
			}
			if (gameObject.tag == ULTCHARGER_TAG) {
				CmdUltRegen (affectedList [i].name, healAmount);
			}
		}
		yield return new WaitForSeconds (0.2f);
		if (affectedList.Count > 0) {
			healCounter += healAmount;
		}
		if (affectedList.Count == 0) {
			healCounter -= healAmount/2f;
		}
		heal = true;
	}

	IEnumerator Cooldown() {
		affectedList.Clear ();
		cooling = true;
		baseObj.GetComponent<Renderer> ().materials[0].color = Color.black;
		baseObj.GetComponent<Renderer> ().materials [0].SetColor ("_EmissionColor", Color.black);
		ringObj.GetComponent<Renderer> ().material.color = Color.black;
		ringObj.GetComponent<Renderer> ().material.SetColor("_EmissionColor", Color.black);
		yield return new WaitForSeconds (cooldownTime);
		baseObj.GetComponent<Renderer> ().materials[0].color = Color.green;
		baseObj.GetComponent<Renderer> ().materials [0].SetColor ("_EmissionColor", Color.green);
		ringObj.GetComponent<Renderer> ().material.color = Color.green;
		ringObj.GetComponent<Renderer> ().material.SetColor("_EmissionColor", Color.green);
		healCounter = 0;
		cooling = false;
	}

	IEnumerator ParticlePlay() {
		playParticle = false;
		for (int i = 0; i < affectedList.Count; i++) {
			if (gameObject.tag == HEALTHREGEN_TAG) {
				GameObject temp = Instantiate(particleManager.GetParticle("healthPadParticle"), affectedList[i].transform.position, affectedList [i].gameObject.transform.Find ("RotationBlock").gameObject.transform.localRotation);
			}
			if (gameObject.tag == ULTCHARGER_TAG) {
				GameObject temp = Instantiate(particleManager.GetParticle("ultPadParticle"), affectedList[i].transform.position, affectedList [i].gameObject.transform.Find ("RotationBlock").gameObject.transform.localRotation);
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

		networkPlayerStats.RpcHealthFlatRegenerate(_heal, transform.name);
	}

	[Command]
	void CmdUltRegen(string _playerID, float _heal)
	{
		//Debug.Log(_playerID + "is charging.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcUltimateFlatCharging(_heal, transform.name);
	}
}
