using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Melee : MonoBehaviour {

	// Player Animator
	[SerializeField]
	private Animator anim;

	// Rigidbody
	private Rigidbody BotRigidbody;
	private const string PLAYER_TAG = "Player";

	private Collider[] hitColliders;

	// Int
	public float BotDamage = 25;
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


	// Floats
	private float speed;

	// Ints
	[SerializeField]
	private int attackCounter;





	// Use this for initialization
	void Start() {
		BotRigidbody = transform.GetComponent<Rigidbody>();
		BotDamage = 5;
		attackRadius = 3;
		attackCounter = 0;
	}

	void Update() {
		CheckAnimation();
		AttackPlayer();
		PlayerVelocity();
	}



	void CheckAnimation() {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
			anim.SetBool("Attack", false);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
			anim.SetBool("Jump", false);
		}

		if (anim.GetBool("Moving")) {
			anim.SetBool("Attack", false);
		}
	}

	void AttackPlayer()
	{

		if (isAttacking == false) {
			anim.SetBool("Attack", true);
			StartCoroutine(meleeAnimationPlaying());
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

		
	public void Attacking() {
		CheckCollision();
	}

	void CheckCollision() {
		hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

		foreach (Collider hitCol in hitColliders) {
			if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG) {
				Debug.Log("Hit Player!");
				isHitting = true;


				hitCol.GetComponent<SinglePlayer_CombatManager>().enabled = true; // enables the combat nmanager to get correct attack damage values

				if (hitCol.GetComponent<SinglePlayer_CombatManager>().isAttacking == true) { // check to see if the other player is attacking
					if (hitCol.GetComponent<SinglePlayer_CombatManager>().playerDamage == this.GetComponent<Bot_Melee>().BotDamage) {  // if the player has equal damage as oppenent
						//Debug.Log("knockedback");
						//StartCoroutine(knockBack());
					}
				} else if (hitCol.GetComponent<SinglePlayer_CombatManager>().playerDamage < this.GetComponent<Bot_Melee>().BotDamage) { // if the player has more damage then oponent

					// Debug.Log("won clash");

					if (attackCounter == 0) {
						SendDamage(hitCol);
						Debug.Log ("attack counter went to 1");
						attackCounter = 1;
					}

				} else {
					// Debug.Log("i had less damage and loss the clash");
				}

				//hitCol.GetComponent<SinglePlayer_CombatManager>().enabled = false;
			}

			if (hitCol.transform.root != transform.root && hitCol.gameObject.tag != PLAYER_TAG) {
				isHitting = false;
				StopCoroutine(ERNNAttacking(hitCol));
			}
		}
	}

	void SendDamage(Collider hitCol) {
		if (isHitting) 
		{
			StartCoroutine(ERNNAttacking(hitCol));
		}
	}

	IEnumerator ERNNAttacking(Collider hitCol) {
		yield return new WaitForSeconds(0.36f);
		TakeDamage(hitCol.gameObject.name, BotDamage, transform.name);
		yield return new WaitForSeconds(0.36f);
		TakeDamage(hitCol.gameObject.name, BotDamage, transform.name);
		yield return new WaitForSeconds(0.36f);
		TakeDamage(hitCol.gameObject.name, BotDamage, transform.name);
	}

	IEnumerator knockBack() {
		Debug.Log("knock Back"); 
		BotRigidbody.constraints = RigidbodyConstraints.None; // allows the player to move around the 3 axis's
		BotRigidbody.constraints = RigidbodyConstraints.FreezeRotation; // stops the player from rotating
		BotRigidbody.AddForce(transform.forward * -thrust);
		yield return new WaitForSeconds(delay);
		BotRigidbody.constraints = RigidbodyConstraints.None; // free the player to allow movement agian
		BotRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}

	void TakeDamage(string _playerID, float _damage, string _sourceID) {
		Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}

	void PlayerVelocity() {
		speed = BotRigidbody.velocity.magnitude;

		if (speed < lowDamageVelocity) {
			BotDamage = 25.0f;
		}//end low velocity
		else if (lowDamageVelocity < speed && speed < highDamageVelocity) {
			BotDamage = 50.0f;
		} else if (highDamageVelocity < speed) {
			BotDamage = 70.0f;
		} else {
			BotDamage = 25.0f;
		}
	}


	//	void OnTriggerEnter(Collider other) {
	//		if (other.tag == "UnitD1_RangedWeapon") {
	//			//StartCoroutine (stunTimer());
	//			StartCoroutine(stunTimer(d1StunTime));
	//		}
	//		if (other.tag == "ThrowingSword") {
	//			StartCoroutine(slowTimer());
	//		}
	//		if (other.tag == "Sparkus_Ranged") {
	//			StartCoroutine(stunTimer(sparkusStunTime));
	//		}
	//	}

	//	IEnumerator stunTimer(float stunTime) {
	//		/*
	//        //play stun particles
	//		this.gameObject.GetComponentInChildren<PlayerController>().enabled = false;
	//		Debug.Log (" A player has been stunned");
	//		yield return new WaitForSeconds (stunTime);
	//		if (isLocalPlayer) // if they are the local player enable so they they can move agian whuile not ebalaing it for other players
	//		{
	//			this.gameObject.GetComponentInChildren<PlayerController>().enabled = true;
	//		}
	//		Debug.Log ("the player can move agian");*/
	//
	//		if (!isLocalPlayer) {
	//			this.gameObject.GetComponentInChildren<PlayerController>().stunned = true;
	//			yield return new WaitForSeconds(stunTime);
	//		}
	//
	//		if (isLocalPlayer) {
	//			this.gameObject.GetComponentInChildren<PlayerController>().stunned = false;
	//		}
	//	}

	//	IEnumerator slowTimer() {
	//		playerControllermodifier = this.gameObject.GetComponentInChildren<PlayerController>();
	//		//play stun particles
	//		if (!isLocalPlayer) {
	//			playerControllermodifier.moveSettings.forwardVel = reducedWalkSpeed;
	//			playerControllermodifier.moveSettings.rightVel = reducedWalkSpeed;
	//			playerControllermodifier.moveSettings.jumpVel = reducedJumpSpeed;
	//			yield return new WaitForSeconds(slowTime);
	//		}
	//		if (isLocalPlayer) // if they are the local player enable so they they can move agian whuile not ebalaing it for other players
	//		{
	//			playerControllermodifier.moveSettings.forwardVel = normalWalkSpeed;
	//			playerControllermodifier.moveSettings.rightVel = normalWalkSpeed;
	//			playerControllermodifier.moveSettings.jumpVel = normalJumpSpeed;
	//		}
	//	}

}
