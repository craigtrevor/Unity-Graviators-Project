using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_CompactHud : MonoBehaviour {

	public float playerHealth;
	public float playerUlt;

	public GameObject healthMask;
	public GameObject damageMask;
	public GameObject ultMask;

	//The following references are for the gravity charge UI system
	public List<GameObject> gravPips = new List<GameObject>();
	public GameObject GravFull1;
	public GameObject GravFull2;
	public GameObject GravFull3;
	public GameObject GravFull4;
	public GameObject GravFull5;



	public GameObject gravityAxis;
	public int gravCharge;

	public SinglePlayer_WeaponSpawn rangedManager;
	public GameObject reloadMask;
	public SinglePlayer_CombatManager combatManager;
	public TutorialManager tutorialManager;
	[SerializeField]
	GameObject scoreboard;

	public bool healTrigger;
	public bool ultTrigger;
	public bool spikeTrigger;
	public bool slowTrigger;

	public bool onHealthPad;
	public bool onUltPad;
	public bool onSpikeTrap;
	public bool onSlowTrap;

	void Start()
	{
		gravCharge = gravityAxis.GetComponent<Sp_GravitySwitch>().gravityCharge;
		gravPips.Add (GravFull5);
		gravPips.Add (GravFull4);
		gravPips.Add (GravFull3);
		gravPips.Add (GravFull2);
		gravPips.Add (GravFull1);
		playerHealth = 1;
		playerUlt = 0;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			tutorialManager.Scoreboardshown = true;
			scoreboard.SetActive(true);
		}

		else if (Input.GetKeyUp(KeyCode.Tab))
		{
			scoreboard.SetActive(false);
		}

		if (onSpikeTrap) {
			tutorialManager.spikeTrapTouched = true;
			if (!spikeTrigger) {
				StartCoroutine (SlowSpike ());
			}
			tutorialManager.tutProgression = 6;
		}

		if (onHealthPad && tutorialManager.spikeTrapTouched == true) 
		{
			tutorialManager.healthPadTouched = true;
			if (!healTrigger) 
			{
				StartCoroutine (SlowHeal ());
			}
			tutorialManager.tutProgression = 7;
		}

		if (onUltPad && tutorialManager.healthPadTouched == true) {
			tutorialManager.ultPadTouched = true;
			if (!ultTrigger) {
				StartCoroutine (SlowUlt ());
			}
			tutorialManager.tutProgression = 8;
		}
		if (onSlowTrap && tutorialManager.ultPadTouched == true) {
			tutorialManager.slowTrapTouched = true;
			if (!slowTrigger) {
				StartCoroutine (SlowSlow ());
			}
			tutorialManager.tutProgression = 9;
		}


		//set health and damage ticker
		healthMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 ((playerHealth * 600),200);
		if (damageMask.GetComponent<RectTransform> ().sizeDelta.x > healthMask.GetComponent<RectTransform> ().sizeDelta.x) {
			damageMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 (damageMask.GetComponent<RectTransform> ().sizeDelta.x - 3, damageMask.GetComponent<RectTransform> ().sizeDelta.y);
		}

		if (damageMask.GetComponent<RectTransform> ().sizeDelta.x < healthMask.GetComponent<RectTransform> ().sizeDelta.x) {
			damageMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 (healthMask.GetComponent<RectTransform> ().sizeDelta.x, damageMask.GetComponent<RectTransform> ().sizeDelta.y);
		}

		//set ultimate charge
		ultMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 (playerUlt * 400, 100);

		// set grav charges
		gravCharge = gravityAxis.GetComponent<Sp_GravitySwitch>().gravityCharge;
		for (int i = 0; i < 5; i++) {
			if (gravCharge > i) {
				gravPips [i].SetActive (true);	
			} else {
				gravPips [i].SetActive (false);	
			}
		}

		if (rangedManager.reloading == true) {
			reloadMask.SetActive(false);
		} else if (rangedManager.reloading == false) {
			reloadMask.SetActive(true);
		}
	}

	private IEnumerator SlowHeal() {
		healTrigger = true;
		playerHealth += 0.05f;
		yield return new WaitForSeconds (0.25f);
		healTrigger = false;
	}

	private IEnumerator SlowUlt() {
		ultTrigger = true;
		playerUlt += 0.05f;
		yield return new WaitForSeconds (0.25f);
		ultTrigger = false;
	}

	private IEnumerator SlowSpike() {
		spikeTrigger = true;
		if (playerHealth > 0.1) {
			playerHealth -= 0.05f;
		}
		yield return new WaitForSeconds (0.01f);
		spikeTrigger = false;
	}

	private IEnumerator SlowSlow() {
		slowTrigger = true;
		combatManager.Slow ();
		yield return new WaitForSeconds (1f);
		slowTrigger = false;
	}
}
