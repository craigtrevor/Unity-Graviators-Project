using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class damageRange : NetworkBehaviour {
	public int Damage = 5;
	public float rotateSpeed = 500;

	private Collider[] hitColliders;
	private Vector3 attackOffset;
	private float attackRadius;
	private const string PLAYER_TAG = "Player";
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.down, rotateSpeed * Time.deltaTime);
		ThrowDamage();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Player") {
			Debug.Log ("i have hit "+ col.tag+ " and am now dissapering");
			Destroy (this.gameObject);
		}
	}

	[Client]
	public void ThrowDamage()
	{
		hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

		foreach (Collider hitCol in hitColliders)
		{
			if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG)
			{
				Debug.Log("Hit Player!");

				CmdTakeDamage(hitCol.gameObject.name, Damage, transform.name);

				Destroy (this.gameObject);
			}
		}
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}

	[Command]
	protected void CmdPlayerAttacked(string _playerID, float _damage, string _sourceID)
	{
		Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}
}
