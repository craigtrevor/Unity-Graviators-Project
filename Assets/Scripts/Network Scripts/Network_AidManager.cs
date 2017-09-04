using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_AidManager : NetworkBehaviour {

	//GameObject tags
	private const string PLAYER_TAG = "Player";
	private const string HEALTHREGEN_TAG = "HealthRegen";
	private const string ULTCHARGER_TAG = "UltCharger";

	//Scripts
	Network_PlayerManager networkPlayerManager;

	[SerializeField]
	private float healAmount = 1;
	private float chargeAmount = 15f;

	[Client]
	void OnTriggerStay (Collider other)
	{
		if (this.gameObject.tag == HEALTHREGEN_TAG && other.gameObject.tag == PLAYER_TAG)
		{
			networkPlayerManager = other.GetComponent<Network_PlayerManager>();
			//Debug.Log(other.gameObject.name);
			//Debug.Log(transform.name);
			CmdHealthRegen(other.gameObject.name, healAmount, transform.name);
		}
		/*
		if (this.gameObject.tag == ULTCHARGER_TAG && other.gameObject.tag == PLAYER_TAG)
		{
			networkPlayerManager = other.GetComponent<Network_PlayerManager>();
			Debug.Log(other.gameObject.name);
			Debug.Log(transform.name);
			CmdUltCharger(other.gameObject.name, chargeAmount, transform.name);
		}
		*/
	}

	[Command]
	void CmdHealthRegen(string _playerID, float _heal, string _sourceID)
	{
		//Debug.Log(_playerID + "is regenerating.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcHealthRegenerate(_heal, transform.name);
	}

	/*
	[Command]
	void CmdUltCharger(string _playerID, float _charge, string _sourceID)
	{
		Debug.Log(_playerID + "is charging up teh lazor.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcUltimateCharging(_charge, transform.name);
	}
	*/
}
