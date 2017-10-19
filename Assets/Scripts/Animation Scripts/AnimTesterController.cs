using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTesterController : MonoBehaviour {

	public GameObject noNameModel;
	public GameObject sparkusModel;
	public GameObject d1Model;

	public GameObject activeModel;

	public GameObject noNameWep;
	public Transform noNameWepSpawn;
	public Transform noNameWepSpawn2;

	public GameObject reloadBall;
	public GameObject sparkusWep;
	public Transform sparkusWepSpawn;
	public GameObject sparkusUlt;
	public Transform sparkusUltSpawn;

	public GameObject wingRing;
	public GameObject d1Wep;
	public Transform d1WepSpawn;
	public GameObject d1Ult;
	public Transform d1UltSpawn;
	public bool d1Ulting;

	public bool preventOtherAnim;

	void Start() {
		activeModel = noNameModel;
		Physics.gravity = new Vector3 (0, -9.81f, 0); 
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
			preventOtherAnim = true;
			activeModel.GetComponent<Animator> ().SetBool ("RangedAttacking", true);

//			if (activeModel == d1Model) {
//				StartCoroutine (D1Ranged ());
//			}
			StartCoroutine (Reload ());
		}
	}

	public void SpawnRanged() {
		if (activeModel == noNameModel) {
			GameObject wep = Instantiate (noNameWep, noNameWepSpawn.position, Quaternion.identity);
			wep = Instantiate (noNameWep, noNameWepSpawn2.position, Quaternion.identity);
			wep.GetComponent<AnimationTestDummyAttacks> ().spinReverse = true;
		}
		if (activeModel == sparkusModel) {
			GameObject wep = Instantiate (sparkusWep, sparkusWepSpawn.position, Quaternion.identity);
			wep.transform.eulerAngles = new Vector3 (-180, 0, 0);
		}
		if (activeModel == d1Model) {
			GameObject wep = Instantiate (d1Wep, d1WepSpawn.position, Quaternion.identity);
		}
	}

	public void StartUltimate() {
		if (!preventOtherAnim) {
			StartCoroutine (Ultimate ());
		}
	}

	public void Flourish() {
		if (!preventOtherAnim) {
			activeModel.GetComponent<Animator> ().SetTrigger ("Flourish");
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
		yield return new WaitForSeconds(1); 
		activeModel.GetComponent<Animator> ().SetBool ("Jump", Toggle (activeModel.GetComponent<Animator> ().GetBool ("Jump")));
		activeModel.GetComponent<Animator> ().SetBool ("InAir", Toggle (activeModel.GetComponent<Animator> ().GetBool ("InAir")));
		preventOtherAnim = false;

	}

	public IEnumerator Ultimate() {
		preventOtherAnim = true;
		activeModel.GetComponent<Animator> ().SetTrigger ("StartUltimate");
		activeModel.GetComponent<Animator> ().SetBool ("UltimateLoop", Toggle (activeModel.GetComponent<Animator> ().GetBool ("UltimateLoop")));
		if (activeModel == noNameModel) {
			noNameModel.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 5, 0);
		}
		if (activeModel == d1Model) {
			d1Ulting = true;
			StartCoroutine (Jump ());
		}
		if (activeModel == sparkusModel) {
			yield return new WaitForSeconds(0.5f); 
			GameObject wep = Instantiate (sparkusUlt, sparkusUltSpawn.position, Quaternion.identity, sparkusUltSpawn);
			wep.transform.eulerAngles = new Vector3 (0, 180, 0);
		}
		if (activeModel == noNameModel) {
			yield return new WaitForSeconds (0.1f); 
			activeModel.GetComponent<Animator> ().SetBool ("UltimateLoop", Toggle (activeModel.GetComponent<Animator> ().GetBool ("UltimateLoop")));
		} else {
			yield return new WaitForSeconds (1f); 
			activeModel.GetComponent<Animator> ().SetBool ("UltimateLoop", Toggle (activeModel.GetComponent<Animator> ().GetBool ("UltimateLoop")));
		}

		if (activeModel == d1Model) {
			yield return new WaitForSeconds(0.6f); 
			GameObject wep = Instantiate (d1Ult, d1UltSpawn.position, Quaternion.identity);
		}
		preventOtherAnim = false;
		d1Ulting = false;
	}

//	public IEnumerator D1Ranged() {
//		yield return new WaitForSeconds(0.4f); 
//		GameObject wep = Instantiate (d1Wep, d1WepSpawn.position, Quaternion.identity);
//	}

	public IEnumerator Reload() {
		if (activeModel == d1Model) {
			StartCoroutine (D1WingOff (0.5f));
		}
		yield return new WaitForSeconds(0.1f); 
		activeModel.GetComponent<Animator> ().SetBool ("RangedAttacking", false);
		yield return new WaitForSeconds(1); 
		if (activeModel == d1Model) {
			StartCoroutine (D1WingOn (0.5f));
		}
		activeModel.GetComponent<Animator> ().SetTrigger ("Ranged Attack Reload");
		if (activeModel == sparkusModel) {
			yield return new WaitForSeconds(0.3f); 
			reloadBall.SetActive (true);
		}
		preventOtherAnim = false;
	}

	IEnumerator D1WingOn(float time) {
		float emissionStrength = 0.1f;
		for (int i = 0; i < 10; i++) {
			emissionStrength += 0.2f;
			//print (emissionStrength);
			wingRing.GetComponent<Renderer> ().material.SetFloat ("_Emission", emissionStrength);
			yield return new WaitForSeconds (time / 10f);
		}
	}

	IEnumerator D1WingOff(float time) {
		float emissionStrength = 2f;
		for (int i = 0; i < 10; i++) {
			emissionStrength -= 0.2f;
			//print (emissionStrength);
			wingRing.GetComponent<Renderer> ().material.SetFloat ("_Emission", emissionStrength);
			yield return new WaitForSeconds (time/10f);
		}
	}
}
