using UnityEngine;
using UnityEngine.Networking;

public class Network_TakeDamage : NetworkBehaviour {

    private int playerDamage;
    GetHit getHit;

    void Start()
    {
        getHit = GetComponentInChildren<GetHit>();
    }

    [Client]
    public void PlayerAttack()
    {
        Debug.Log("I'm being called!");
        //playerDamage = GetComponent<Network_PlayerStats>().maxHealth = GetComponent<Network_PlayerStats>().maxHealth - GetComponent<DamageStorage>().Damage;
        CmdPlayerAttacked(getHit.playerID, playerDamage);
    }

    [Command]
    void CmdPlayerAttacked(string _playerID, int _damage)
    {
        Debug.Log("CMD");
        Debug.Log(_playerID + " has been hit.");

        Network_PlayerStats _networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
        //_networkPlayerStats.TakeDamage(_damage);
    }

    //[Command]
    //void CmdPlayerAttacked(string _playerID, int _damage)
    //{
    //    Debug.Log("CMD");
    //    Debug.Log(_playerID + " has been hit.");

    //    Network_PlayerStats _networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
    //    _networkPlayerStats.TakeDamage(_damage);
    //}
}
