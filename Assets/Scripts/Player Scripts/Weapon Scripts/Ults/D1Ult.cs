using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class D1Ult : NetworkBehaviour {

    public string _sourceID; //set the source id to the player that throws it. this is set by transform.name
    public string _sourceTag;
    public GameObject weapon; // prefab of the unitD1 ult 

    private const string PLAYER_TAG = "Player";
    private const string BOT_TAG = "NetBot";
    private const string ULTCHARGER_TAG = "UltCharger";

    PlayerController playerController;
    GravityAxisScript gravityScript;

    public const float STOMP_SPEED = -50f;
    public const float STOMP_COST = 100f;
    public float chargeMax = 100;
    public const float PASSIVE_CHARGE = 1f; // the amount of charge gained passively;

    public float damage = 30f;
    public float stun = 5f;

    //float charge = 0;

    [SerializeField]
    private bool isStomping = false;

    bool canCharge = true;
    bool canUseUlt = false;
    bool hasStopmed = false;

    // Scripts
    Network_Soundscape networkSoundscape;
    Network_PlayerManager networkPlayerManagerScript;
    private Network_CombatManager combatManager;

    [SerializeField]
    NetworkAnimator playerNetAnimator;

    [SerializeField]
    Animator playerAnimator;

    private Collider[] hitColliders;

    // Use this for initialization
    void Start() {

        _sourceID = transform.root.name;
        _sourceTag = gameObject.tag;

        gravityScript = GetComponentInChildren<GravityAxisScript>();
        playerController = GetComponentInChildren<PlayerController>();

        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        combatManager = transform.GetComponent<Network_CombatManager>();
        networkPlayerManagerScript = transform.GetComponent<Network_PlayerManager>();

    }

    // Update is called once per frame
    void Update() {

        ChargeUlt();
        UltInput();
    }

    private void FixedUpdate() {
        hitGround();
        Collide();
    }

    public GameObject debugCube;

    void Collide() {

        if (combatManager.isUlting) {
            if (isStomping) {//while falling

                hitColliders = Physics.OverlapBox(playerController.transform.position + playerController.transform.up * 0.5f, Vector3.one * 3.5f / 2f);
                //debugCube.transform.position = playerController.transform.position + playerController.transform.up * 0.5f;
                //debugCube.transform.localScale = Vector3.one * 3.5f;

            } else { //during icicles

                hitColliders = Physics.OverlapBox(playerController.transform.position + playerController.transform.up * 1.5f, new Vector3(9f, 6f, 9f) / 2f);
               //debugCube.transform.position = playerController.transform.position + playerController.transform.up * 1.5f;
               //debugCube.transform.localScale = new Vector3(9f, 6f, 9f);
            }

            foreach (Collider hitCol in hitColliders) {
                if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG) {
                    Debug.Log("Hit Player!");

                    CmdTakeDamage(hitCol.gameObject.name, damage*Time.deltaTime, transform.name);
                }
                if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == BOT_TAG) {

                    hitCol.gameObject.GetComponent<Network_Bot>().TakeDamage(transform.name, damage * Time.deltaTime);
                }
            }
        } else {
            //debugCube.transform.position = playerController.transform.position;
            //debugCube.transform.localScale = Vector3.one * 0f;
        }
    }

    //actual damage dealing command
    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    void hitGround() {
        if (isStomping && playerController.Grounded() && !hasStopmed) { //ult has stopped
            isStomping = false;
            playerAnimator.SetBool("UltimateLoop", false);
            CmdChargeUltimate(-STOMP_COST, transform.name);
            CmdSpawnUlt(playerController.transform.position - playerController.transform.up*0.5f, playerController.transform.rotation, playerController.velocity.y);
            //spawn the ice thingies
            /*CmdSpawnUlt(playerController.transform.position + playerController.transform.forward * 6f, playerController.transform.rotation, playerController.velocity.y);
            CmdSpawnUlt(playerController.transform.position + playerController.transform.forward * -6f, playerController.transform.rotation, playerController.velocity.y);
            CmdSpawnUlt(playerController.transform.position + playerController.transform.right * 6f, playerController.transform.rotation, playerController.velocity.y);
            CmdSpawnUlt(playerController.transform.position + playerController.transform.right * -6f, playerController.transform.rotation, playerController.velocity.y);*/

            StartCoroutine(EndUlt());
        }
    }

    IEnumerator EndUlt() {
        yield return new WaitForSeconds(2f);
        //Debug.Log("apple");
        combatManager.isUlting = false;
    }

    void ChargeUlt() { //deals with charging the ult bar
        if (canCharge) {
            CmdChargeUltimate(PASSIVE_CHARGE * Time.deltaTime, transform.name);
        } else {
        }

        //charge = networkPlayerManager.currentUltimateGain;

        canUseUlt = networkPlayerManagerScript.currentUltimateGain >= STOMP_COST;
        //Debug.Log(charge);
        //Debug.Log("lasering: " + isLasering);
    }

    void UltInput() {
        if (Input.GetButtonDown("Ultimate") && !playerController.Grounded() && canUseUlt && !hasStopmed && !gravityScript.gravitySwitching && !combatManager.isAttacking) { //ult has started
            canCharge = false;
            isStomping = true;
            combatManager.isUlting = true;

            playerController.velocity.y = Mathf.Min(STOMP_SPEED, playerController.velocity.y);

            playerAnimator.SetBool("UltimateLoop", true);
            playerAnimator.SetTrigger("StartUltimate");

        } else if (networkPlayerManagerScript.currentUltimateGain <= 0f) {
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
    /*
        [Command]
        void CmdUltCharger(string _playerID, float _charge, string _sourceID) {
            Debug.Log(_playerID + " is charging up teh lazor.");

            Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

            networkPlayerStats.RpcUltimateCharging(_charge, transform.name);
        }*/

    //command that modifies the ultimate bar value
    [Command]
    private void CmdChargeUltimate(float _ultimatePoints, string _playerID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcUltimateFlatCharging(_ultimatePoints, _playerID);
    }

    [Command]
    private void CmdSpawnUlt(Vector3 position, Quaternion rotation, float yVel) {
        // create an instance of the weapon and store a reference to its rigibody
        GameObject weaponInstance = Instantiate(weapon, position, rotation);

        string[] ultSourceParams = new string[2];
        ultSourceParams[0] = _sourceID;
        ultSourceParams[1] = _sourceTag;

        weaponInstance.SendMessage("SetUltRefs", ultSourceParams, SendMessageOptions.RequireReceiver);
        //weaponInstance.SendMessage("GetYVel", yVel, SendMessageOptions.RequireReceiver);

        NetworkServer.Spawn(weaponInstance.gameObject);
    }
}
