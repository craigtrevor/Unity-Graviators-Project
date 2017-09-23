using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Out_of_Bounds : MonoBehaviour {
	[SerializeField]
	public GameObject spawnpostion;
	[SerializeField]
	private Vector3 SpawnVector;
	// Use this for initialization
	void Start () {
		SpawnVector = spawnpostion.transform.position;

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider Col)
	{
		//Debug.Log (Col.tag);
		//if (Col.gameObject.tag == "Player" || Col.gameObject.tag == "PlayerController") {
		//if (Col.gameObject.tag == "PlayerController") {
		if (Col.gameObject.tag == "Player") {
			Col.gameObject.transform.position = spawnpostion.transform.position;
		} else {
			return;
		}
	}
}
