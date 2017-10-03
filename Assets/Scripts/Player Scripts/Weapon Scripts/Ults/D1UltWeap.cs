using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class D1UltWeap : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
    private const string BOT_TAG = "NetBot";

    public GameObject colliderFrame;

    //[SerializeField]
    private string sourceID;
    private string attackedByEntity;

    Collider[] hitColliders;

    public float damage;
    float stun = 1f;

    Network_PlayerManager networkPlayerManager;
    PlayerController playerController;

    void SetUltRefs(string[] _sourceParams) {
        sourceID = _sourceParams[0];
        attackedByEntity = _sourceParams[1];
    }

    private void Update() {
        //Debug.Log("ID: " + sourceID + "| Tag: " + attackedByEntity);
        //Debug.Log("Size:" + damage);
    }

    public void getUltSize(float sizeMeasurement) {
        float multiplierPercent = Mathf.Clamp((-sizeMeasurement - 50f) / 50f, 0f, 1f);
        float sizeMultiplier = Mathf.Max(multiplierPercent * 3f, 0.5f);
        float damageMultiplier = multiplierPercent * 4f;
        float stunMultiplier = 2f + multiplierPercent * 5f;
        //Debug.Log(damageMultiplier * damage);

        this.transform.localScale = Vector3.one * sizeMultiplier;
        damage *= damageMultiplier;
        stun *= stunMultiplier;
        Debug.Log(stun);
    }

    // Use this for initialization
    void Start() {
        Destroy(this.gameObject, 2f);
        //StompDamage(damage);
    }
    /*
    [Client]
    public void StompDamage(float damage) {
        //hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);
        hitColliders = Physics.OverlapBox(colliderFrame.transform.position, colliderFrame.transform.localScale/2f);        
        
        foreach (Collider hitCol in hitColliders) {
            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && hitCol.transform.root.name != sourceID) {
                //Debug.Log("i hit " + hitCol);
                CmdTakeDamage(hitCol.gameObject.name, da    mage, sourceID);
            }
        }
    }*/

    //[Client]
    private void OnTriggerStay(Collider other) {/*
        if (other.transform.root != transform.root && other.gameObject.tag == PLAYER_TAG && other.transform.root.name != sourceID) {
            CmdTakeDamage(other.gameObject.name, damage, sourceID);
        }
		if (other.transform.root != transform.root && other.gameObject.tag == BOT_TAG && other.transform.root.name != sourceID) {
			other.gameObject.GetComponent<Network_Bot> ().TakeDamage (sourceID, damage);
		}*/

        if (other.transform.root != transform.root && (other.gameObject.tag == PLAYER_TAG || other.gameObject.tag == BOT_TAG) && other.transform.name != sourceID) {

            if (other.gameObject.tag == PLAYER_TAG) {

                SendDamage(other.gameObject.name, damage*Time.deltaTime, sourceID);
                other.gameObject.GetComponent<Network_CombatManager>().StunForSeconds(stun);
                //Debug.Log("sourceID is: " + sourceID);
                //Debug.Log("hit: " + other.transform.name);
            }

            if (other.gameObject.tag == BOT_TAG) {
                other.gameObject.GetComponent<Network_Bot>().TakeDamage(sourceID, damage * Time.deltaTime);
                other.gameObject.GetComponent<Network_Bot>().Stun(stun);
            }
            //Die();

        }
    }

    void SendDamage(string _playerID, float _damage, string _sourceID) {
        if (attackedByEntity == PLAYER_TAG) {
            CmdTakeDamageByPlayer(_playerID, _damage, _sourceID);
        } else if (attackedByEntity == BOT_TAG) {
            CmdTakeDamageByBot(_playerID, _damage, _sourceID);
        }
    }

    [Command]
    void CmdTakeDamageByPlayer(string _playerID, float _damage, string _sourceID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    [Command]
    void CmdTakeDamageByBot(string _playerID, float _damage, string _sourceID) {
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        networkPlayerStats.RpcTakeDamageByBot(_damage, _sourceID);
    }

    /*
    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {

        Debug.Log(_playerID + " has been attacked with " + _damage + " by " + _sourceID);

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }*/
}
