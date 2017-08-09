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

    //public double ultGain;

    public float ultGain;

    public float CurrentUltGain() {
        return ultGain;
    }

    // Boolean
    public bool isAttacking;
    [SerializeField]
    public bool animationPlaying;
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
	PlayerController playerControllermodifier;


	// Use this for initialization
	void Start ()
    {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        playerRigidbody = transform.GetComponent<Rigidbody>();

        playerDamage = 5;
        attackRadius = 5;
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
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetBool("Jump", false);
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

        if (Input.GetKeyUp(KeyCode.Mouse0) && isAttacking == false)
        {
            anim.SetBool("Attacking", true);
            netanim.SetTrigger("Attack");
            Attacking();
        }
    }

    //IEnumerator AttackTime()
    //{
    //    animationPlaying = true;
    //    anim.SetBool("Attack", true);
    //    yield return new WaitForSeconds(0.04f);
    //    anim.SetBool("Attack", false);
    //}

    //IEnumerator ThreeAttacks()
    //{
    //    yield return new WaitForSeconds(25f);
    //    CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
    //    yield return new WaitForSeconds(25f);
    //    CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
    //    yield return new WaitForSeconds(25f);
    //    CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
    //}

    [Client]
     public void Attacking()
     {
        hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

        foreach (Collider hitCol in hitColliders)
        {
			if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG) 
			{
				Debug.Log ("Hit Player!");
                //networkSoundscape.PlaySound(1, 1, 0f);

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
