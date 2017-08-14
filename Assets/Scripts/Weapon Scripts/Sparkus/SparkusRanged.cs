using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusRanged : NetworkBehaviour {

    private string sourceID;

    public float stunTime = 5f;

    private Collider[] hitColliders;
    private Vector3 attackOffset;
    private float attackRadius;
    private const string PLAYER_TAG = "Player";

    private float creationTime;
    private float timeSinceCreation;

    private void Start() {
        creationTime = Time.time;
    }

    private void Update() {

        timeSinceCreation = Time.time - creationTime;

        transform.Translate(Vector3.forward * 0.2f);
        transform.localScale += new Vector3(0.2f, 0.2f, 0f);

        if (timeSinceCreation > 2f) {
            Destroy(this.gameObject);
        }

        //Physics.IgnoreLayerCollision (10, 30); // the ranged object will ignore the local player layer
        hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

        foreach (Collider hitCol in hitColliders) {
            //Debug.Log("i have hit " + hitCol.gameObject.tag);

            if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && hitCol.transform.name != sourceID) {

                Debug.Log("I have hit " + hitCol.gameObject.name);
                CmdStun(hitCol.gameObject, stunTime, sourceID);

            }
        }

    }

    [Client]

    /*private void OnTriggerEnter(Collider other) {

        if (other.tag != "Player") {
            Debug.Log("i have hit " + other.tag);

            //Destroy(this.gameObject);
        } else {

            //Physics.IgnoreLayerCollision (10, 30); // the ranged object will ignore the local player layer
            hitColliders = Physics.OverlapSphere(transform.TransformPoint(attackOffset), attackRadius);

            foreach (Collider hitCol in hitColliders) {
                if (hitCol.transform.root != transform.root && hitCol.gameObject.tag == PLAYER_TAG && hitCol.transform.name != sourceID) {

                    CmdStun(hitCol.gameObject.name, stunTime, sourceID);

                }
            }
        }
    }*/

    [Command]
    void CmdStun(GameObject player, float _stunTime, string _sourceID) {

        //Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        //networkPlayerStats.RpcTakeDamage(_damage, _sourceID);

        player.GetComponent<PlayerController>().StartStun(5f);
    }



}
