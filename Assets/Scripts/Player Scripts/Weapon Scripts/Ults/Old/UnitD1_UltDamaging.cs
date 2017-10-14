using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitD1_UltDamaging : NetworkBehaviour {


	private string sourceID;

	private Collider[] hitColliders;
	private Vector3 attackOffset;
	private float attackRadius;
	private const string PLAYER_TAG = "Player";



	public float Damage = 100f; 
	public float  MinSize = 0.5f;
	public float MaxSize;
	public float GrowFactor = 5f;

	// scripts
	Network_PlayerManager networkPlayerManager;
	PlayerController playerController;


	void SetInitialReferences(string _sourceID)
	{
		sourceID = _sourceID;
	}
		
	public void getUltSize (float sizeMeasurement)
	{
		//Debug.Log("i have reciced info and the size measurement is " +  sizeMeasurement);
		MaxSize = sizeMeasurement / 2;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.x < MaxSize) 
		{
			GrowFactor = Mathf.Abs (GrowFactor);
			ApplyScaleRate (); // only grow bigger if smaller then max size
		}
			else if (transform.localScale.x > MaxSize) 
		{
			//Debug.Log ("i have reached max size at " + MaxSize);
			//Destroy (this.gameObject); // not destorying for testing
		}

		//ApplyScaleRate ();
	}


	[Client]
	void onTriggerEnter (Collider other)
	{
		if(other.tag == "Player")
		{
			foreach (Collider hitCol in hitColliders)
			{
				if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && hitCol.transform.name != sourceID)
				{
					//Debug.Log ("i have hit another player");
					CmdTakeDamage(hitCol.gameObject.name, Damage, sourceID);
				}
			}

		}
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		//Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}
		
	void ApplyScaleRate()
	{
		transform.localScale += Vector3.one * GrowFactor;
	}
}
