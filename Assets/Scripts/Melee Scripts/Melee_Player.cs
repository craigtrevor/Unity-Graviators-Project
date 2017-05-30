using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Melee_Player : NetworkBehaviour {

    // Rigidbody
    private Rigidbody playerRigidbody;
    private const string PLAYER_TAG = "Player";

    // Int
    public int playerDamage;

    private float lowDamageVelocity = 0.0f;
    private float midDamageVelocity = 20.0f;
    private float highDamageVelocity = 30.0f;

    // Boolean
    private bool isAttacking;
    private bool canAttack;

    // Floats
    private float attackDelay = 0.1f;
    private float meleeTime = 1.0f;
    private float speed;

	// Use this for initialization
	void Start ()
    {
        playerRigidbody = transform.GetComponent<Rigidbody>();

        canAttack = false;
        playerDamage = 10;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown (KeyCode.Mouse0))
        {
            isAttacking = true;
            //StartCoroutine(AttackDelay(attackDelay));
        }
	}

    [Client]
    void OnTriggerEnter(Collider col)
    {
        if (isAttacking && col.tag == PLAYER_TAG)
        {
            Debug.Log("CMD1");
            CmdPlayerAttacked(col.gameObject.name, playerDamage);
            //StartCoroutine(AttackDelay(attackDelay));
        }
    }

    [Client]
    void OnTriggerStay(Collider col)
    {
        if (isAttacking && col.tag == PLAYER_TAG)
        {
            Debug.Log("CMD2");
            CmdPlayerAttacked(col.gameObject.name, playerDamage);
            //StartCoroutine(AttackDelay(attackDelay));
        }
    }

    private IEnumerator AttackDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log(canAttack);
        isAttacking = false;
    }

    [Command]
    void CmdPlayerAttacked (string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been attacked.");

        isAttacking = false;
        canAttack = false;

        Network_PlayerStats networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        networkPlayerStats.TakeDamage(_damage);
    }


    //[Command]
    //void CmdstartAttack()
    //{
    //    melee = Instantiate(meleehitboxObject, MeleeSpawn.position, MeleeSpawn.rotation, this.gameObject.transform);
    //    NetworkServer.SpawnWithClientAuthority(meleehitboxObject, connectionToClient);

    //    CanAttack = false;
    //    Attaking = false;
    //    Destroy(melee, MeleeTime);

    //    StartCoroutine(attackDelay());
    //}

    //void startAttack()
    //{
    //    canAttack = false;
    //    isAttacking = false;
    //}

    //void PlayerVelocity()
    //{
    //    speed = playerRigidbody.velocity.magnitude;

    //    if (speed < lowDamageVelocity)
    //    {
    //        transform.GetComponent<Renderer>().material.color = Color.green;
    //        playerDamage = 5;
    //    }
    //    else if (lowDamageVelocity < speed && speed < highDamageVelocity)
    //    {
    //        transform.GetComponent<Renderer>().material.color = Color.yellow;
    //        playerDamage = 10;
    //    }
    //    else if (highDamageVelocity < speed)
    //    {
    //        transform.GetComponent<Renderer>().material.color = Color.red;
    //        playerDamage = 15;
    //    }
    //    else
    //    {
    //        playerDamage = 0;
    //    }
    //}
}
