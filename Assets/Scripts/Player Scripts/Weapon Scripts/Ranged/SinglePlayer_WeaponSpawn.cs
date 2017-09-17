using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer_WeaponSpawn : MonoBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name

    public int m_PlayerNumber = 1;
    public Rigidbody weapon; // prefab of the weapon
    public Transform fireTransform; // a child of the player where the weapon is spawned
	public Transform fireTransformSecondary;
    public float force = 2000; // the force to be applied to the weapon
    public float reloadTime = 5f;
    //	[SyncVar]
    //	public int m_localID;

	public bool reloading;

    //private string m_FireButton; // the input axis that is used for firing
    private Rigidbody m_Rigidbody; // reference to the rigidbody
    [SerializeField]
    private bool m_Fired = false;          // Whether or not the weapon has been launched with this button press.

    public Animator playerAnimator;
    public GameObject weaponToHide;
	public GameObject weaponToHide2;
    public MonoBehaviour trailToHide;
    NonNetworked_Soundscape soundscape;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        soundscape = GetComponent<NonNetworked_Soundscape>();
        _sourceID = transform.name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && m_Fired == false)
        {
            Fire();
            PlayThrowSound();
            StartCoroutine(reload());
        }
    }

    private void Fire()
    {
        m_Fired = true; // set the fire flag so that fire is only called once
        playerAnimator.SetTrigger("Ranged Attack");
    }

	public void RangedAttack()
    {
        weaponToHide.SetActive(false);
        ThrowWeapon(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
		ThrowWeapon(m_Rigidbody.velocity, force, fireTransformSecondary.forward, fireTransformSecondary.position, fireTransformSecondary.rotation);
    }

    void ThrowWeapon(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
    {
        // create an instance of the weapon and store a reference to its rigibody
        Rigidbody weaponInstance = Instantiate(weapon, position, rotation) as Rigidbody;
		if (position == fireTransformSecondary.position) {
			weaponInstance.GetComponent<SinglePlayer_ThrownSword> ().SetRight ();
		}
        // Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
        Vector3 velocity = rigidbodyVelocity + launchForce * forward;

        // Set the shell's velocity to this velocity.
        weaponInstance.velocity = velocity;

        //Destroy(weaponInstance, 3);
		reloading = true;
    }

    IEnumerator reload()
    {
        // delay before the player can fire agian
        yield return new WaitForSeconds(reloadTime);
        m_Fired = false;
        playerAnimator.SetTrigger("Ranged Attack Reload");
		reloading = false;
    }


    void PlayThrowSound()
    {
        if (m_Fired == true)
        {
            soundscape.PlayNonNetworkedSound(8, 1, 0.2f);
        }
    }

    public void NoNameShowWeapons() {
		weaponToHide.SetActive(true);
		weaponToHide2.SetActive(true);
	}
}
