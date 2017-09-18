using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSpawn : NetworkBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name

    public int m_PlayerNumber = 1;
	public GameObject weapon; // prefab of the weapon
    public Collider2D colliderWeapon;
    public bool notRigid;
    public Transform fireTransform; // a child of the player where the weapon is spawned
	public Transform secondaryFireTransform; // second position for no name
    public bool right;
    public float force = 2000; // the force to be applied to the weapon
    public float reloadTime = 15f;
	public bool reloading;
    //	[SyncVar]
    //	public int m_localID;

    //private string m_FireButton; // the input axis that is used for firing
    private Rigidbody m_Rigidbody; // reference to the rigidbody
    private Collider2D m_Collider2D;
    public bool m_Fired = false;          // Whether or not the weapon has been launched with this button press.

    [SerializeField]
    Animator playerAnimator;

    [SerializeField]
    NetworkAnimator playerNetAnimator;
   
	//nonames weapons
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

    private string playerCharacterID;

	public int reloadTimer = 6;

    // Scripts
    Network_Soundscape networkSoundscape;
    Network_PlayerManager networkPlayerManagerScript;
	private Network_CombatManager combatManager;

    private void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider2D = GetComponent<Collider2D>();
        _sourceID = transform.name;
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
		combatManager = transform.GetComponent<Network_CombatManager> ();
        networkPlayerManagerScript = transform.GetComponent<Network_PlayerManager>();
        playerCharacterID = networkPlayerManagerScript.playerCharacterID;
    }

    void PlayThrowSound()
    {
        if (m_Fired == true && playerCharacterID == "ERNN")
        {
            networkSoundscape.PlaySound(8, 1, 0.2f, 0.0f);
        }

        else if (m_Fired == true && playerCharacterID == "SPKS")
        {
            networkSoundscape.PlaySound(9, 1, 0.2f, 0.0f);
        }

        else if (m_Fired == true && playerCharacterID == "UT-D1")
        {
            networkSoundscape.PlaySound(10, 1, 0.2f, 0.0f);
        }
    }

    [ClientCallback]
    private void Update() {
        if (!isLocalPlayer) {
            return;
        }

		if (Input.GetButtonDown("Fire2") && !m_Fired && !combatManager.isUlting && !combatManager.isAttacking) {
			reloading = true;
			StartCoroutine(reload());
			Fire();
            PlayThrowSound();
        }
    }

    [Client]
    private void Fire() {
        m_Fired = true; // set the fire flag so that fire is only called once
        playerAnimator.SetBool("Attacking", false);
        playerNetAnimator.SetTrigger("Ranged Attack");
        //StartCoroutine(WaitForCurrentAnim());
    }

	//Ranged attack is called by the animation at the appropriate time, check AnimationEventPassalong.cs
	public void RangedAttack(AnimationEvent animEvent) {

        if (!isLocalPlayer)
            return;

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
        combatManager.safeList.Add(weaponInstance);

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
            playerAnimator.SetBool("Attacking", false);
            playerNetAnimator.SetTrigger("Ranged Attack Reload");
		}

		if (playerCharacterID == "SPKS") {
            playerAnimator.SetBool("Attacking", false);
            playerNetAnimator.SetTrigger("Ranged Attack Reload");
			sparkusReloadBall.SetActive (true);
		}

		if (playerCharacterID == "UT-D1") {
			StartCoroutine (D1WingOn (0.5f));
            playerAnimator.SetBool("Attacking", false);
		}

        networkSoundscape.PlayNonNetworkedSound(13, 1, 0.1f);

        // allow the player to fire again

        m_Fired = false;
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

    public void InstantReload() {
        if (playerCharacterID == "ERNN") {
            //play reload anim and wait for it to trigger
            playerAnimator.SetBool("Attacking", false);
            playerNetAnimator.SetTrigger("Ranged Attack Reload");
        }

        if (playerCharacterID == "SPKS") {
            sparkusReloadBall.SetActive(true);
        }

        if (playerCharacterID == "UT-D1") {
            wingRing.GetComponent<Renderer>().material.color = Color.cyan;
        }

        networkSoundscape.PlayNonNetworkedSound(13, 1, 0.1f);

        // allow the player to fire again

        m_Fired = false;
        reloading = false;
    }

	public void NoNameShowWeapons (AnimationEvent animEvent) {
		//show nonames the weapons again
		weaponToHide.SetActive (true);
		weaponToHide2.SetActive (true);
		trailToHide.enabled = true;
		trailToHide2.enabled = true;
	}
}
