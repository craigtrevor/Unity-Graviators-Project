using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat_Manager : NetworkBehaviour {

	//textures
	public Renderer rend;

    // Rigidbody
    private Rigidbody playerRigidbody;
    private const string PLAYER_TAG = "Player";
    private Collider[] hitColliders;

    // Int
    public float playerDamage;
    //private int attackMask;
    private Vector3 attackOffset;

    // Float
    private float attackRadius;
	[SerializeField]
	private float lowDamageVelocity = 10; 
	[SerializeField]
		private float highDamageVelocity = 25;

    // Boolean
    public bool isAttacking;
    private bool canAttack;

    // Floats
    private float speed;

	// Use this for initialization
	void Start ()
    {
        playerRigidbody = transform.GetComponent<Rigidbody>();

        playerDamage = 5;
        attackRadius = 5;
    }
	
	// Update is called once per frame
	void Update ()
    {
        AttackPlayer();
        PlayerVelocity();
    }

    [Client]
    void AttackPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isAttacking == false)
            {
                isAttacking = true;
                Attack();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isAttacking = false;
        }
    }

    [Client]
     public void Attack()
    {
        hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG)
            {
                Debug.Log("Hit Player!");

                CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
                isAttacking = false;
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

        if (isAttacking)
        {
            networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
            isAttacking = false;
        }
    }

    void PlayerVelocity()
    {
        speed = playerRigidbody.velocity.magnitude;

        if (speed < lowDamageVelocity)
        {
            //transform.GetComponent<Renderer>().material.color = Color.green;
            playerDamage = 2.5f;
        }
        else if (lowDamageVelocity < speed && speed < highDamageVelocity)
        {
            //transform.GetComponent<Renderer>().material.color = Color.yellow;
            playerDamage = 5;
        }
        else if (highDamageVelocity < speed)
        {
            //transform.GetComponent<Renderer>().material.color = Color.red;
            playerDamage = 7.5f;
        }
        else
        {
            playerDamage = 2.5f;
        }
    }
}
