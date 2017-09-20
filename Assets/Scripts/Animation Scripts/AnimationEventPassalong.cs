using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPassalong : MonoBehaviour {

	public GameObject parentObject;
	public GameObject controllerObject;

	private WeaponSpawn wepSpawn;
	private PlayerController netPlayer;
	private AnimTesterSpawner animSpawnTest;
	private SinglePlayer_WeaponSpawn SP_wepSpawn;
	private Network_CombatManager combatManager;
	private Network_Bot netBot;

	void Start() {
		if (parentObject.GetComponent<WeaponSpawn>() != null) {
			wepSpawn = parentObject.GetComponent<WeaponSpawn> ();
		}

		if (parentObject.GetComponent<PlayerController> () != null) {
			netPlayer = controllerObject.GetComponent<PlayerController> ();
		}

		if (this.GetComponent<AnimTesterSpawner> () != null) {
			animSpawnTest = this.GetComponent<AnimTesterSpawner> ();
		}

		if (parentObject.GetComponent<SinglePlayer_WeaponSpawn> () != null) {
			SP_wepSpawn = parentObject.GetComponent<SinglePlayer_WeaponSpawn> ();
		}

		if (parentObject.GetComponent<Network_CombatManager> () != null) {
			combatManager = parentObject.GetComponent<Network_CombatManager> ();
		}

		if (parentObject.GetComponent<Network_Bot> () != null) {
			netBot = parentObject.GetComponent<Network_Bot> ();
		}

	}

	void RangedAttack (AnimationEvent animEvent) {
		if (wepSpawn != null) {
			wepSpawn.RangedAttack (animEvent);
		} 
		if (animSpawnTest != null) {
			animSpawnTest.RangedAttack (animEvent);
		}
		if (SP_wepSpawn != null) {
			SP_wepSpawn.RangedAttack ();
		}
		if (netBot != null) {
			netBot.RangedAttack ();
		}
	}

	void NoNameShowWeapons (AnimationEvent animEvent) {
		if (wepSpawn != null) {
			wepSpawn.NoNameShowWeapons (animEvent);
		} 
		if (animSpawnTest != null) {
			animSpawnTest.NoNameShowWeapons (animEvent);
		} 
		if (SP_wepSpawn != null) {
			SP_wepSpawn.NoNameShowWeapons ();
		}
		if (netBot != null) {
			netBot.NoNameShowWeapons ();
		}
	}

	void ActualJump (AnimationEvent animEvent) {
		if (netPlayer != null) {
			netPlayer.ActualJump ();
		} 
		if (animSpawnTest != null) {
			animSpawnTest.ActualJump ();
		} 
		if (netBot != null) {
			netBot.ActualJump ();
		}
	}

	void AttackFinished () {
		if (combatManager != null) {
			combatManager.AttackFinished ();
		} 
		if (animSpawnTest != null) {
			
		} 
		if (netBot != null) {
			netBot.AttackFinished ();
		}
	}
}
