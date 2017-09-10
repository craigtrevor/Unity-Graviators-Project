using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompactHud : MonoBehaviour {
	[SerializeField]
	Network_PlayerManager networkPlayerManager;

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

	public WeaponSpawn rangedManager;
	public Image reloadMask;

	public Sprite reload6;
	public Sprite reload5;
	public Sprite reload4;
	public Sprite reload3;
	public Sprite reload2;
	public Sprite reload1;
	public Sprite reload0;

	public void SetPlayer(Network_PlayerManager _networkPlayerManager) {
		networkPlayerManager = _networkPlayerManager;
	}

	void Start() {
		gravCharge = gravityAxis.GetComponent<GravityAxisScript>().gravityCharge;
		gravPips.Add (GravFull5);
		gravPips.Add (GravFull4);
		gravPips.Add (GravFull3);
		gravPips.Add (GravFull2);
		gravPips.Add (GravFull1);
	}

	public void Update() {
		playerHealth = networkPlayerManager.GetHealthPct();
		playerUlt = networkPlayerManager.GetUltimatePct();

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
		gravCharge = gravityAxis.GetComponent<GravityAxisScript>().gravityCharge;
		for (int i = 0; i < 5; i++) {
			if (gravCharge > i) {
				gravPips [i].SetActive (true);	
			} else {
				gravPips [i].SetActive (false);	
			}
		}

		if (rangedManager.reloadTimer == 0) {
			reloadMask.sprite = reload0;
		}

		if (rangedManager.reloadTimer == 1) {
			reloadMask.sprite = reload1;
		}
		if (rangedManager.reloadTimer == 2) {
			reloadMask.sprite = reload2;
		}
		if (rangedManager.reloadTimer == 3) {
			reloadMask.sprite = reload3;
		}
		if (rangedManager.reloadTimer == 4) {
			reloadMask.sprite = reload4;
		}
		if (rangedManager.reloadTimer == 5) {
			reloadMask.sprite = reload5;
		}
		if (rangedManager.reloadTimer == 6) {
			reloadMask.sprite = reload6;
		}
	}
}
