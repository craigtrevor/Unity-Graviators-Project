using System.Collections;
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
	private float lowDamageVelocity = 40;
	[SerializeField]
	private float highDamageVelocity = 80;

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

	// Use this for initialization
	void Start()
	{
		playerRigidbody = transform.GetComponent<Rigidbody>();
		dashScript = transform.GetComponent<Dash>();

		playerDamage = 5;
		attackRadius = 5;
		attackCounter = 0;

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
			StartCoroutine(stunTimer(d1StunTime));
		}
		if (other.tag == "ThrowingSword")
		{
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

	IEnumerator stunTimer(float stunTime)
	{
		this.gameObject.GetComponentInChildren<PlayerController>().stunned = true;
		yield return new WaitForSeconds(stunTime);
		this.gameObject.GetComponentInChildren<PlayerController>().stunned = false;

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

	void PlayMeleeSound() 
	{
		//networkSoundscape.PlaySound(0, 0, 0f);
	}

	void AttackPlayer()
	{
		if (UI_PauseMenu.IsOn == true)
			return;

		if (Input.GetKeyUp(KeyCode.Mouse0) && isAttacking == false) {
			anim.SetBool("Attacking", true);
			anim.SetTrigger("Attack");
			StartCoroutine(meleeAnimationPlaying());
			PlayMeleeSound();
			isAttacking = true;
		}

		if (isAttacking) {
			Attacking();
		}
	}

	IEnumerator meleeAnimationPlaying() {
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		attackCounter = 0;
		StopAllCoroutines();
		isAttacking = false;
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
				Debug.Log("Hit Player!");
				isHitting = true;
				//networkSoundscape.PlaySound(1, 1, 0f);

				hitCol.GetComponent<Bot_Script>().enabled = true; // enables the combat manager to get correct attack damage values

				if (hitCol.GetComponent<Bot_Melee>().isAttacking == true)
				{ // check to see if the other player is attacking
					if (hitCol.GetComponent<Bot_Melee>().BotDamage == this.GetComponent<SinglePlayer_CombatManager>().playerDamage)
					{  // if the player has equal damage as oppenent
						//Debug.Log("knockedback");
						StartCoroutine(knockBack());
					}
				}

				else if (hitCol.GetComponent<Bot_Melee>().BotDamage < this.GetComponent<SinglePlayer_CombatManager>().playerDamage)
				{ // if the player has more damage then oponent

					// Debug.Log("won clash");
					//do damage
					if (attackCounter == 0) {
						SendDamage(hitCol);
						attackCounter = 1;
					}

					//GetComponent<Dash>().chargePercent += ultGain;

					isAttacking = false;
				}

				else
				{
					// Debug.Log("i had less damage and loss the clash");
				}

			}
			if (hitCol.transform.root != transform.root && hitCol.gameObject.tag != PLAYER_TAG)
			{
				isHitting = false;
				StopCoroutine(ERNNAttacking(hitCol));
			}
		}
	}

	void SendDamage(Collider hitCol) {
		if (isHitting) 
		{
			StartCoroutine(ERNNAttacking(hitCol));
			//GetComponent<Dash>().chargePercent += ultGain;
		}
	}

	IEnumerator ERNNAttacking(Collider hitCol) {
		yield return new WaitForSeconds(0.36f);
		hitCol.GetComponent<Bot_Script> ().health = hitCol.GetComponent<Bot_Script> ().health - playerDamage;
		yield return new WaitForSeconds(0.36f);
		hitCol.GetComponent<Bot_Script> ().health = hitCol.GetComponent<Bot_Script> ().health - playerDamage;
		yield return new WaitForSeconds(0.36f);
		hitCol.GetComponent<Bot_Script> ().health = hitCol.GetComponent<Bot_Script> ().health - playerDamage;
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

			playerDamage = 25.0f;
			ultGain = 5;



		}//end low velocity
		else if (lowDamageVelocity < speed && speed < highDamageVelocity)
		{

			playerDamage = 50.0f;
			ultGain = 10;
		}
		else if (highDamageVelocity < speed)
		{
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
