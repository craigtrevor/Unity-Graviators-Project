using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_TrapPads : MonoBehaviour {


	public float countDownTimer = 0f;
	public bool countingDown;
	public bool spikesOut;
	public GameObject spikes;
	public GameObject baseObj;
	public bool blinking;

	public SP_CompactHud compactHud;
	public bool healthPad;
	public bool ultPad;
	public bool spikeTrap;
	public bool slowTrap;

	//void OnTriggerEnter (Collider col) {
	void OnTriggerStay (Collider col) {
		if (healthPad && col.tag == "Player") {
			compactHud.onHealthPad = true;
		}
		if (ultPad && col.tag == "Player") {
			compactHud.onUltPad = true;
		}
		if (spikeTrap && col.tag == "Player") {
			compactHud.onSpikeTrap = true;
			if (!countingDown) {
				StartCoroutine (CountDown ());
				countingDown = true;
			}
		}
		if (slowTrap && col.tag == "Player") {
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

	IEnumerator CountDown() {
		float tickDown = countDownTimer / 5f;
		float blinkAmount = 1f;

		for (int x = 0; x < 5f; x++) {
			for (int y = 0; y < blinkAmount; y++ ) {
				yield return StartCoroutine (Blink (tickDown/blinkAmount));
			}
			blinkAmount += 1f;
		}

		spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, 0.65f, spikes.transform.localPosition.z);

		spikesOut = true;
		baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor", Color.red * 2);
		yield return new WaitForSeconds (2f);
		spikesOut = false;
		baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor", Color.red * 0);
		while (spikes.transform.localPosition.y > -0.1f) {
			spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, spikes.transform.localPosition.y - 0.01f, spikes.transform.localPosition.z);
			yield return new WaitForSeconds (0.05f);
		}
		spikes.transform.localPosition = new Vector3 (spikes.transform.localPosition.x, -0.1f, spikes.transform.localPosition.z);
		countingDown = false;
	}

	IEnumerator Blink(float blinkTime) {
		blinking = true;
		float tickTime = (blinkTime / 2f) / 5f;
		float emissionStrength = 0.1f;
		for (int i = 0; i < 5; i++) {
			emissionStrength += 0.2f;
			baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor", Color.red * emissionStrength);
			yield return new WaitForSeconds (tickTime);
		}
		for (int i = 0; i < 5; i++) {
			emissionStrength -= 0.2f;
			baseObj.GetComponent<Renderer> ().materials [1].SetColor ("_EmissionColor",  Color.red * emissionStrength);
			yield return new WaitForSeconds (tickTime);
		}
		blinking = false;
	}
}
