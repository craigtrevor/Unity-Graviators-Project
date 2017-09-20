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

	public GameObject corpse;

	public bool isAttacking;
	public bool isHitting;
	public int attackCounter;

	public float playerDamage = 25;

	public bool stunned;
	public bool slowed;

	public float speed = 1.5f;
	public bool movingToPoint;
	public GameObject currentTarget;
	public string currentTargetGravity;
	public bool matchingGravity;

	//---------------------------------
	// GRAVITY STUFF

	public string gravity;
	public Vector3 gravVector;
	public float fallSpeed = 25f;
	public bool isFalling;

	private static Vector3 xPlus = new Vector3(1, 0, 0);     // x+
	private static Vector3 xNeg = new Vector3(-1, 0, 0);     // x-
	private static Vector3 yPlus = new Vector3(0, 1, 0);        // y+
	private static Vector3 yNeg = new Vector3(0, -1, 0);     // y-
	private static Vector3 zPlus = new Vector3(0, 0, 1);   // z+
	private static Vector3 zNeg = new Vector3(0, 0, -1); // z-

	// END OF GRAVITY STUFF
	//-------------------------------------

	//Ranged Attack Variables
	public float reloadTime = 15f;
	public bool reloading;
	public int reloadTimer = 6;

	public GameObject weapon; // prefab of the weapon
	public Transform fireTransform; // a child of the player where the weapon is spawned
	public Transform secondaryFireTransform; // second position for no name
	public bool right;
	public float force = 200;

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

	public bool rangedAttacking;

	// Use this for initialization
	void Start () {
		gravity = "-y";
		username.text = "Test Bot";
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
		m_Rigidbody = GetComponent<Rigidbody>();
		_sourceID = transform.root.name;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTarget == null) {
			FindTarget ();
		}

		CheckAnimation ();
		ApplyGravity ();
		CheckTargetGravity ();

		healthSlider.value = health - 10;
		if (health <= 0) {
			Die();
		}

		Think ();
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

	void ApplyGravity() {
		Vector3 tempVec;
		//Check gravity
		if (gravity == "-y") {
			this.transform.rotation = Quaternion.Euler(0, 0, 0);
			gravVector = yNeg * fallSpeed;
			if (!Physics.Raycast(transform.position, yNeg, 0.1f)) {
				isFalling = true;
			} else {isFalling = false;}
			if (currentTarget != null) {
				transform.LookAt(currentTarget.transform.position, yPlus);
			}
		} else if (gravity == "y") {
			this.transform.rotation = Quaternion.Euler(0, 0, 180);
			gravVector = yPlus * fallSpeed;
			if (!Physics.Raycast(transform.position, yPlus, 0.1f)) {
				isFalling = true;
			} else {isFalling = false;}
			if (currentTarget != null) {
				transform.LookAt(currentTarget.transform.position, yNeg);
			}
		} else if (gravity == "z") {
			this.transform.rotation = Quaternion.Euler(-90, 0, 0);
			gravVector = zPlus * fallSpeed;
			if (!Physics.Raycast(transform.position, zPlus, 0.1f)) {
				isFalling = true;
			} else {isFalling = false;}
			if (currentTarget != null) {
				transform.LookAt(currentTarget.transform.position, zNeg);
			}
		} else if (gravity == "-z") {
			this.transform.rotation = Quaternion.Euler(-90, -180, 0);
			gravVector = zNeg * fallSpeed;
			if (!Physics.Raycast(transform.position, zNeg, 0.1f)) {
				isFalling = true;
			} else {isFalling = false;}
			if (currentTarget != null) {
				transform.LookAt(currentTarget.transform.position, zPlus);
			}
		} else if (gravity == "x") {
			this.transform.rotation = Quaternion.Euler(-90, 90, 0);
			gravVector = xPlus * fallSpeed;
			if (!Physics.Raycast(transform.position, xPlus, 0.1f)) {
				isFalling = true;
			} else {isFalling = false;}
			if (currentTarget != null) {
				transform.LookAt(currentTarget.transform.position, xNeg);
			}
		} else if (gravity == "-x") {
			this.transform.rotation = Quaternion.Euler(-90, -90, 0);
			gravVector = xNeg * fallSpeed;
			if (!Physics.Raycast(transform.position, xNeg, 0.1f)) {
				isFalling = true;
			} else {isFalling = false;}
			if (currentTarget != null) {
				transform.LookAt(currentTarget.transform.position, xPlus);
			}
		}
	}

	void FindTarget() {
		List<GameObject> opponents = new List<GameObject>();
		GameObject[] searchList;
		searchList = GameObject.FindGameObjectsWithTag (PLAYER_TAG);
		for (int i = 0; i < searchList.Length; i++) {
			opponents.Add (searchList [i]);
		}
//		searchList = GameObject.FindGameObjectsWithTag (BOT_TAG);
//		for (int i = 0; i < searchList.Length; i++) {
//			opponents.Add (searchList [i]);
//		}

		opponents.Remove (this.gameObject);
		currentTarget = opponents [Random.Range (0, opponents.Count)];
	}

	void CheckTargetGravity() {
		if (currentTarget.tag == PLAYER_TAG) {
			currentTargetGravity = currentTarget.GetComponentInChildren<GravityAxisScript> ().gravity;
		}
		if (currentTarget.tag == BOT_TAG) {
			currentTargetGravity = currentTarget.GetComponent<Network_Bot>().gravity;
		}
		if (gravity != currentTargetGravity && !matchingGravity) {
			StartCoroutine (MatchGravity ());
		}
	}

	IEnumerator MatchGravity() {
		matchingGravity = true;
		yield return new WaitForSeconds(Random.Range(0.5f,1.5f));
		gravity = currentTargetGravity;
		matchingGravity = false;
	}

	void MoveTowardsPoint(Vector3 target) {
		anim.SetBool("Moving", true);
		Vector3 direction = target - transform.position;
		m_Rigidbody.velocity = gravVector + (direction * speed);
	}

	void MoveBackFromPoint(Vector3 target) {
		anim.SetBool("Moving", true);
		Vector3 direction = target - transform.position;
		direction *= -1;
		m_Rigidbody.velocity = gravVector + (direction * speed);
	}

	void Think() {
		// Move towards the player if far away
		if (Vector3.Distance (transform.position, currentTarget.transform.position) > 10) {
			MoveTowardsPoint (currentTarget.transform.position);
		}

		if (Vector3.Distance (transform.position, currentTarget.transform.position) < 10 && Vector3.Distance (transform.position, currentTarget.transform.position) > 2) {
			if (Random.Range (0, 1) == 0) {
				MoveTowardsPoint (currentTarget.transform.position);
			}
		}

		if (Vector3.Distance (transform.position, currentTarget.transform.position) < 2) {
			anim.SetBool("Moving", false);
			if (isFalling) {
				m_Rigidbody.velocity = gravVector;
			} else {
				m_Rigidbody.velocity = Vector3.zero;
			}
		}

		if (!rangedAttacking) {
			if (Vector3.Distance (transform.position, currentTarget.transform.position) < 15 && Vector3.Distance (transform.position, currentTarget.transform.position) > 5 ) {
				if (Random.Range (0, 10) == 0) {
					rangedAttacking = true;
					SecondaryAttack ();
				}
			}
		}


		if (currentTarget.gameObject.GetComponent<Network_CombatManager> ().isAttacking && !isAttacking) {
			if (Vector3.Distance (transform.position, currentTarget.transform.position) < 4 && Vector3.Distance (transform.position, currentTarget.transform.position) < 7) {
				MoveBackFromPoint (currentTarget.transform.position);
			}
		}

		if (Vector3.Distance (transform.position, currentTarget.transform.position) < 4 && !isAttacking) {
			Attack ();
		}
	}

	IEnumerator ActionDelay() {
		yield return new WaitForSeconds(5f);
		gravity = "y";
	}

	public void TakeDamage(float damage) {
		health -= damage;
	}

	public void Die() {
		GameObject playDeathParticle = Instantiate(particleManager.GetParticle("deathParticle"), this.transform.position, this.transform.rotation);
		GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
		NetworkServer.Spawn(corpseobject);
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
		reloading = true;
		StartCoroutine(reload());
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
		yield return new WaitForSeconds(reloadTime);

		if (playerCharacterID == "ERNN") {
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

		reloading = false;
		rangedAttacking = false;
	}

	IEnumerator D1WingOn(float time) {
		float emissionStrength = 0.1f;
		for (int i = 0; i < 10; i++) {
			emissionStrength += 0.2f;
			wingRing.GetComponent<Renderer> ().material.SetFloat ("_Emission", emissionStrength);
			yield return new WaitForSeconds (time / 10f);
		}
	}

	IEnumerator D1WingOff(float time) {
		float emissionStrength = 2f;
		for (int i = 0; i < 10; i++) {
			emissionStrength -= 0.2f;
			wingRing.GetComponent<Renderer> ().material.SetFloat ("_Emission", emissionStrength);
			yield return new WaitForSeconds (time/10f);
		}
	}

	public void NoNameShowWeapons () {
		//show nonames the weapons again
		weaponToHide.SetActive (true);
		weaponToHide2.SetActive (true);
		trailToHide.enabled = true;
		trailToHide2.enabled = true;
	}
}
