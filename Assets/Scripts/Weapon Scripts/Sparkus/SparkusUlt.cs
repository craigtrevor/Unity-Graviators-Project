using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour {

    private string sourceID;

    const float LASER_DAMAGE = 0.1f;

    public GameObject look;

    public GameObject laserParticle;
    GameObject playLaser;

    private const string PLAYER_TAG = "Player";

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.F)) {

            playLaser = (GameObject)Instantiate(laserParticle, this.transform.position, Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y - 90f, this.transform.eulerAngles.z), this.transform) as GameObject;
            playLaser.GetComponent<ParticleSystem>().Emit(1);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.forward, 30f)) {
                if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
                    if (hit.transform.root != transform.root && hit.transform.gameObject.tag == PLAYER_TAG/* && hit.transform.name != sourceID*/) {
                        Debug.Log("Hit Player!");

                        CmdLaserDamage(hit.transform.gameObject.name, LASER_DAMAGE, sourceID);
                    }
                }
            } else {
                //print("No boop");
            }
        } else {
            Destroy(playLaser);
        }
    }

    [Command]
    void CmdLaserDamage(string _playerID, float _damage, string _sourceID) {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _sourceID);
    }

    void hurt() {

    }

}

