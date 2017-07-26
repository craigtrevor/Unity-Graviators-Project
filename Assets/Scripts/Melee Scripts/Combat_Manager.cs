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

	public Transform cameraRotation; // the camerasrotation, assign in game editor

    // Int
    public float playerDamage;
	public int thrust = 1000; //change this for speed of knock back
	public float delay = 5.00f; 
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
				if (hitCol.GetComponent<Combat_Manager> ().playerDamage < this.GetComponent<Combat_Manager> ().playerDamage) { // checks to see if the other player has a smaller damage
					Debug.Log ("Hit Player!");
					Debug.Log (hitCol.GetComponent<Combat_Manager> ().playerDamage);

					CmdTakeDamage (hitCol.gameObject.name, playerDamage, transform.name);
					isAttacking = false;
				} else 
				{
					
					StartCoroutine (knockBack ());
				}
            }
        }
    }

	IEnumerator knockBack()
	{
		Debug.Log ("knock Back");
		GetComponentInChildren<PlayerController> ().enabled = false; // turn off player controls
		playerRigidbody.constraints = RigidbodyConstraints.None; // allows the player to move around the 3 axis's
		//playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // stops the player from rotating
		playerRigidbody.AddForce(cameraRotation.forward * -thrust);
		yield return new WaitForSeconds (delay);
		playerRigidbody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		GetComponentInChildren<PlayerController> ().enabled = true; // turn on player controls
		//playerRigidBody.AddForce (cameraRotation.forward * thrust);// push the player forward
		// get the palyer rigidbody and push it back
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
            playerDamage = 25.0f;
        }
        else if (lowDamageVelocity < speed && speed < highDamageVelocity)
        {
            //transform.GetComponent<Renderer>().material.color = Color.yellow;
            playerDamage = 50.0f;
        }
        else if (highDamageVelocity < speed)
        {
            //transform.GetComponent<Renderer>().material.color = Color.red;
            playerDamage = 70.0f;
        }
        else
        {
            playerDamage = 25.0f;
        }
    }
}
