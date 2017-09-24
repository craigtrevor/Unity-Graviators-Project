using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Network_DevKeys : NetworkBehaviour {

    /* dev mode instruction
	 * hold "\" to enable dev mode
	 * press 1 to take damage
	 * hold 2 to gain health
	 * press 3 to charge ult
	 * press 4 to set range cool down to 0
	 * press 5 to refil gravity charges
	 * press 6 to go back to player spawn (player will keep thier current gravity set)
	 */

    public bool devmodeenabled = false;
    [SerializeField]
    Network_PlayerManager networkPlayerManager;
    [SerializeField]
    Network_CombatManager networkCombatManager;
    [SerializeField]
    private Vector3 spawnpostion;

    public GameObject devStats;
    public Text playerDamage;


    void Start() {
        networkPlayerManager = gameObject.GetComponent<Network_PlayerManager>();
        networkCombatManager = gameObject.GetComponent<Network_CombatManager>();
        spawnpostion = gameObject.transform.position;
        devStats.SetActive(false);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Backslash) || Input.GetKey(KeyCode.Slash)) {
            devmodeenabled = true;
        } else {
            devmodeenabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Period)) {
            if (!devStats) {
                devStats.SetActive(true);
            }
            if (devStats) {
                devStats.SetActive(false);
            }
            if (!devmodeenabled) {
                devmodeenabled = true;
            }
            if (devmodeenabled) {
                devmodeenabled = false;
            }
        }

        if (devStats) {
            playerDamage.text = "Player Damage: " + networkCombatManager.playerDamage;
        }

        if (devmodeenabled == true) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // key 1 to heal
            {

                CmdHealthRegen(gameObject.name, 10 * 10f / Time.deltaTime, transform.name);

            }
            if (Input.GetKey(KeyCode.Alpha2)) // hold 2 to damage
            {
                CmdTakeDamage(gameObject.name, 100 * Time.deltaTime, transform.name);

            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) // key 3 to replenish/deplete ult
            {
                if (Input.GetKey(KeyCode.LeftAlt)) {
                    networkPlayerManager.currentUltimateGain -= 100;
                } else {
                    networkPlayerManager.currentUltimateGain += 100;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) // key 4 to reload ranged
            {
                gameObject.GetComponent<WeaponSpawn>().InstantReload();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) // key 5 to fill/empty gravity charges
            {
                if (Input.GetKey(KeyCode.LeftAlt)) {
                    gameObject.GetComponentInChildren<GravityAxisScript>().gravityCharge = 0;
                } else {
                    gameObject.GetComponentInChildren<GravityAxisScript>().gravityCharge = 5;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) // key 6 to go to spawn;
            {
                gameObject.transform.position = spawnpostion;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7)) // key 7 to selfstun for 2 secodns
            {
                networkCombatManager.StunForSeconds(2);
            }
            if (Input.GetKeyDown(KeyCode.C)) // key c to sturn on clash
            {
                gameObject.GetComponent<Network_CombatManager>().clashActive = true;
            }
            if (Input.GetKeyDown(KeyCode.A)) // key a to turn on always attack
            {
                gameObject.GetComponent<Network_CombatManager>().alwaysAttack = true;
            }
        }
    }



    [Command] // to take damage
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        networkPlayerStats.RpcTakDamageByBot(_damage, transform.name);
    }

    [Command] // to heal
    void CmdHealthRegen(string _playerID, float _heal, string _sourceID) {
        Debug.Log(_playerID + "is regenerating.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcHealthRegenerate(_heal, transform.name);
    }

}
