using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTesterController : MonoBehaviour {

	public GameObject noNameModel;
	public GameObject sparkusModel;
	public GameObject d1Model;

	public GameObject activeModel;

	public bool preventOtherAnim;

	void Start() {
		activeModel = noNameModel;
	}
		
	public void NoNameActive() {
			noNameModel.SetActive(true);
			sparkusModel.SetActive(false);
			d1Model.SetActive(false);
			activeModel = noNameModel;
	}

	public void SparkusActive() {
			noNameModel.SetActive(false);
			sparkusModel.SetActive(true);
			d1Model.SetActive(false);
			activeModel = sparkusModel;
	}

	public void D1Active() {
		noNameModel.SetActive (false);
		sparkusModel.SetActive (false);
		d1Model.SetActive (true);
		activeModel = d1Model;
	}

	public void Run() {
		if (!preventOtherAnim) {
			activeModel.GetComponent<Animator> ().SetBool ("Moving", Toggle (activeModel.GetComponent<Animator> ().GetBool ("Moving")));
		}
	}

	public void StartJump() {
		if (!preventOtherAnim) {
			StartCoroutine (Jump ());
		}
	}

	public void Attack() {
		if (!preventOtherAnim) {
			activeModel.GetComponent<Animator> ().SetTrigger ("Attack");
		}
	}

	public void RangedAttack() {
		if (!preventOtherAnim) {
			activeModel.GetComponent<Animator> ().SetTrigger ("Ranged Attack");
		}
	}

	public void StartUltimate() {
		if (!preventOtherAnim) {
			StartCoroutine (Ultimate ());
		}
	}

	private bool Toggle(bool toSwitch) {
		if (toSwitch) {
			toSwitch = false;
			return toSwitch;
		} else {
			toSwitch = true;
			return toSwitch;
		}
	}

	public IEnumerator Jump() {
		preventOtherAnim = true;
		activeModel.GetComponent<Animator> ().SetBool ("Jump", Toggle (activeModel.GetComponent<Animator> ().GetBool ("Jump")));
		activeModel.GetComponent<Animator> ().SetBool ("InAir", Toggle (activeModel.GetComponent<Animator> ().GetBool ("InAir")));
		activeModel.GetComponent<Rigidbody> ().velocity = new Vector3(0,5,0);
		yield return new WaitForSeconds(1); 
		activeModel.GetComponent<Animator> ().SetBool ("Jump", Toggle (activeModel.GetComponent<Animator> ().GetBool ("Jump")));
		activeModel.GetComponent<Animator> ().SetBool ("InAir", Toggle (activeModel.GetComponent<Animator> ().GetBool ("InAir")));
		preventOtherAnim = false;

	}

	public IEnumerator Ultimate() {
		preventOtherAnim = true;
		activeModel.GetComponent<Animator> ().SetTrigger ("StartUltimate");
		activeModel.GetComponent<Animator> ().SetBool ("UltimateLoop", Toggle (activeModel.GetComponent<Animator> ().GetBool ("UltimateLoop")));
		yield return new WaitForSeconds(1); 
		activeModel.GetComponent<Animator> ().SetBool ("UltimateLoop", Toggle (activeModel.GetComponent<Animator> ().GetBool ("UltimateLoop")));
		preventOtherAnim = false;
	}
}
