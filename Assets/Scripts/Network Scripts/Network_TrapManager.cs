using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_TrapManager : NetworkBehaviour {

    // GameObject tags
    private const string PLAYER_TAG = "Player";
    private const string SPIKETRAP_TAG = "SpikeTrap";
    private const string SLOWTRAP_TAG = "SlowTrap";
	private const string SPEEDTRAP_TAG = "SpeedTrap";

    // Scripts
    Network_PlayerManager networkPlayerManager;
    PlayerController playerController;

    //Slow Trap Variables
    [SerializeField]
    private int reducedWalkSpeed = 6;
	[SerializeField]
	private int increasedWalkSpeed = 50;
    [SerializeField]
    private int normalWalkSpeed = 12;

    [SerializeField]
    private int reducedJumpSpeed = 7;
	[SerializeField]
	private int increasedJumpSpeed = 100;
    [SerializeField]
    private int normalJumpSpeed = 15;

    //Spike Trap Variables

    [SerializeField]
    private float trapDamage = 100;

    [Client]
    void OnTriggerEnter(Collider other)
    {
        // Player collides with Spike Trap

        if (this.gameObject.tag == SPIKETRAP_TAG && other.gameObject.tag == PLAYER_TAG)
        {
            networkPlayerManager = other.GetComponent<Network_PlayerManager>();
            Debug.Log(other.gameObject.name);
            Debug.Log(transform.name);
            CmdTakeDamage(other.gameObject.name, trapDamage, transform.name);
        }

        // Player collides with Slow Trap

        if (this.gameObject.tag == SLOWTRAP_TAG && other.tag == PLAYER_TAG)
        {
            playerController = other.GetComponentInChildren<PlayerController>();         
            playerController.moveSettings.forwardVel = reducedWalkSpeed;
            playerController.moveSettings.rightVel = reducedWalkSpeed;
            playerController.moveSettings.jumpVel = reducedJumpSpeed;
        }

		// Player collides with Speed Trap

		if (this.gameObject.tag == SPEEDTRAP_TAG && other.tag == PLAYER_TAG)
		{
			playerController = other.GetComponentInChildren<PlayerController>();         
			playerController.moveSettings.forwardVel = increasedWalkSpeed;
			playerController.moveSettings.rightVel = increasedWalkSpeed;
			playerController.moveSettings.jumpVel = increasedJumpSpeed;
		}
    }

    void OnTriggerExit(Collider other)
    {
        // Player leaves the Slow Trap's collision 

        if (this.gameObject.tag == SLOWTRAP_TAG && other.tag == PLAYER_TAG)
        {
            playerController = other.GetComponentInChildren<PlayerController>();
            playerController.moveSettings.forwardVel = normalWalkSpeed;
            playerController.moveSettings.rightVel = normalWalkSpeed;
            playerController.moveSettings.jumpVel = normalJumpSpeed;
        }

		// Player leaves the Speed Trap's collision

		if (this.gameObject.tag == SPEEDTRAP_TAG && other.tag == PLAYER_TAG)
		{
			Debug.Log("I'm making someone fast as fuck boiiii");
			playerController = other.GetComponentInChildren<PlayerController>();
			playerController.moveSettings.forwardVel = normalWalkSpeed;
			playerController.moveSettings.rightVel = normalWalkSpeed;
			playerController.moveSettings.jumpVel = normalJumpSpeed;
		}
    }

    // Spike Trap sends damage to Player 

    [Command]
    void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
    {
        Debug.Log(_playerID + " has been attacked.");

        //Debug.Log(networkTrap);
        //Network_TrapManager networkTrap = Network_GameManager.GetTrap(_sourceID);
        Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);

        networkPlayerStats.RpcTakeTrapDamage(_damage, transform.name);
    }
}
