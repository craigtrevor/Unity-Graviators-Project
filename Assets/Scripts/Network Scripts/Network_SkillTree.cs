using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_SkillTree : NetworkBehaviour {


	Network_PlayerManager playerManagerScript;
	PlayerController playerControllermodifier;
	Network_CombatManager playerCombatmanager;



	public int killStats;
	public string playerCharacterID;



	public int UpgradedJump = 5; 
	public int Upgradedwalk = 5;


	// Use this for initialization
	void Start () {
		playerManagerScript = this.gameObject.GetComponent<Network_PlayerManager> ();
		playerCharacterID = playerManagerScript.playerCharacterID;
		playerControllermodifier = this.gameObject.GetComponentInChildren<PlayerController> ();
		playerCombatmanager = this.gameObject.GetComponent<Network_CombatManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		killStats = playerManagerScript.killStats;

		if (playerCharacterID == "ERNN") 
		{
			if (killStats >= 3) 
			{
				playerControllermodifier.moveSettings.forwardVel = playerControllermodifier.moveSettings.forwardVel +Upgradedwalk; // upgrades movement speed to increase his base but still alows him to be slowed
				playerControllermodifier.moveSettings.rightVel = playerControllermodifier.moveSettings.forwardVel +Upgradedwalk;
				playerControllermodifier.moveSettings.jumpVel = playerControllermodifier.moveSettings.forwardVel +UpgradedJump;
			}

			if (killStats >= 5) 
			{
				playerCombatmanager.playerDamage = playerCombatmanager.playerDamage + 10;
			}

			if (killStats >= 8) 
			{
				// increase ult gain
				//waiting on ult script changes
			}
		}

		if (playerCharacterID == "SPKS")
		{
			if (killStats >= 3) 
			{
				// increase the range of the sparkus range
				//waiting on sparkus range
			}

			if (killStats >= 5) 
			{
				playerCombatmanager.playerDamage = playerCombatmanager.playerDamage + 10;
			}

			if (killStats >= 8) 
			{
				//increase ult duration
				//waiting on ult script changes
			}
		}

		if (playerCharacterID == "UT-D1")
		{
			if (killStats >= 3) 
			{
				// gravity stacks
	
			}

			if (killStats >= 5) 
			{
				// fall speed
			}

			if (killStats >= 8) 
			{
				//increase charge
				//waiting on ult script changes
			}
		}
	}
}
