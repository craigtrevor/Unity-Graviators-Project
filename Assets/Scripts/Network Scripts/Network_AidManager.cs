using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_AidManager : NetworkBehaviour {

	//GameObject tags
	private const string PLAYER_TAG = "Player";
	private const string HEALTHREGEN_TAG = "HealthRegen";

	//Scripts
	Network_PlayerManager networkPlayerManager;

	[SerializeField]
	private float healAmount = 100;

	[Client]
	void OnTriggerEnter (Collider other)
	{
		if (this.gameObject.tag == HEALTHREGEN_TAG && other.gameObject.tag == PLAYER_TAG)
		{
			networkPlayerManager = other.GetComponent<Network_PlayerManager>();
			Debug.Log(other.gameObject.name);
			Debug.Log(transform.name);
			CmdHealthRegen(other.gameObject.name, healAmount, transform.name);
		}
	}

	[Command]
	void CmdHealthRegen(string _playerID, float _heal, string _sourceID)
	{
		Debug.Log(_playerID + "is regenerating.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcHealthRegenerate(_heal, transform.name);
	}
}
