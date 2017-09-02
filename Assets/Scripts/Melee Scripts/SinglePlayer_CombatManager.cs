﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer_CombatManager : MonoBehaviour {

    // Player Animator
    [SerializeField]
    private Animator anim;

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
    PlayerController playerControllermodifier;
    Dash dashScript;

	public bool isDashing;
	public GravityAxisScript gravityAxisScript;


    // Use this for initialization
    void Start()
    {
        playerRigidbody = transform.GetComponent<Rigidbody>();
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
        ParticleSystem playGravLandMed = (ParticleSystem)Instantiate(gravLandParticleMed, this.transform.position + Vector3.down, this.transform.rotation);
        gravLandParticleMed.Emit(1);
    }

	public void Slow() {
		StartCoroutine (slowTimer ());
	}

    IEnumerator stunTimer()
    {
        //play stun particles
        this.gameObject.GetComponentInChildren<PlayerController>().enabled = false;
        Debug.Log(" A player has been stunned");
        yield return new WaitForSeconds(stunTime);

        Debug.Log("the player can move agian");
    }

    IEnumerator slowTimer()
    {
        playerControllermodifier = this.gameObject.GetComponentInChildren<PlayerController>();
        //play stun particles
        playerControllermodifier.moveSettings.forwardVel = reducedWalkSpeed;
        playerControllermodifier.moveSettings.rightVel = reducedWalkSpeed;
        playerControllermodifier.moveSettings.jumpVel = reducedJumpSpeed;
        Debug.Log(" A player has been slowed");
        yield return new WaitForSeconds(slowTime);
		playerControllermodifier.moveSettings.forwardVel = normalWalkSpeed;
		playerControllermodifier.moveSettings.rightVel = normalWalkSpeed;
		playerControllermodifier.moveSettings.jumpVel = normalJumpSpeed;
        Debug.Log("the player is running at normal speed agian");
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
            anim.SetTrigger("Attack");
            isAttacking = true;
        }

        if (isAttacking)
        {
            Attacking();
        }
    }

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
				if (hitCol.gameObject.GetComponent<Bot_Script>().isAttacking == true)
                { // check to see if the other player is attacking
					if (hitCol.gameObject.GetComponent<Bot_Script>().playerDamage == this.GetComponent<SinglePlayer_CombatManager>().playerDamage)
                    {  // if the player has equal damage as oppenent
                        //Debug.Log("knockedback");
                        StartCoroutine(knockBack());
						hitCol.gameObject.GetComponent<Bot_Script> ().TakeDamage (100);
                    }
                }

				else if (hitCol.gameObject.GetComponent<Bot_Script>().playerDamage < this.GetComponent<SinglePlayer_CombatManager>().playerDamage)
                { // if the player has more damage then oponent

                    // Debug.Log("won clash");

                    //GetComponent<Dash>().chargePercent += ultGain;

                    isAttacking = false;

					hitCol.gameObject.GetComponent<Bot_Script> ().TakeDamage (100);
                }

                else
                {
                    // Debug.Log("i had less damage and loss the clash");
                }
            }

            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag != PLAYER_TAG)
            {
                isAttacking = false;
            }
        }
    }

    IEnumerator knockBack()
    {
        Debug.Log("knock Back");
        GetComponentInChildren<PlayerController>().enabled = false; // turn off player controls
        playerRigidbody.constraints = RigidbodyConstraints.None; // allows the player to move around the 3 axis's
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // stops the player from rotating
        playerRigidbody.AddForce(cameraRotation.forward * -thrust);
        yield return new WaitForSeconds(delay);
        playerRigidbody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        GetComponentInChildren<PlayerController>().enabled = true; // turn on player controls
    }

    void PlayerVelocity()
    {
        speed = playerRigidbody.velocity.magnitude;

        if (speed < lowDamageVelocity)
        {
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
        else if (lowDamageVelocity < speed && speed < highDamageVelocity)
        {
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

	public void ActualJump() {
	}

	bool CanFall() {
		if (!isDashing && !gravityAxisScript.GetGravitySwitching()) {
			return true;
		} else {
			//velocity.y *= retainFallOnGrav;
			return false;
		}
	}
}
