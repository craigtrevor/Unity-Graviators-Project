﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_TrapPads : MonoBehaviour {

	public SP_CompactHud compactHud;
	public bool healthPad;
	public bool ultPad;
	public bool spikeTrap;
	public bool slowTrap;

	void OnTriggerEnter () {
		if (healthPad) {
			compactHud.onHealthPad = true;
		}
		if (ultPad) {
			compactHud.onUltPad = true;
		}
		if (spikeTrap) {
			compactHud.onSpikeTrap = true;
		}
		if (slowTrap) {
			compactHud.onSlowTrap = true;
		}
	}

	void OnTriggerExit () {
		if (healthPad) {
			compactHud.onHealthPad = false;
		}
		if (ultPad) {
			compactHud.onUltPad = false;
		}
		if (spikeTrap) {
			compactHud.onSpikeTrap = false;
		}
		if (slowTrap) {
			compactHud.onSlowTrap = false;
		}
	}
}