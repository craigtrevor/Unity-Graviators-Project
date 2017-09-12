﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class D1UltWeap : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public GameObject colliderFrame;

    private string sourceID;

    Collider[] hitColliders;

    public float damage;

    Network_PlayerManager networkPlayerManager;
    PlayerController playerController;

    void SetInitialReferences(string _sourceID) {
        sourceID = _sourceID;
    }

    public void getUltSize(float sizeMeasurement) {
        float multiplierPercent = Mathf.Clamp((-sizeMeasurement - 50f) / 50f, 0f, 1f);
        float sizeMultiplier = Mathf.Max(multiplierPercent * 2.5f, 0.5f);
        float damageMultiplier = multiplierPercent * 5f;
        Debug.Log(damageMultiplier*damage);

        this.transform.localScale = Vector3.one * sizeMultiplier;
        damage *= damageMultiplier;
    }

    // Use this for initialization
    void Start() {
        Destroy(this.gameObject, 3f);
        StompDamage(damage);
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
    }

    [Client]
    public void StompDamage(float damage) {
        //hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);
        hitColliders = Physics.OverlapBox(colliderFrame.transform.position, colliderFrame.transform.localScale/2f);
        
        foreach (Collider hitCol in hitColliders) {
            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && sourceID != hitCol.gameObject.name) {
                Debug.Log("Hit Player " + hitCol.transform.name);
                Debug.Log("sourceid is " + sourceID);

                CmdTakeDamage(hitCol.gameObject.name, damage, transform.name);
            }
        }
    }

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }
}
