using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class damageRange : NetworkBehaviour {

    private string sourceID;

	// Sword Stats
	public int SwordDamage = 30;
	public float rotateSpeed = 500;
	//slow Variables
	[SerializeField]
	private int reducedWalkSpeed = 6;
	[SerializeField]
	private int normalWalkSpeed = 12;

	[SerializeField]
	private int reducedJumpSpeed = 7;
	[SerializeField]
	private int normalJumpSpeed = 15;

	public float stunTime = 2f;


	//UnitD1 Ranged stats


	private Collider[] hitColliders;
	private Vector3 attackOffset;
	private float attackRadius;
	private const string PLAYER_TAG = "Player";
	private const string THROWINGSWORD_TAG = "ThrowingSword";
	private const string UNITD1RANGEWEAPON_TAG = "UnitD1_RangedWeapon";

	// scripts
	Network_PlayerManager networkPlayerManager;
	PlayerController playerController;

    void SetInitialReferences(string _sourceID)
    {
        sourceID = _sourceID;
    }

	void Update () {

		if (this.gameObject.tag == THROWINGSWORD_TAG) {
			transform.Rotate (Vector3.down, rotateSpeed * Time.deltaTime);
		}
	}

	[Client]
	void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player") {
				Debug.Log ("i have hit " + other.tag + " and am now dissapering");
				Destroy (this.gameObject);
		} else 
		{

            //Physics.IgnoreLayerCollision (10, 30); // the ranged object will ignore the local player layer
			hitColliders = Physics.OverlapSphere (transform.TransformPoint (attackOffset), attackRadius);

			foreach (Collider hitCol in hitColliders) {
				if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && hitCol.transform.name != sourceID) {
					Debug.Log ("Hit Player!");

					if (this.gameObject.tag == THROWINGSWORD_TAG)// if a throwing sword hit the player
					{
                        CmdTakeDamage(hitCol.gameObject.name, SwordDamage, sourceID);
					}


					if (this.gameObject.tag == UNITD1RANGEWEAPON_TAG) // if UnitD1 range weapon hit the player
					{
                        CmdTakeDamage(hitCol.gameObject.name, SwordDamage, sourceID);
					}
					Destroy (this.gameObject);
				}
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
}
