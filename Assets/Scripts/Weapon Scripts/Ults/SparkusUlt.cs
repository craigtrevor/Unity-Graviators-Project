using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour {

    private const string ULTCHARGER_TAG = "UltCharger";
    private const string PLAYER_TAG = "Player";

    public float laserLength; //laser range
    public float laserDamage;
    public float laserCost;
    public const float ULT_MAX = 100f;

    [SerializeField]
    private bool isLasering = false;

    public Transform spawnTransform; //where the laser spawns from

    public GameObject laserParticle;
    GameObject laser;

    RaycastHit hit;
    public LayerMask mask;
    GameObject target;

    float charge = 0; // the amount of charge
    public float passiveCharge; // the amount of charge gained passively;

    // Scripts
    Network_PlayerManager networkPlayerManager;
    Network_Soundscape networkSoundscape;

    public Animator playerAnimator;

    // Use this for initialization
    void Start() {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();
    }

    // Update is called once per frame
    void Update() {
        ChargeUlt();
        UltInput();
        UltDamage();
    }

    void FixedUpdate() {
        SetAim();
    }

    void UltInput() {

        if (Input.GetKeyDown(KeyCode.F)) {
            if (charge >= ULT_MAX) {
                isLasering = true;
                laser = Instantiate(laserParticle, spawnTransform.position, spawnTransform.rotation, spawnTransform);
                spawnTransform.gameObject.GetComponent<FaceCamera>().lerpFace = true;
            }
        }

        if (laser != null) {
            laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, hit.distance / 2f);
        }

        if (charge <= 0) {
            isLasering = false;
            Destroy(laser);
            spawnTransform.gameObject.GetComponent<FaceCamera>().lerpFace = false;
        }
    }

    void ChargeUlt() { //deals with charging the ult bar
        if (!isLasering) {
            CmdChargeUltimate(passiveCharge, transform.name);
        } else {
            CmdChargeUltimate(-laserCost, transform.name);
        }

        charge = networkPlayerManager.currentUltimateGain;
        //Debug.Log(charge);
        //Debug.Log("lasering: " + isLasering);
    }

    void SetAim() {

        Color colour; //for debug line

        if (Physics.Raycast(spawnTransform.position, spawnTransform.forward, out hit, Mathf.Infinity, mask.value)) {

            if (hit.distance > laserLength) {
                colour = Color.red;
                target = null;
            } else {
                colour = Color.green; //can hit
                target = hit.transform.gameObject;
            }

            DebugLines(hit.point, colour);
        }
    }

    void DebugLines(Vector3 centre, Color colour) {

        Debug.DrawLine(spawnTransform.position, hit.point, colour);
        if (target != null) {
            Debug.DrawLine(spawnTransform.position, target.transform.position, Color.yellow);
        }

        for (float x = -1; x <= 1; x += 0.5f) {
            for (float y = -1; y <= 1; y += 0.5f) {
                for (float z = -1; z <= 1; z += 0.5f) {
                    Debug.DrawLine(centre, centre + new Vector3(x, y, z), Color.cyan);
                }
            }
        }
    }

    [Client]
    void UltDamage() {

        if (target != null) {
            if (target.transform.root != transform.root && target.gameObject.tag == PLAYER_TAG) {
                Debug.Log("Hit Player!");

                CmdTakeDamage(target.gameObject.name, laserDamage, transform.name);
            }
        }

    }

    //actual damage dealing command
    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    //command that modifies the ultimate bar value
    [Command]
    void CmdChargeUltimate(float _ultimatePoints, string _playerID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcUltimateCharging(_ultimatePoints, _playerID);
    }
}
