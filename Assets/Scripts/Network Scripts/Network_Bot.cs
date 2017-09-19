using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Network_Bot : NetworkBehaviour {

	public string _sourceID;
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

	//Ranged Attack Variables
	public float reloadTime = 15f;
	public bool reloading;
	public int reloadTimer = 6;

	public GameObject weapon; // prefab of the weapon
	public Transform fireTransform; // a child of the player where the weapon is spawned
	public Transform secondaryFireTransform; // second position for no name
	public bool right;
	public float force = 2000;

	private Rigidbody m_Rigidbody;

	public GameObject weaponToHide;
	public GameObject weaponToHide2;
	public MeleeWeaponTrail trailToHide;
	public MeleeWeaponTrail trailToHide2;

	private ParticleSystem playSparkusRanged;
	public GameObject sparkusRanged;
	public GameObject sparkusReloadBall;

	private ParticleSystem playD1Ranged;
	public ParticleSystem D1Ranged;
	public GameObject wingRing;

	// Use this for initialization
	void Start () {
		username.text = "Test Bot";
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
		m_Rigidbody = GetComponent<Rigidbody>();
		_sourceID = transform.root.name;
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
		SecondaryAttack();
	}

	public void TakeDamage(float damage) {
		health -= damage;
	}

	public void Die() {
		Destroy (this.gameObject);
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

	public void ActualJump () {
	}

	//------------------------------------
	// MELEE ATTACK

	public void Attack() {
		anim.SetBool("Attacking", true);
		netanim.SetTrigger("Attack");
		isAttacking = true;
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
				hitCol.gameObject.GetComponent<Network_Bot> ().TakeDamage (playerDamage / 15);
				StartCoroutine(AttackDelay());
			} else if (playerCharacterID == "SPKS") {
				hitCol.gameObject.GetComponent<Network_Bot> ().TakeDamage (playerDamage / 2);
				StartCoroutine(AttackDelay());
			} else if (playerCharacterID == "UT-D1") {
				hitCol.gameObject.GetComponent<Network_Bot> ().TakeDamage (playerDamage);
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
		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
		networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
	}


	//------------------------------------
	// RANGED ATTACK

	[Client]
	private void SecondaryAttack() {
		anim.SetBool("Attacking", false);
		netanim.SetTrigger("Ranged Attack");
	}

	public void RangedAttack() {

		//Nonames Attack
		if (playerCharacterID == "ERNN") {
			CmdFire(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
			right = true;
			CmdFire(m_Rigidbody.velocity, force, secondaryFireTransform.forward, secondaryFireTransform.position, secondaryFireTransform.rotation);
			right = false;
			weaponToHide.SetActive(false);
			weaponToHide2.SetActive(false);
			trailToHide.enabled = false;
			trailToHide2.enabled = false;
		}

		//D1s Attack
		if (playerCharacterID == "UT-D1") {
			CmdFire(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
			StartCoroutine (D1WingOff (0.5f));

		}

		//Sparkus Attack
		if (playerCharacterID == "SPKS") {
			CmdFire(Vector3.zero, 0f, fireTransform.forward, fireTransform.position, fireTransform.rotation);
			sparkusReloadBall.SetActive (false);
		}
	}

	[Command]
	private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
	{
		// create an instance of the weapon and store a reference to its rigibody
		GameObject weaponInstance = Instantiate(weapon, position, rotation);

		// Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
		Vector3 velocity = rigidbodyVelocity + launchForce * forward;

		if (playerCharacterID == "UT-D1")
		{
			velocity = new Vector3(velocity.x * 3, velocity.y * 3, velocity.z * 3);
		}

		weaponInstance.SendMessage("SetInitialReferences", _sourceID);
		if (right)
		{
			weaponInstance.SendMessage("SetRight");
		}

		// Set the shell's velocity to this velocity.
		weaponInstance.GetComponent<Rigidbody>().velocity = velocity;

		NetworkServer.Spawn(weaponInstance.gameObject);

		if (playerCharacterID == "SPKS")
		{
			weaponInstance.transform.SetParent(fireTransform);
		}
	}

	IEnumerator reload() {
		// delay before the player can fire agian
		reloadTimer = 0;

		for (int i = 0; i < 6; i++) {
			yield return new WaitForSeconds(reloadTime/6f);
			reloadTimer += 1;
		}

		if (playerCharacterID == "ERNN") {
			//play reload anim and wait for it to trigger
			anim.SetBool("Attacking", false);
			netanim.SetTrigger("Ranged Attack Reload");
		}

		if (playerCharacterID == "SPKS") {
			anim.SetBool("Attacking", false);
			netanim.SetTrigger("Ranged Attack Reload");
			sparkusReloadBall.SetActive (true);
		}

		if (playerCharacterID == "UT-D1") {
			StartCoroutine (D1WingOn (0.5f));
			anim.SetBool("Attacking", false);
		}

		//networkSoundscape.PlayNonNetworkedSound(13, 1, 0.1f);

		// allow the player to fire again

		reloading = false;
	}

	IEnumerator D1WingOn(float time) {
		float emissionStrength = 0.1f;
		for (int i = 0; i < 10; i++) {
			emissionStrength += 0.2f;
			print (emissionStrength);
			wingRing.GetComponent<Renderer> ().material.SetFloat ("_Emission", emissionStrength);
			yield return new WaitForSeconds (time / 10f);
		}
	}

	IEnumerator D1WingOff(float time) {
		float emissionStrength = 2f;
		for (int i = 0; i < 10; i++) {
			emissionStrength -= 0.2f;
			print (emissionStrength);
			wingRing.GetComponent<Renderer> ().material.SetFloat ("_Emission", emissionStrength);
			yield return new WaitForSeconds (time/10f);
		}
	}

	public void NoNameShowWeapons (AnimationEvent animEvent) {
		//show nonames the weapons again
		weaponToHide.SetActive (true);
		weaponToHide2.SetActive (true);
		trailToHide.enabled = true;
		trailToHide2.enabled = true;
	}
}
