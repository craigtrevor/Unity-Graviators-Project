using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour {

    [SerializeField]
    GameObject player;

    private const string ULTCHARGER_TAG = "UltCharger";
    private const string PLAYER_TAG = "Player";
	private const string BOT_TAG = "NetBot";

    public float laserLength; //laser range
    public float laserDamage;
    public float laserCost;
    public const float ULT_MAX = 100f;
    public const float PASSIVE_CHARGE = 1f; // the amount of charge gained passively;

    [SerializeField]
    private bool isLasering = false;

    public Transform spawnTransform; //where the laser spawns from

    public GameObject laserParticle;
    GameObject laser;

    RaycastHit hit;
    public LayerMask mask;
    GameObject target;
    
    public float passiveCharge; // the amount of charge gained passively;

    // Scripts
    Network_PlayerManager networkPlayerManager;
    Network_Soundscape networkSoundscape;
    Network_CombatManager networkCombatManager;

    // Use this for initialization
    void Start() {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        networkPlayerManager = transform.GetComponent<Network_PlayerManager>();
        networkCombatManager = transform.GetComponent<Network_CombatManager>();
    }

    void Update()
    {
        UltInput();
    }

    void LateUpdate()
    {
        ChargeUlt();
        UltDamage();
    }

    void FixedUpdate()
    {
        SetAim();
    }

    [Client]
    void UltInput() {

        if (Input.GetButtonDown("Ultimate") && !networkCombatManager.isAttacking)
        {
            if (networkPlayerManager.currentUltimateGain >= ULT_MAX)
            {
                isLasering = true;
                networkCombatManager.isUlting = true;
                spawnTransform.gameObject.GetComponent<FaceCamera>().lerpFace = true;
                CmdFire();
            }
        }

        if (laser != null) {
            laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, hit.distance / 2f);
        }

        if (networkPlayerManager.currentUltimateGain <= 0) {
            isLasering = false;
            networkCombatManager.isUlting = false;
            Destroy(laser);
            spawnTransform.gameObject.GetComponent<FaceCamera>().lerpFace = false;
        }
    }

    [Command]
    private void CmdFire()
    {
        laser = Instantiate(laserParticle, spawnTransform.position, spawnTransform.rotation, spawnTransform);
        NetworkServer.Spawn(laser.gameObject);
    }

    void ChargeUlt() { //deals with charging the ult bar
        if (!isLasering) {
            CmdChargeUltimate(PASSIVE_CHARGE*Time.deltaTime, transform.name);
        } else {
            CmdChargeUltimate(-laserCost*Time.deltaTime, transform.name);
        }
        Debug.Log(networkPlayerManager.currentUltimateGain);

        //charge = networkPlayerManager.currentUltimateGain;
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

            //DebugLines(hit.point, colour);
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

    void UltDamage()
    {
        if (target != null && isLasering)
        {
            if (target.transform.root != transform.root && target.gameObject.tag == PLAYER_TAG)
            {
                CmdTakeDamage(target.transform.name, laserDamage, transform.name);
            }
			if (target.transform.root != transform.root && target.gameObject.tag == BOT_TAG)
			{
				target.gameObject.GetComponent<Network_Bot> ().TakeTrapDamage (laserDamage);
			}
        }
    }

    //actual damage dealing command
    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        Debug.Log(_playerID + " has been attacked with " + _damage + " by " + _sourceID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    //command that modifies the ultimate bar value
    [Command]
    void CmdChargeUltimate(float _ultimatePoints, string _playerID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcUltimateFlatCharging(_ultimatePoints, _playerID);
    }

//	[Client]
//	void OnTriggerStay(Collider other) //Ultimate charger - CB
//	{
//		if (this.gameObject.tag == PLAYER_TAG && other.gameObject.tag == ULTCHARGER_TAG)
//		{
//			//networkPlayerManager = other.GetComponent<Network_PlayerManager>();
//			networkPlayerManager = this.gameObject.GetComponent<Network_PlayerManager>();
//			Debug.Log(this.gameObject.name);
//			Debug.Log(transform.name);
//			CmdUltCharger(this.gameObject.name, chargeMax, transform.name);
//		}
//	}
/*
	[Command]
	void CmdUltCharger(string _playerID, float _charge, string _sourceID)
	{
		Debug.Log(_playerID + " is charging up teh lazor.");

		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

		networkPlayerStats.RpcUltimateCharging(_charge, transform.name);
	}*/
}
