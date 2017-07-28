using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class spiketrap : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private float trapDamage = 100;

    [Client]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG)
        {
            Debug.Log(other.gameObject.name);
            Debug.Log(transform.name);
            CmdTakeDamage(other.gameObject.name, trapDamage, other.gameObject.transform.name); 
        }
    }

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
    {
        Debug.Log(_playerID + " has been attacked.");

        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeDamage(_damage, _playerID);
    }

}
