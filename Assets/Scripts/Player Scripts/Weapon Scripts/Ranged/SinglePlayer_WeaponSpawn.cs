using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer_WeaponSpawn : MonoBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name

    public int m_PlayerNumber = 1;
    ////public Rigidbody weapon; // prefab of the weapon
	public GameObject weapon; // prefab of the weapon
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
	public MeleeWeaponTrail trailToHide;
	public MeleeWeaponTrail trailToHide2;
    NonNetworked_Soundscape soundscape;
	[SerializeField]
	private SinglePlayer_CombatManager combatManager;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        soundscape = GetComponent<NonNetworked_Soundscape>();
		combatManager = transform.GetComponent<SinglePlayer_CombatManager> ();
        _sourceID = transform.name;
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Mouse1) && m_Fired == false && !combatManager.isAttacking)
        {
            Fire();
            //PlayThrowSound();
            StartCoroutine(reload());
        }
    }

    private void Fire()
    {
        m_Fired = true; // set the fire flag so that fire is only called once
        //playerAnimator.SetTrigger("Ranged Attack");
		playerAnimator.SetBool("RangedAttacking",true);
		//RangedAttack ();// added since range animations are not triggering
    }

	public void RangedAttack()
    {
        weaponToHide.SetActive(false);
		weaponToHide2.SetActive (false);
		trailToHide.enabled = false;
		trailToHide2.enabled = false;
        ThrowWeapon(m_Rigidbody.velocity, force, fireTransform.forward, fireTransform.position, fireTransform.rotation);
		ThrowWeapon(m_Rigidbody.velocity, force, fireTransformSecondary.forward, fireTransformSecondary.position, fireTransformSecondary.rotation);
    }

    void ThrowWeapon(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
    {
        // create an instance of the weapon and store a reference to its rigibody
		GameObject weaponInstance = Instantiate(weapon, position, rotation);
        ////Rigidbody weaponInstance = Instantiate(weapon, position, rotation) as Rigidbody;
		if (position == fireTransformSecondary.position) {
			weaponInstance.GetComponent<SinglePlayer_ThrownSword> ().SetRight ();
		}
        // Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
		PlayThrowSound();
        Vector3 velocity = rigidbodyVelocity + launchForce * forward;

        // Set the shell's velocity to this velocity.
		weaponInstance.GetComponent<Rigidbody>().velocity = velocity;
        ////weaponInstance.velocity = velocity;

        //Destroy(weaponInstance, 3);
		reloading = true;
    }

    IEnumerator reload()
    {
        // delay before the player can fire agian
        yield return new WaitForSeconds(reloadTime);
        m_Fired = false;
        //playerAnimator.SetTrigger("Ranged Attack Reload");
		playerAnimator.SetBool("Ranged Attack Reload" ,true);
		playerAnimator.SetBool("RangedAttacking",false);
		soundscape.PlayNonNetworkedSound (13, 1, 0.2f);
		//NoNameShowWeapons (); // added since range animations are not triggering
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

		trailToHide.enabled = true;
		trailToHide2.enabled = true;
		weaponToHide.SetActive(true);
		weaponToHide2.SetActive(true);
	}
}
