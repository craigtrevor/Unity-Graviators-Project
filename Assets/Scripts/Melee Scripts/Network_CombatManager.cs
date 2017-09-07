﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_CombatManager : NetworkBehaviour {

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
	public float lowDamageVelocity = 40;
    [SerializeField]
	public float highDamageVelocity = 80;

    //stun timers
    private float d1StunTime = 2f;
    private float sparkusStunTime = 5f;

    // slow info
    public float slowTime = 2f;
    private int reducedWalkSpeed = 6;
    private int normalWalkSpeed = 12;

    private int reducedJumpSpeed = 7;
    private int normalJumpSpeed = 15;

    // Boolean
    public bool isAttacking;
    public bool isHitting;
    [SerializeField]
    public bool animationPlaying;


    // Floats
    private float speed;
    [SerializeField]
	public float ultGain;

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

	public GameObject grindParticle;

    // Scripts
    Network_Soundscape networkSoundscape;
    PlayerController playerControllermodifier;
    Network_PlayerManager networkPlayerManager;
    Dash dashScript;

	public bool clashActive;
	public bool alwaysAttack;

    // Use this for initialization
    void Start() {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        playerRigidbody = transform.GetComponent<Rigidbody>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();
        dashScript = transform.GetComponent<Dash>();

        playerDamage = 5;
        attackRadius = 3;
        attackCounter = 0;

        //anim = GetComponent<Animator>();
        //anim.speed = 0.2f;
    }

    void Update() {
        CheckAnimation();
        AttackPlayer();
        PlayerVelocity();
		}

    void OnTriggerEnter(Collider other) {
        if (other.tag == "UnitD1_RangedWeapon") {
            //StartCoroutine (stunTimer());
            StartCoroutine(stunTimer(d1StunTime));
        }
        if (other.tag == "ThrowingSword") {
            StartCoroutine(slowTimer());
        }
        if (other.tag == "Sparkus_Ranged") {
            StartCoroutine(stunTimer(sparkusStunTime));
        }
	 if (other.tag == "collider") {
			
			ParticleSystem playGravLandMed = (ParticleSystem)Instantiate (gravLandParticleMed, this.transform.position + Vector3.down, this.transform.rotation);
			gravLandParticleMed.Emit (1);
		} 
		
    }

    IEnumerator stunTimer(float stunTime) {
        /*
        //play stun particles
        this.gameObject.GetComponentInChildren<PlayerController>().enabled = false;
		Debug.Log (" A player has been stunned");
		yield return new WaitForSeconds (stunTime);
		if (isLocalPlayer) // if they are the local player enable so they they can move agian whuile not ebalaing it for other players
		{
			this.gameObject.GetComponentInChildren<PlayerController>().enabled = true;
		}
		Debug.Log ("the player can move agian");*/

        if (!isLocalPlayer) {
            this.gameObject.GetComponentInChildren<PlayerController>().stunned = true;
            yield return new WaitForSeconds(stunTime);
        }

        if (isLocalPlayer) {
            this.gameObject.GetComponentInChildren<PlayerController>().stunned = false;
        }
    }

    IEnumerator slowTimer() {
        playerControllermodifier = this.gameObject.GetComponentInChildren<PlayerController>();
        //play stun particles
        if (!isLocalPlayer) {
            playerControllermodifier.moveSettings.forwardVel = reducedWalkSpeed;
            playerControllermodifier.moveSettings.rightVel = reducedWalkSpeed;
            playerControllermodifier.moveSettings.jumpVel = reducedJumpSpeed;
            yield return new WaitForSeconds(slowTime);
        }
        if (isLocalPlayer) // if they are the local player enable so they they can move agian whuile not ebalaing it for other players
        {
            playerControllermodifier.moveSettings.forwardVel = normalWalkSpeed;
            playerControllermodifier.moveSettings.rightVel = normalWalkSpeed;
            playerControllermodifier.moveSettings.jumpVel = normalJumpSpeed;
        }
    }

    void CheckAnimation() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            anim.SetBool("Attacking", false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            anim.SetBool("Jump", false);
        }

        if (anim.GetBool("Moving")) {
            anim.SetBool("Attacking", false);
        }
    }

    void PlayMeleeSound() {
        if (networkPlayerManager.playerCharacterID == "ERNN") {
            networkSoundscape.PlaySound(0, 0, 0f);
        } else if (networkPlayerManager.playerCharacterID == "SPKS") {
            networkSoundscape.PlaySound(1, 0, 0f);
        } else if (networkPlayerManager.playerCharacterID == "UT-D1") {
            networkSoundscape.PlaySound(2, 0, 0f);
        }
    }

    void AttackPlayer() {
        if (UI_PauseMenu.IsOn == true)
            return;

        if (Input.GetKeyUp(KeyCode.Mouse0) && isAttacking == false) {
            anim.SetBool("Attacking", true);
            netanim.SetTrigger("Attack");
            PlayMeleeSound();
            isAttacking = true;
        }

		if (alwaysAttack && isAttacking == false) {
			anim.SetBool("Attacking", true);
			netanim.SetTrigger("Attack");
			PlayMeleeSound();
			isAttacking = true;
		}

        if (isAttacking) {
            Attacking();
        }

        if (isLocalPlayer && Input.GetKeyUp(KeyCode.K)) {
            playerDamage = 10;
            CmdTakeDamage(transform.name, playerDamage, transform.name);
        }
    }

	public void AttackFinished() {
        attackCounter = 0;
        isAttacking = false;
    }

    [Client]
    public void Attacking() {
        //CheckCollision();
    }

	public void weaponCollide(Collider collision, Vector3 hitPoint) {
		//Debug.Log (collision.gameObject.name + " struck at " + hitPoint);
		if (isAttacking) {
			Debug.Log ("strike");
			GameObject tempParticle = Instantiate (grindParticle);
			tempParticle.transform.position = hitPoint;

			if (collision.gameObject.tag == PLAYER_TAG) {
				isHitting = true;

				if (!clashActive) {
					if (attackCounter == 0) {
						SendDamage (collision);
						attackCounter = 1;
					}
				} else if (clashActive) {
					// Clash Active
					if (collision.gameObject.GetComponent<Network_CombatManager> ().isAttacking == true) { // check to see if the other player is attacking
						if (collision.gameObject.GetComponent<Network_CombatManager> ().playerDamage == this.GetComponent<Network_CombatManager> ().playerDamage) {  // if the player has equal damage as oppenent
							StartCoroutine (KnockBack (collision, hitPoint));
						}
					} else if (collision.gameObject.GetComponent<Network_CombatManager> ().playerDamage < this.GetComponent<Network_CombatManager> ().playerDamage) { // if the player has more damage then oponent
						if (attackCounter == 0) {
							SendDamage (collision);
							attackCounter = 1;
						}

					} else {
						StartCoroutine (KnockBack (collision, hitPoint));
					}
				}
			}

			else if (clashActive) {
				StartCoroutine (KnockBack (collision, hitPoint));
			}
		}
	}

    void SendDamage(Collider hitCol) {
        if (isHitting) {
            if (networkPlayerManager.playerCharacterID == "ERNN") {
				CmdTakeDamage (hitCol.gameObject.name, playerDamage / 15, transform.name);
				StartCoroutine (AttackDelay ());
                //GetComponent<Dash>().chargePercent += ultGain;
            } else if (networkPlayerManager.playerCharacterID == "SPKS") {
                CmdTakeDamage(hitCol.gameObject.name, playerDamage / 2, transform.name);
				StartCoroutine (AttackDelay ());
            } else if (networkPlayerManager.playerCharacterID == "UT-D1") {
                CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
				StartCoroutine (AttackDelay ());
            }
        }
    }

	IEnumerator AttackDelay() {
		yield return new WaitForSeconds (0.1f);
		attackCounter = 0;
	}

	IEnumerator KnockBack(Collider hitCol, Vector3 hitPoint) {
		gameObject.GetComponentInChildren<PlayerController>().enabled = false;
		playerRigidbody.constraints = RigidbodyConstraints.None;
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		float multiplier = 1; 
		if (hitCol.gameObject.tag == PLAYER_TAG) {
			multiplier = hitCol.gameObject.GetComponent<Network_CombatManager> ().playerDamage;
		}
		playerRigidbody.AddExplosionForce(100000 * multiplier, hitPoint, 10f);	
		yield return new WaitForSeconds (0.5f);
		playerRigidbody.constraints = RigidbodyConstraints.None;
		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		gameObject.GetComponentInChildren<PlayerController>().enabled = true;
	}

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    void PlayerVelocity() {
        speed = playerRigidbody.velocity.magnitude;

        if (speed < lowDamageVelocity) {
            playerDamage = 25.0f;
            ultGain = 5;
        } else if (lowDamageVelocity < speed && speed < highDamageVelocity) {
            playerDamage = 50.0f;
            ultGain = 10;
        } else if (highDamageVelocity < speed) {
            playerDamage = 70.0f;
            ultGain = 20;
        } 
    }

}
