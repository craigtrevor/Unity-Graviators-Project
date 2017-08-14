using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusRangeSpawn : NetworkBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name

    public int m_PlayerNumber = 1;
    public GameObject weapon; // prefab of the weapon
    public Transform fireTransform; // a child of the player where the weapon is spawned
    //public float force = 2000; // the force to be applied to the weapon
    public float reloadTime = 15f;
    //	[SyncVar]
    //	public int m_localID;

    //private string m_FireButton; // the input axis that is used for firing
    private Rigidbody m_Rigidbody; // reference to the rigidbody
    [SerializeField]
    private bool m_Fired = false;          // Whether or not the weapon has been launched with this button press.

    //public Animator playerAnimator;


    private void Start() {
        m_Rigidbody = GetComponent<Rigidbody>();
        _sourceID = transform.name;
    }

    [ClientCallback]
    private void Update() {

        if (!isLocalPlayer) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && m_Fired == false) {
            Fire();
            StartCoroutine(reload());
        }
    }

    [Client]
    private void Fire() {
        m_Fired = true; // set the fire flag so that fire is only called once
        //playerAnimator.SetTrigger("Ranged Attack");
        StartCoroutine(WaitForCurrentAnim());
    }

    private IEnumerator WaitForCurrentAnim() {
        yield return new WaitForSeconds(0.1f);
        CmdFire(m_Rigidbody.velocity, fireTransform.forward, fireTransform.position, fireTransform.rotation);
    }

    [Command]
    private void CmdFire(Vector3 rigidbodyVelocity, Vector3 forward, Vector3 position, Quaternion rotation) {
        // create an instance of the weapon and store a reference to its rigibody
        GameObject weaponInstance = Instantiate(weapon, position, rotation) as GameObject;
        // Create a velocity that is the players velocity and the launch force in the fire position's forward direction.
        //Vector3 velocity = rigidbodyVelocity + launchForce * forward;

        weaponInstance.SendMessage("SetInitialReferences", _sourceID);

        // Set the shell's velocity to this velocity.
        //weaponInstance.velocity = velocity;

        NetworkServer.Spawn(weaponInstance.gameObject);
        Destroy(weaponInstance, 3);
    }

    IEnumerator reload() {
        // delay before the player can fire agian
        yield return new WaitForSeconds(reloadTime);
        m_Fired = false;
        //playerAnimator.SetTrigger("Ranged Attack Reload");
        yield return new WaitForSeconds(0.1f);
        //weaponToHide.SetActive(true);
    }
}