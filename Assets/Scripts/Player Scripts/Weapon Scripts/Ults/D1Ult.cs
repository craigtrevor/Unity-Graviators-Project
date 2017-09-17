using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class D1Ult : NetworkBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name
    public Collider weapon; // prefab of the unitD1 ult hitbox

    private const string PLAYER_TAG = "Player";
    private const string ULTCHARGER_TAG = "UltCharger";

    PlayerController playerController;
    GravityAxisScript gravityScript;

    public const float STOMP_SPEED = -50f;
    public const float STOMP_COST = 100f;
    public float chargeMax = 100;

    float charge = 0;

    [SerializeField]
    private bool isStomping = false;

    bool canCharge = true;
    bool canUseUlt = false;
    bool hasStopmed = false;

    // Scripts
    Network_PlayerManager networkPlayerManager;
    Network_Soundscape networkSoundscape;

    [SerializeField]
    NetworkAnimator playerNetAnimator;

    [SerializeField]
    Animator playerAnimator;

    public float passiveCharge; // the amount of charge gained passively;

    // Use this for initialization
    void Start() {

        _sourceID = transform.name;

        gravityScript = GetComponentInChildren<GravityAxisScript>();
        playerController = GetComponentInChildren<PlayerController>();

        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();

    }

    // Update is called once per frame
    void Update() {

        ChargeUlt();
        UltInput();
    }

    private void FixedUpdate() {
        hitGround();
    }

    void hitGround() {
        if (isStomping && playerController.Grounded() && !hasStopmed) { //ult has stopped
            isStomping = false;
            playerAnimator.SetBool("UltimateLoop", false);
            playerController.isDashing = true;
            //spawn the ice thingy
            CmdSpawnUlt(this.transform.position, playerController.transform.rotation, playerController.velocity.y);
            StartCoroutine(EndUlt());
        }
    }

    IEnumerator EndUlt() {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("apple");
        playerController.isDashing = false;
    }

    void ChargeUlt() { //deals with charging the ult bar
        if (canCharge) {
            CmdChargeUltimate(passiveCharge, transform.name);
        } else {
            CmdChargeUltimate(-200f, transform.name);
        }

        charge = networkPlayerManager.currentUltimateGain;

        canUseUlt = charge >= STOMP_COST;
        //Debug.Log(charge);
        //Debug.Log("lasering: " + isLasering);
    }

    void UltInput() {
        if (Input.GetButtonDown("Ultimate") && !playerController.Grounded() && canUseUlt && !hasStopmed && !gravityScript.gravitySwitching) { //ult has started
            canCharge = false;
            isStomping = true;
            playerController.velocity.y = Mathf.Min(STOMP_SPEED, playerController.velocity.y);
            playerAnimator.SetBool("Atacking", false);
            playerAnimator.SetBool("UltimateLoop", true);

        } else if (charge <= 0f) {
            canCharge = true;
        }
    }

//    [Client]
//    void OnTriggerStay(Collider other) //Ultimate charger - CB
//{
//        if (this.gameObject.tag == PLAYER_TAG && other.gameObject.tag == ULTCHARGER_TAG) {
//            //networkPlayerManager = other.GetComponent<Network_PlayerManager>();
//            networkPlayerManager = this.gameObject.GetComponent<Network_PlayerManager>();
//            Debug.Log(this.gameObject.name);
//            Debug.Log(transform.name);
//            CmdUltCharger(this.gameObject.name, chargeMax, transform.name);
//        }
//    }

    [Command]
    void CmdUltCharger(string _playerID, float _charge, string _sourceID) {
        Debug.Log(_playerID + " is charging up teh lazor.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcUltimateCharging(_charge, transform.name);
    }

    //command that modifies the ultimate bar value
    [Command]
    void CmdChargeUltimate(float _ultimatePoints, string _playerID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcUltimateCharging(_ultimatePoints, _playerID);
    }

    [Command]
    private void CmdSpawnUlt(Vector3 position, Quaternion rotation, float SizeMeasurement) {
        // create an instance of the weapon and store a reference to its rigibody
        Collider weaponInstance = Instantiate(weapon, position, rotation) as Collider;

        NetworkServer.Spawn(weaponInstance.gameObject);
        weaponInstance.SendMessage("SetInitialReferences", _sourceID);
        weaponInstance.SendMessage("getUltSize", SizeMeasurement);
    }
}
