using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSpawn : NetworkBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name

    public int m_PlayerNumber = 1;
    public Rigidbody weapon; // prefab of the weapon
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
    [SerializeField]
    private bool m_Fired = false;          // Whether or not the weapon has been launched with this button press.

    public Animator playerAnimator;
   
	//nonames weapons
	public GameObject weaponToHide;
	public GameObject weaponToHide2;
    //public MonoBehaviour trailToHide;

	private ParticleSystem playSparkusRanged;
	public GameObject sparkusRanged;
	public GameObject sparkusReloadBall;

	private ParticleSystem playD1Ranged;
	public ParticleSystem D1Ranged;
	public GameObject wingRing;

    private string playerCharacterID;

    // Scripts
    Network_Soundscape networkSoundscape;
    Network_PlayerManager networkPlayerManagerScript;

    private void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider2D = GetComponent<Collider2D>();
        _sourceID = transform.name;
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        networkPlayerManagerScript = transform.GetComponent<Network_PlayerManager>();
        playerCharacterID = networkPlayerManagerScript.playerCharacterID;
    }

    void PlayThrowSound()
    {
        if (m_Fired == true && playerCharacterID == "ERNN")
        {
            networkSoundscape.PlaySound(3, 1, 0.0f);
        }

        else if (m_Fired == true && playerCharacterID == "SPKS")
        {
            networkSoundscape.PlaySound(17, 1, 0.0f);
        }

        else if (m_Fired == true && playerCharacterID == "UT-D1")
        {
            networkSoundscape.PlaySound(18, 1, 0.0f);
        }
    }

    [ClientCallback]
    private void Update() {
        if (!isLocalPlayer) {
            return;
        }

		if (Input.GetButtonDown("Fire2") && m_Fired == false) {
			reloading = true;
			StartCoroutine(reload());
			Fire();
            PlayThrowSound();
        }
    }

    [Client]
    private void Fire() {
        m_Fired = true; // set the fire flag so that fire is only called once
        playerAnimator.SetTrigger("Ranged Attack");
        //StartCoroutine(WaitForCurrentAnim());
    }

	//Ranged attack is called by the animation at the appropriate time, check AnimationEventPassalong.cs
	public void RangedAttack(AnimationEvent animEvent) {

		//Nonames Attack
		if (playerCharacterID == "ERNN") {
			CmdFire(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
			right = true;
			CmdFire(m_Rigidbody.velocity, force, secondaryFireTransform.forward, secondaryFireTransform.position, secondaryFireTransform.rotation);
			right = false;
			weaponToHide.SetActive(false);
			weaponToHide2.SetActive(false);
		}

		//D1s Attack
		if (playerCharacterID == "UT-D1") {
			CmdFire(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
			wingRing.GetComponent<Renderer> ().material.color = Color.black;
			ParticleSystem playD1Ranged = (ParticleSystem)Instantiate (D1Ranged, this.transform.position, this.transform.rotation);
			playD1Ranged.transform.parent = this.transform;
			playD1Ranged.Emit (1);
		}

		//Sparkus Attack
		if (playerCharacterID == "SPKS") {
			CmdFire(Vector3.zero, 0f, fireTransform.forward, fireTransform.position, fireTransform.rotation);
			sparkusReloadBall.SetActive (false);
		}
	}

    [Command]
    private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation) {
        if (!notRigid) {
            // create an instance of the weapon and store a reference to its rigibody
            Rigidbody weaponInstance = Instantiate(weapon, position, rotation) as Rigidbody;

            // Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
            Vector3 velocity = rigidbodyVelocity + launchForce * forward;

			if (playerCharacterID == "UT-D1") {
				velocity = new Vector3 (velocity.x * 3, velocity.y * 3, velocity.z * 3);
			}
				
            weaponInstance.SendMessage("SetInitialReferences", _sourceID);
            if (right) {
                weaponInstance.SendMessage("SetRight");
            }

            // Set the shell's velocity to this velocity.
            weaponInstance.velocity = velocity;

            NetworkServer.Spawn(weaponInstance.gameObject);
     //     Destroy(weaponInstance, 3);
        } else {

            // create an instance of the weapon and store a reference to its collider
            Collider2D weaponInstance = Instantiate(colliderWeapon, position, rotation) as Collider2D;

            weaponInstance.SendMessage("SetInitialReferences", _sourceID);

            NetworkServer.Spawn(weaponInstance.gameObject);
        }
    }

    IEnumerator reload() {
        // delay before the player can fire agian
        yield return new WaitForSeconds(reloadTime);

        if (playerCharacterID == "ERNN") {
			//play reload anim and wait for it to trigger
            playerAnimator.SetTrigger("Ranged Attack Reload");
		}

		if (playerCharacterID == "SPKS") {
			sparkusReloadBall.SetActive (true);
		}

		if (playerCharacterID == "UT-D1") {
			wingRing.GetComponent<Renderer> ().material.color = Color.cyan;
		}

        networkSoundscape.PlayNonNetworkedSound(19, 1);

        // allow the player to fire again

        m_Fired = false;
		reloading = false;
    }

    public void InstantReload() {
        if (playerCharacterID == "ERNN") {
            //play reload anim and wait for it to trigger
            playerAnimator.SetTrigger("Ranged Attack Reload");
        }

        if (playerCharacterID == "SPKS") {
            sparkusReloadBall.SetActive(true);
        }

        if (playerCharacterID == "UT-D1") {
            wingRing.GetComponent<Renderer>().material.color = Color.cyan;
        }

        networkSoundscape.PlayNonNetworkedSound(19, 1);

        // allow the player to fire again

        m_Fired = false;
        reloading = false;
    }

	public void NoNameShowWeapons (AnimationEvent animEvent) {
		//show nonames the weapons again
		weaponToHide.SetActive (true);
		weaponToHide2.SetActive (true);
	}
}
