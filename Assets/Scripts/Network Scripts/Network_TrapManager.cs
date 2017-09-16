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

	// Player Animator & Model
	public Animator anim;
	public Transform playerModelTransform;

	[SerializeField]
	private float animSpeed = 0f;

    //Slow Trap Variables
    [SerializeField]
    private int reducedWalkSpeed = 6;
	[SerializeField]
	private int increasedWalkSpeed = 18;
    [SerializeField]
    private int normalWalkSpeed = 12;

    [SerializeField]
    private int reducedJumpSpeed = 7;
	[SerializeField]
	private int increasedJumpSpeed = 20;
    [SerializeField]
    private int normalJumpSpeed = 15;

    //Spike Trap Variables

    [SerializeField]
    private float trapDamage = 100;

	private ParticleSystem playSlowParticle;
	public ParticleSystem slowParticle;

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


			//anim = other.GetComponent<Animator>();
			//playerAnim.Animator.speed = 0.6f;
			//playerAnimator.speed = 0.2f;
			//playerAnim["Moving"].speed = animSpeed;
			//playerAnim["Moving"].speed = animSpeed;
			//anim["Moving"].speed = 0.2f;
			//anim["Walking"].speed = 0.2f;
			//anim["Moving"].speed = 0.2f;
			//other.GetComponent<Animator>().speed = 0.1f;

			//anim.speed = animSpeed;

			//Debug.Log("My animation should be slowed down...");

        }

		// Player collides with Speed Trap

		if (this.gameObject.tag == SPEEDTRAP_TAG && other.tag == PLAYER_TAG)
		{
			Debug.Log("I'm making someone fast as fuck boiiii");
			playerController = other.GetComponentInChildren<PlayerController>();         
			playerController.moveSettings.forwardVel = increasedWalkSpeed;
			playerController.moveSettings.rightVel = increasedWalkSpeed;
			playerController.moveSettings.jumpVel = increasedJumpSpeed;
		}
    }

  private void OnTriggerStay(Collider other)
   {
       if (this.gameObject.tag == SLOWTRAP_TAG && other.gameObject.tag == PLAYER_TAG){
			ParticleSystem playSlowParticle = (ParticleSystem)Instantiate (slowParticle, playerController.transform.position, playerController.transform.rotation);
       }
   }

	//[Client]
	//void OnTriggerStay(Collider other)
	//{
	//	anim.speed = animSpeed;
	//}

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
