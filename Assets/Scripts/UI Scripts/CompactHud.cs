using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	//GravityAxisScript gravityAxisScript;

	public void SetPlayer(Network_PlayerManager _networkPlayerManager) {
		networkPlayerManager = _networkPlayerManager;
	}

	void Start()
	{

		gravCharge = gravityAxis.GetComponent<GravityAxisScript>().gravityCharge;
		gravPips.Add (GravFull5);
		gravPips.Add (GravFull4);
		gravPips.Add (GravFull3);
		gravPips.Add (GravFull2);
		gravPips.Add (GravFull1);

	}

	public void Update()
	{
		playerHealth = networkPlayerManager.GetHealthPct();
		playerUlt = networkPlayerManager.GetUltimatePct();

		//set health and damage ticker
		healthMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 ((playerHealth * 600),100);
		if (damageMask.GetComponent<RectTransform> ().sizeDelta.x > healthMask.GetComponent<RectTransform> ().sizeDelta.x) {
			damageMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 (damageMask.GetComponent<RectTransform> ().sizeDelta.x - 3, damageMask.GetComponent<RectTransform> ().sizeDelta.y);
		}

		if (damageMask.GetComponent<RectTransform> ().sizeDelta.x < healthMask.GetComponent<RectTransform> ().sizeDelta.x) {
			damageMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 (healthMask.GetComponent<RectTransform> ().sizeDelta.x, damageMask.GetComponent<RectTransform> ().sizeDelta.y);
		}

		//set ultimate charge
		ultMask.GetComponent<RectTransform> ().sizeDelta = new Vector2 (playerUlt * 300, 100);

		// set grav charges
		gravCharge = gravityAxis.GetComponent<GravityAxisScript>().gravityCharge;
		for (int i = 0; i < 5; i++) {
			if (gravCharge > i) {
				gravPips [i].SetActive (true);	
			} else {
				gravPips [i].SetActive (false);	
			}
		}
	}
}
