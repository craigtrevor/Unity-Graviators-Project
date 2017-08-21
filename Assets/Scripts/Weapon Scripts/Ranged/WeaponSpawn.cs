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
    public bool right;
    public float force = 2000; // the force to be applied to the weapon
    public float reloadTime = 15f;
    //	[SyncVar]
    //	public int m_localID;

    //private string m_FireButton; // the input axis that is used for firing
    private Rigidbody m_Rigidbody; // reference to the rigidbody
    private Collider2D m_Collider2D;
    [SerializeField]
    private bool m_Fired = false;          // Whether or not the weapon has been launched with this button press.

    public Animator playerAnimator;
    public GameObject weaponToHide;
    //public MonoBehaviour trailToHide;

	private ParticleSystem playSparkusRanged;
	public ParticleSystem sparkusRanged;

    // Scripts
    Network_Soundscape networkSoundscape;

    private void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider2D = GetComponent<Collider2D>();
        _sourceID = transform.name;
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
    }

    void PlayThrowSound() {
        if (m_Fired == true) {
            networkSoundscape.PlaySound(3, 1, 0.5f);
        }
    }

    [ClientCallback]
    private void Update() {
        if (!isLocalPlayer) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && m_Fired == false) {
            Fire();
            PlayThrowSound();
            StartCoroutine(reload());
        }
    }

    [Client]
    private void Fire() {
        m_Fired = true; // set the fire flag so that fire is only called once
        playerAnimator.SetTrigger("Ranged Attack");
        StartCoroutine(WaitForCurrentAnim());
    }

    private IEnumerator WaitForCurrentAnim() {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime * 0.7f);
        if (!notRigid) {
            CmdFire(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
        } else {
            CmdFire(Vector3.zero, 0f, fireTransform.forward, fireTransform.position, fireTransform.rotation);
        }
        if (weaponToHide != null) {
            weaponToHide.SetActive(false);
        }
    }

    [Command]
    private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation) {
        if (!notRigid) {
            // create an instance of the weapon and store a reference to its rigibody
            Rigidbody weaponInstance = Instantiate(weapon, position, rotation) as Rigidbody;

			//ParticleSystem playSparkusRanged = (ParticleSystem)Instantiate (sparkusRanged, this.transform.position, this.transform.rotation);
			//playSparkusRanged.Emit (1);

            // Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
            Vector3 velocity = rigidbodyVelocity + launchForce * forward;

            weaponInstance.SendMessage("SetInitialReferences", _sourceID);
            if (right) {
                weaponInstance.SendMessage("SetRight");
            }

            // Set the shell's velocity to this velocity.
            weaponInstance.velocity = velocity;

            NetworkServer.Spawn(weaponInstance.gameObject);
            Destroy(weaponInstance, 3);
        } else {
			
            // create an instance of the weapon and store a reference to its collider
            Collider2D weaponInstance = Instantiate(colliderWeapon, position, rotation) as Collider2D;
			ParticleSystem playSparkusRanged = (ParticleSystem)Instantiate (sparkusRanged, position, rotation);
			playSparkusRanged.Emit (1);


            NetworkServer.Spawn(weaponInstance.gameObject);
        }
    }

    IEnumerator reload() {
        // delay before the player can fire agian
        yield return new WaitForSeconds(reloadTime);
        m_Fired = false;
        playerAnimator.SetTrigger("Ranged Attack Reload");
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime);
        if (weaponToHide != null) {
            weaponToHide.SetActive(true);
        }
    }
}
