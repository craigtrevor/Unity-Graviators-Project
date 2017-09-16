using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_SkillTree : NetworkBehaviour {


	Network_PlayerManager playerManagerScript;
	PlayerController playerControllermodifier;
	Network_CombatManager playerCombatmanager;



	[SerializeField]
	private GameObject SkillUI;
	[SerializeField]
	private GameObject Level1Text;
	[SerializeField]
	private GameObject Level2Text;
	[SerializeField]
	private GameObject Level3Text;
	[SerializeField]
	private GameObject SpeedUpgradeText;
	[SerializeField]
	private GameObject damageUpgradeText;
	[SerializeField]
	private GameObject HpUpgradeText;
	[SerializeField]
	private GameObject UltGainUpgradeText;
	[SerializeField]
	private GameObject UltUpradeText;
	[SerializeField]
	private GameObject UltMaxText;










	public int killStats;
	public string playerCharacterID;




	public int UpgradedJump = 1; // upgrades the speed by increments of 1 unitl it reaches max speed
	public int Upgradedwalk = 1;

	[SerializeField]
	private int skillLevel = 1; // used to detemine what skill level will be chosen

	[SerializeField]
	private bool SkillUIactive = false;
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

		if (Input.GetKey (KeyCode.X))
		{
			SkillUIactive = true;
		}
		if (Input.GetKeyUp (KeyCode.X)) 
		{
			SkillUIactive = false;
		}


		// this section is to see what skill are active
		if (killStats >= 3) 
		{
			Level1Text.SetActive(false);	
			// have player hold down and press button to choose power up
			if(SkillUIactive == true)
			{
				if(Input.GetKeyUp(KeyCode.Q) && skill1B == false && skillLevel == 1)
				{
					skill1A = true;
					SpeedUpgradeText.SetActive (false);
					StartCoroutine(upgradeDelay());
				}
				if(Input.GetKeyUp(KeyCode.E) && skill1A == false && skillLevel == 1)
				{
					skill1B = true;
					damageUpgradeText.SetActive (false);
					StartCoroutine(upgradeDelay());
				}
			}

		}

		if (killStats >= 5) 
		{
			Level2Text.SetActive(false);
			// have player hold down and press button to choose power up
			if(SkillUIactive == true)
			{
				if(Input.GetKeyUp(KeyCode.Q) && skill2B == false && skillLevel == 2)
				{
					skill2A = true;
					HpUpgradeText.SetActive (false);
					StartCoroutine(upgradeDelay());
				}
				if(Input.GetKeyUp(KeyCode.E) && skill2A == false && skillLevel == 2)
				{
					skill2B = true;
					UltGainUpgradeText.SetActive (false);
					StartCoroutine(upgradeDelay());
				}
			}
		}

		if (killStats >= 8) 
		{
			Level3Text.SetActive (false);
			// have player hold down and press button to choose power up
			if(SkillUIactive == true)
			{
				if(Input.GetKeyUp(KeyCode.Q) && skill3B == false && skillLevel == 3)
				{
					UltUpradeText.SetActive (false);
					skill3A = true;
				}
				if(Input.GetKeyUp(KeyCode.E) && skill3A == false && skillLevel == 3)
				{
					UltMaxText.SetActive (false);
					skill3B = true;
				}
			}
		}




		// this section will see what charcter is selected and upgrade the associated skills
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



		}
	} // end of update



	IEnumerator upgradeDelay() // a timer to stop all upgrade being activatesd at once
	{

		yield return new WaitForEndOfFrame ();
		skillLevel += 1;
	}
}
