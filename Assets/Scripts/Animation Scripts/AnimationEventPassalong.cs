﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPassalong : MonoBehaviour {

	public GameObject parentObject;
	public GameObject controllerObject;

	private WeaponSpawn wepSpawn;
	private PlayerController netPlayer;
	private AnimTesterSpawner animSpawnTest;
	private SinglePlayer_WeaponSpawn SP_wepSpawn;

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
	}

	void ActualJump (AnimationEvent animEvent) {
		if (netPlayer != null) {
			netPlayer.ActualJump ();
		} 
		if (animSpawnTest != null) {
			animSpawnTest.ActualJump ();
		} 
	}
}
