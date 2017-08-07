using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat_Manager : NetworkBehaviour {

    // Player Animator
    [SerializeField]
    private Animator anim;

    //textures
    public Renderer rend;

    // Rigidbody
    private Rigidbody playerRigidbody;
    private const string PLAYER_TAG = "Player";

    private Collider[] hitColliders;

	public Transform cameraRotation; // the camerasrotation, assign in game editor

    // Int
	public float playerDamage = 25;
	public int thrust = 2000; //change this for speed of knock back
	public float delay = 0.2f; 
    //private int attackMask;
    private Vector3 attackOffset;

    // Float
    private float attackRadius;
	[SerializeField]
	private float lowDamageVelocity = 10; 
	[SerializeField]
	private float highDamageVelocity = 25;

	public float ultGain;

    public float CurrentUltGain() {
        return ultGain;
    }

    // Boolean
    public bool isAttacking;
    private bool canAttack;
	private bool gravParticleSystemPlayed = false;

    // Floats
    private float speed;

	//particles
	private ParticleSystem playGravLandSmall;
	private ParticleSystem playGravLandMed;
	private ParticleSystem playGravLandLarge;
	public ParticleSystem gravLandParticleSmall;
	public ParticleSystem gravLandParticleMed;
	public ParticleSystem gravLandParticleLarge;

    // Scripts
    Network_Soundscape networkSoundscape;

	// Use this for initialization
	void Start ()
    {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
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

    void AttackPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isAttacking == false)
                {
                    isAttacking = true;

                    if (isLocalPlayer && anim.GetBool("Attack") == true && isAttacking && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
                    {
                        networkSoundscape.PlaySound(0, 0, 0.0f);
                    }

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
				Debug.Log ("Hit Player!");
                networkSoundscape.PlaySound(1, 1, 0f);

                hitCol.GetComponent<Combat_Manager> ().enabled = true; // enables the combat nmanager to get correct attack damage values
				if (hitCol.GetComponent<Combat_Manager> ().isAttacking == true) { // check to see if the other player is attacking
					if (hitCol.GetComponent<Combat_Manager> ().playerDamage == this.GetComponent<Combat_Manager> ().playerDamage) {  // if the player has equal damage as oppenent
						Debug.Log ("knockedback");
						StartCoroutine (knockBack ());
					} else if (hitCol.GetComponent<Combat_Manager> ().playerDamage < this.GetComponent<Combat_Manager> ().playerDamage) { // if the player has more damage then oponent
						Debug.Log ("won clash");
						Debug.Log (hitCol.GetComponent<Combat_Manager> ().playerDamage);

                        CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
						isAttacking = false;
						GetComponent<Dash> ().chargePercent += ultGain;
					} else {
						Debug.Log ("i had less damage and loss the clash");	
					}
				} else 
				{
					Debug.Log ("did damage");
					Debug.Log (hitCol.GetComponent<Combat_Manager> ().playerDamage);

                    CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
					isAttacking = false;
					GetComponent<Dash> ().chargePercent += ultGain;
				}
				hitCol.GetComponent<Combat_Manager> ().enabled = false; 
			}
        }
    }

	IEnumerator knockBack()
	{
		Debug.Log ("knock Back");
		GetComponentInChildren<PlayerController> ().enabled = false; // turn off player controls
		playerRigidbody.constraints = RigidbodyConstraints.None; // allows the player to move around the 3 axis's
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // stops the player from rotating
		playerRigidbody.AddForce(cameraRotation.forward * -thrust);
		yield return new WaitForSeconds (delay);
		playerRigidbody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		GetComponentInChildren<PlayerController> ().enabled = true; // turn on player controls
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

		if (speed < lowDamageVelocity) {
			//This does not work, collider check needs to be in OnTrigger
			/*if (collider.tag == "collider" ) {
				ParticleSystem playGravLandSmall = (ParticleSystem)Instantiate (gravLandParticleSmall, this.transform.position, this.transform.rotation);

				if (!gravParticleSystemPlayed) {
					{
						playGravLandSmall.Emit (1);
						gravParticleSystemPlayed = true;
						Debug.Log ("gravPlayed");
					}

					if (gravParticleSystemPlayed == true) {
						Destroy (playGravLandSmall);
						Debug.Log ("gravDead");
					}
					}
					}*/ //End ParticleScript
					//transform.GetComponent<Renderer>().material.color = Color.green;
					playerDamage = 25.0f;
					ultGain = 5;
				
			
		}//end low velocity
        else if (lowDamageVelocity < speed && speed < highDamageVelocity) {
			/*if (collider.tag == "collider") {
				ParticleSystem playGravLandMed = (ParticleSystem)Instantiate (gravLandParticleMed, this.transform.position, this.transform.rotation);

				if (!gravParticleSystemPlayed) {
					{
						playGravLandMed.Emit (1);
						gravParticleSystemPlayed = true;
						Debug.Log ("gravPlayed");
					}

					if (gravParticleSystemPlayed == true) {
						Destroy (playGravLandMed);
						Debug.Log ("gravDead");
					}
				}
			}*/ //End Particle script\


			//transform.GetComponent<Renderer>().material.color = Color.yellow;
			playerDamage = 50.0f;
			ultGain = 10;
		}
        else if (highDamageVelocity < speed)
        {
            //transform.GetComponent<Renderer>().material.color = Color.red;
            playerDamage = 70.0f;
			ultGain = 20;
        }
        else
        {
            playerDamage = 25.0f;
			ultGain = 5;
        }
    }
}
