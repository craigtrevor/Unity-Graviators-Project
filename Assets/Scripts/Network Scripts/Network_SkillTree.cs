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



	public int UpgradedJump = 1; 
	public int Upgradedwalk = 1;

	public bool skill1A = false; 
	public bool skill1B = false;
	public bool skill2A = false; 
	public bool skill2B = false;
	public bool skill3A = false; 
	public bool skill3B = false;


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
			if(skill1A == true) // increase movment speed
			{
				if( 10 < playerControllermodifier.moveSettings.forwardVel && playerControllermodifier.moveSettings.forwardVel < 19|| 10 < playerControllermodifier.moveSettings.rightVel && playerControllermodifier.moveSettings.rightVel < 19|| 10 < playerControllermodifier.moveSettings.jumpVel && playerControllermodifier.moveSettings.jumpVel < 19) // if they are not at increased speed and are not slowed
				{
				playerControllermodifier.moveSettings.forwardVel = playerControllermodifier.moveSettings.forwardVel +Upgradedwalk; // upgrades movement speed to increase his base but still alows him to be slowed
				playerControllermodifier.moveSettings.rightVel = playerControllermodifier.moveSettings.rightVel +Upgradedwalk;
				playerControllermodifier.moveSettings.jumpVel = playerControllermodifier.moveSettings.jumpVel +UpgradedJump;
				}
			}
			if (skill1B == true) //increase damage
			{
				
				playerCombatmanager.playerDamage = playerCombatmanager.playerDamage + 10;
			}

			if(skill2A == true) // increase health
			{
				if (playerManagerScript.maxHealth < 149) // so max health wont be above 150
				{
					playerManagerScript.maxHealth = playerManagerScript.maxHealth + 50;
				}
			}
			if (skill2B == true)  // increase ult gain
			{
				playerCombatmanager.ultGain = playerCombatmanager.ultGain + 10;
			}

			if(skill3A == true) // increase number of ult charges
			{
				//waiting on ult script changes
			}
			if (skill3B == true) // reduce ult charge needed
			{
				//waiting on ult script changes
			}


			if (killStats >= 3) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha1) && skill1B == false)
					{
						skill1A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha2) && skill1A == false)
					{
						skill1B = true;
					}
				}

			}

			if (killStats >= 5) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha3) && skill2B == false)
					{
						skill2A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha4) && skill2A == false)
					{
						skill2B = true;
					}
				}
			}

			if (killStats >= 8) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha5) && skill3B == false)
					{
						skill3A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha6) && skill3A == false)
					{
						skill3B = true;
					}
				}
			}
		}

		if (playerCharacterID == "SPKS")
		{
			if(skill1A == true) // increase movment speed
			{
				if( 10 < playerControllermodifier.moveSettings.forwardVel && playerControllermodifier.moveSettings.forwardVel < 19|| 10 < playerControllermodifier.moveSettings.rightVel && playerControllermodifier.moveSettings.rightVel < 19|| 10 < playerControllermodifier.moveSettings.jumpVel && playerControllermodifier.moveSettings.jumpVel < 19) // if they are not at increased speed and are not slowed
				{
					playerControllermodifier.moveSettings.forwardVel = playerControllermodifier.moveSettings.forwardVel +Upgradedwalk; // upgrades movement speed to increase his base but still alows him to be slowed
					playerControllermodifier.moveSettings.rightVel = playerControllermodifier.moveSettings.rightVel +Upgradedwalk;
					playerControllermodifier.moveSettings.jumpVel = playerControllermodifier.moveSettings.jumpVel +UpgradedJump;
				}
			}
			if (skill1B == true) //increase damage
			{

				playerCombatmanager.playerDamage = playerCombatmanager.playerDamage + 10;
			}

			if(skill2A == true) // increase health
			{
				if (playerManagerScript.maxHealth < 149) // so max health wont be above 150
				{
					playerManagerScript.maxHealth = playerManagerScript.maxHealth + 50;
				}
			}
			if (skill2B == true)  // increase ult gain
			{
				playerCombatmanager.ultGain = playerCombatmanager.ultGain + 10;
			}

			if(skill3A == true) // increase number of ult charges
			{
				//waiting on ult script changes
			}
			if (skill3B == true) // reduce ult charge needed
			{
				//waiting on ult script changes
			}


			if (killStats >= 3) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha1) && skill1B == false)
					{
						skill1A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha2) && skill1A == false)
					{
						skill1B = true;
					}
				}

			}

			if (killStats >= 5) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha3) && skill2B == false)
					{
						skill2A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha4) && skill2A == false)
					{
						skill2B = true;
					}
				}
			}

			if (killStats >= 8) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha5) && skill3B == false)
					{
						skill3A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha6) && skill3A == false)
					{
						skill3B = true;
					}
				}
			}
		}

		if (playerCharacterID == "UT-D1")
		{
			if(skill1A == true) // increase movment speed
			{
				if( 10 < playerControllermodifier.moveSettings.forwardVel && playerControllermodifier.moveSettings.forwardVel < 19|| 10 < playerControllermodifier.moveSettings.rightVel && playerControllermodifier.moveSettings.rightVel < 19|| 10 < playerControllermodifier.moveSettings.jumpVel && playerControllermodifier.moveSettings.jumpVel < 19) // if they are not at increased speed and are not slowed
				{
					playerControllermodifier.moveSettings.forwardVel = playerControllermodifier.moveSettings.forwardVel +Upgradedwalk; // upgrades movement speed to increase his base but still alows him to be slowed
					playerControllermodifier.moveSettings.rightVel = playerControllermodifier.moveSettings.rightVel +Upgradedwalk;
					playerControllermodifier.moveSettings.jumpVel = playerControllermodifier.moveSettings.jumpVel +UpgradedJump;
				}
			}
			if (skill1B == true) //increase damage
			{

				playerCombatmanager.playerDamage = playerCombatmanager.playerDamage + 10;
			}

			if(skill2A == true) // increase health
			{
				if (playerManagerScript.maxHealth < 149) // so max health wont be above 150
				{
					playerManagerScript.maxHealth = playerManagerScript.maxHealth + 50;
				}
			}
			if (skill2B == true)  // increase ult gain
			{
				playerCombatmanager.ultGain = playerCombatmanager.ultGain + 10;
			}

			if(skill3A == true) // increase number of ult charges
			{
				//waiting on ult script changes
			}
			if (skill3B == true) // reduce ult charge needed
			{
				//waiting on ult script changes
			}


			if (killStats >= 3) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha1) && skill1B == false)
					{
						skill1A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha2) && skill1A == false)
					{
						skill1B = true;
					}
				}

			}

			if (killStats >= 5) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha3) && skill2B == false)
					{
						skill2A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha4) && skill2A == false)
					{
						skill2B = true;
					}
				}
			}

			if (killStats >= 8) 
			{
				// have player hold down and press button to choose power up
				if(Input.GetKey(KeyCode.X))
				{
					if(Input.GetKeyDown(KeyCode.Alpha5) && skill3B == false)
					{
						skill3A = true;
					}
					if(Input.GetKeyDown(KeyCode.Alpha6) && skill3A == false)
					{
						skill3B = true;
					}
				}
			}
		}
	} // end of update









}
