using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_TrapPads : MonoBehaviour {

	public SP_CompactHud compactHud;
	public bool healthPad;
	public bool ultPad;
	public bool spikeTrap;
	public bool slowTrap;

	void OnTriggerEnter (Collider col) {
		if (healthPad && col.tag == "Player") {
			compactHud.onHealthPad = true;
		}
		if (ultPad && col.tag == "Player") {
			compactHud.onUltPad = true;
		}
		if (spikeTrap && col.tag == "Player") {
			compactHud.onSpikeTrap = true;
		}
		if (slowTrap && col.tag == "Player") {
			Debug.Log (" the slow trap pad is active");
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
