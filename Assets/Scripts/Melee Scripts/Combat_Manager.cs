using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat_Manager : NetworkBehaviour {

    // Player Animator
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private NetworkAnimator netanim;

    [SerializeField]
    Transform playerModel;

    string stateName;

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

	//stun timers
	public float stunTime = 2f;

	// slow info
	public float slowTime = 2f;
	private int reducedWalkSpeed = 6;
	private int normalWalkSpeed = 12;

	private int reducedJumpSpeed = 7;
	private int normalJumpSpeed = 15;

    // Boolean
    public bool isAttacking;
    [SerializeField]
    public bool animationPlaying;
    private bool gravParticleSystemPlayed = false;

    // Floats
    private float speed;
    [SerializeField]
    private float ultGain;

    // Ints
    [SerializeField]
    private int attackCounter;

	//particles
	private ParticleSystem playGravLandSmall;
	private ParticleSystem playGravLandMed;
	private ParticleSystem playGravLandLarge;
	public ParticleSystem gravLandParticleSmall;
	public ParticleSystem gravLandParticleMed;
	public ParticleSystem gravLandParticleLarge;

    // Scripts
    Network_Soundscape networkSoundscape;
	PlayerController playerControllermodifier;
    Network_PlayerManager networkPlayerManager;
    Dash dashScript;

	// Use this for initialization
	void Start ()
    {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        playerRigidbody = transform.GetComponent<Rigidbody>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();
        dashScript = transform.GetComponent<Dash>();

        playerDamage = 5;
        attackRadius = 5;

		//anim = GetComponent<Animator>();
		//anim.speed = 0.2f;
    }

    void Update()
    {
        CheckAnimation();
        AttackPlayer();
        PlayerVelocity();
    }
		
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "UnitD1_RangedWeapon") 
		{
			StartCoroutine (stunTimer());
		}
		if (other.tag == "ThrowingSword") 
		{
			StartCoroutine (slowTimer());
		}

		ParticleSystem playGravLandMed = (ParticleSystem)Instantiate(gravLandParticleMed,this.transform.position + Vector3.down, this.transform.rotation);
		gravLandParticleMed.Emit(1);


	}

	IEnumerator stunTimer()
	{
		//play stun particles
		this.gameObject.GetComponentInChildren<PlayerController>().enabled = false;
		Debug.Log (" A player has been stunned");
		yield return new WaitForSeconds (stunTime);
		if (isLocalPlayer) // if they are the local player enable so they they can move agian whuile not ebalaing it for other players
		{
			this.gameObject.GetComponentInChildren<PlayerController>().enabled = true;
		}
		Debug.Log ("the player can move agian");
	}

	IEnumerator slowTimer()
	{
		playerControllermodifier = this.gameObject.GetComponentInChildren<PlayerController> ();
		//play stun particles
		playerControllermodifier.moveSettings.forwardVel = reducedWalkSpeed;
		playerControllermodifier.moveSettings.rightVel = reducedWalkSpeed;
		playerControllermodifier.moveSettings.jumpVel = reducedJumpSpeed;
		Debug.Log (" A player has been slowed");
		yield return new WaitForSeconds (slowTime);
		if (isLocalPlayer) // if they are the local player enable so they they can move agian whuile not ebalaing it for other players
		{
			playerControllermodifier.moveSettings.forwardVel = normalWalkSpeed;
			playerControllermodifier.moveSettings.rightVel = normalWalkSpeed;
			playerControllermodifier.moveSettings.jumpVel = normalJumpSpeed;
		}
		Debug.Log ("the player is running at normal speed agian");
	}

    void CheckAnimation()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetBool("Attacking", false);
            attackCounter = 0;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetBool("Jump", false);
            attackCounter = 1;
        }

        if (anim.GetBool("Moving"))
        {
            anim.SetBool("Attacking", false);
        }
    }

    void AttackPlayer()
    {
        if (UI_PauseMenu.IsOn == true)
            return;

        if (Input.GetKeyUp(KeyCode.Mouse0) && isAttacking == false && attackCounter == 0)
        {
            anim.SetBool("Attacking", true);
            netanim.SetTrigger("Attack");
            isAttacking = true;
        }

        if (isAttacking)
        {
            Attacking();
        }

        if (isLocalPlayer && Input.GetKeyUp(KeyCode.K))
        {
            playerDamage = 100;
            CmdTakeDamage(transform.name, playerDamage, transform.name);
        }
    }

    [Client]
     public void Attacking()
    {
        CheckCollision();
    }

    void CheckCollision()
    {
        hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG)
            {
                Debug.Log("Hit Player!");
                //networkSoundscape.PlaySound(1, 1, 0f);

                hitCol.GetComponent<Combat_Manager>().enabled = true; // enables the combat nmanager to get correct attack damage values

                if (hitCol.GetComponent<Combat_Manager>().isAttacking == true)
                { // check to see if the other player is attacking
                    if (hitCol.GetComponent<Combat_Manager>().playerDamage == this.GetComponent<Combat_Manager>().playerDamage)
                    {  // if the player has equal damage as oppenent
                        //Debug.Log("knockedback");
                        StartCoroutine(knockBack());
                    }
                }

                else if (hitCol.GetComponent<Combat_Manager>().playerDamage < this.GetComponent<Combat_Manager>().playerDamage)
                { // if the player has more damage then oponent

                   // Debug.Log("won clash");

                    if (networkPlayerManager.playerCharacterID == "ERNN")
                    {
                        StartCoroutine(ERNNAttacking(hitCol));
                    }

                    else if (networkPlayerManager.playerCharacterID == "SPKS")
                    {
                        SendDamage(hitCol);
                    }

                    else if (networkPlayerManager.playerCharacterID == "UT-D1")
                    {
                        SendDamage(hitCol);
                    }

                    GetComponent<Dash>().chargePercent += ultGain;
                    isAttacking = false;
                }

                else
                {
                   // Debug.Log("i had less damage and loss the clash");
                }

                hitCol.GetComponent<Combat_Manager>().enabled = false;
            }

            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag != PLAYER_TAG)
            {
                isAttacking = false;
            }
        }
    }

    void SendDamage(Collider hitCol)
    {
        Debug.Log(hitCol.GetComponent<Combat_Manager>().playerDamage);
        CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
    }

    IEnumerator ERNNAttacking(Collider hitCol)
    {
        yield return new WaitForSeconds(0.36f);
        CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
        yield return new WaitForSeconds(0.36f);
        CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
        yield return new WaitForSeconds(0.36f);
        CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);

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
