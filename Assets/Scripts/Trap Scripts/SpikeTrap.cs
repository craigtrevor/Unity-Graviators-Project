﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpikeTrap : NetworkBehaviour {

	private string sourceID;

	public List<GameObject> affectedList = new List<GameObject> ();
	public float countDownTimer = 5f;
	public bool countingDown;
	public bool spikesOut;
	public GameObject spikes;
	public GameObject baseObj;
	public bool blinking;
	 
	void SetInitialReferences(string _sourceID)
	{
		sourceID = _sourceID;
	}

	void Start () {
		spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, -0.1f, spikes.transform.localPosition.z);
	}

	void Update () {
		for (int i = 0; i < affectedList.Count; i++) {
			if (affectedList [i].GetComponent<Network_PlayerManager> ().isDead) {
				affectedList.Remove (affectedList [i]);
			}
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
		if (!countingDown) {
			StartCoroutine (CountDown ());
			countingDown = true;
		}
		if (spikesOut) {
			KillPlayers ();
		}
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (collider.gameObject)) {
				affectedList.Add (collider.gameObject);
			}
		}
	}
		
	IEnumerator CountDown() {
		float tickDown = countDownTimer / 5f;
		float blinkAmount = 1f;

		for (int x = 0; x < 5f; x++) {
			for (int y = 0; y < blinkAmount; y++ ) {
				yield return StartCoroutine (Blink (tickDown/blinkAmount));
			}
			blinkAmount += 1f;
		}

		spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, 0.65f, spikes.transform.localPosition.z);
		KillPlayers ();
		spikesOut = true;
		baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor", Color.red * 2);
		yield return new WaitForSeconds (2f);
		spikesOut = false;
		baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor", Color.red * 0);
		while (spikes.transform.localPosition.y > -0.1f) {
			spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, spikes.transform.localPosition.y - 0.01f, spikes.transform.localPosition.z);
			yield return new WaitForSeconds (0.05f);
		}
		spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, -0.1f, spikes.transform.localPosition.z);
		countingDown = false;
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (NameInList (collider.gameObject)) {
				affectedList.Remove (collider.gameObject);
			}
		}
	}

	void KillPlayers () {
		for (int i = 0; i < affectedList.Count; i++) {
			CmdTakeDamage (affectedList[i].gameObject.name, 1000, sourceID);
		}
	}

	IEnumerator Blink(float blinkTime) {
		blinking = true;
		float tickTime = (blinkTime / 2f) / 5f;
		float emissionStrength = 0.1f;
		for (int i = 0; i < 5; i++) {
			emissionStrength += 0.2f;
			baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor", Color.red * emissionStrength);
			yield return new WaitForSeconds (tickTime);
		}
		for (int i = 0; i < 5; i++) {
			emissionStrength -= 0.2f;
			baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor",  Color.red * emissionStrength);
			yield return new WaitForSeconds (tickTime);
		}
		blinking = false;
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
		networkPlayerStats.RpcTakeTrapDamage(_damage, _sourceID);
	}
}