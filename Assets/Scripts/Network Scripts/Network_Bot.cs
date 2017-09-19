using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Network_Bot : NetworkBehaviour {

	public Text username;

	public float health = 100;
	public Slider healthSlider;

	public string playerCharacterID = "ERNN";

	private const string PLAYER_TAG = "Player";
	private const string BOT_TAG = "NetBot";

	public Animator anim;
	public NetworkAnimator netanim;
	private ParticleManager particleManager;

	public bool isAttacking;
	public bool isHitting;
	public int attackCounter;

	public float playerDamage = 25;

	public bool stunned;
	public bool slowed;

	// Use this for initialization
	void Start () {
		username.text = "Test Bot";
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckAnimation ();
		healthSlider.value = health - 10;
		if (health <= 0) {
			Die();
		}
		if (!isAttacking) {
			StartCoroutine(ActionDelay ());
			isAttacking = true;
		}
	}

	void CheckAnimation() {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
			anim.SetBool("Attacking", false);
		}

		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("UltimateDash")) {
			anim.SetBool("Attacking", false);
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
			anim.SetBool("Jump", false);
		}
	}

	IEnumerator ActionDelay() {
		yield return new WaitForSeconds(5f);
		Attack ();
	}

	public void TakeDamage(float damage) {
		health -= damage;
	}

	public void Die() {
		Destroy (this.gameObject);
	}

	public void Attack() {
		anim.SetBool("Attacking", true);
		netanim.SetTrigger("Attack");
		isAttacking = true;
	}

	public void Stun(float time) {
		stunned = true;
		StartCoroutine (StunnedFor (time));
	}

	IEnumerator StunnedFor(float time) {
		yield return new WaitForSeconds(time);
		stunned = false;
	}

	public void Slow(float time) {
		slowed = true;
		StartCoroutine (SlowedFor (time));
	}

	IEnumerator SlowedFor(float time) {
		yield return new WaitForSeconds(time);
		slowed = false;
	}

	public void RangedAttack () {
	}

	public void ActualJump () {
	}

	public void AttackFinished () {
		isAttacking = false;
	}

	public void weaponCollide(Collider collision, Vector3 hitPoint, bool airStrike) {
		//Debug.Log (collision.gameObject.name + " struck at " + hitPoint);
		if (isAttacking) {
			GameObject tempParticle = Instantiate(particleManager.GetParticle("grindParticle"));
			tempParticle.transform.position = hitPoint;
			if (collision.gameObject.tag == PLAYER_TAG) {
				isHitting = true;
				if (attackCounter == 0) {
					SendDamage (collision);
					attackCounter = 1;
				}
			}
			if (collision.gameObject.tag == BOT_TAG) {
				isHitting = true;
				if (attackCounter == 0) {
					SendBotDamage (collision);
					attackCounter = 1;
				}
			}
		}
	}

	void SendDamage(Collider hitCol) {
		if (isHitting) {
			if (playerCharacterID == "ERNN") {
				CmdTakeDamage(hitCol.gameObject.name, playerDamage / 15, transform.name);
				StartCoroutine(AttackDelay());
				//GetComponent<Dash>().chargePercent += ultGain;
			} else if (playerCharacterID == "SPKS") {
				CmdTakeDamage(hitCol.gameObject.name, playerDamage / 2, transform.name);
				StartCoroutine(AttackDelay());
			} else if (playerCharacterID == "UT-D1") {
				CmdTakeDamage(hitCol.gameObject.name, playerDamage, transform.name);
				StartCoroutine(AttackDelay());
			}
		}
	}

	void SendBotDamage(Collider hitCol) {
		if (isHitting) {
			if (playerCharacterID == "ERNN") {
				hitCol.GetComponent<Network_Bot> ().TakeDamage (playerDamage / 15);
				StartCoroutine(AttackDelay());
			} else if (playerCharacterID == "SPKS") {
				hitCol.GetComponent<Network_Bot> ().TakeDamage (playerDamage / 2);
				StartCoroutine(AttackDelay());
			} else if (playerCharacterID == "UT-D1") {
				hitCol.GetComponent<Network_Bot> ().TakeDamage (playerDamage);
				StartCoroutine(AttackDelay());
			}
		}
	}

	IEnumerator AttackDelay() {
		yield return new WaitForSeconds(0.1f);
		attackCounter = 0;
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
		Debug.Log(_playerID + " has been attacked.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}
}
