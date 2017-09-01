using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_bot : MonoBehaviour {

	public GameObject explosion1;
	public GameObject explosion2;

	public GameObject tutorialManager;
	public GameObject player;

	//adjust this to change speed
	public float speed = 2.5f;
	//adjust this to change how high it goes
	public float height = 0.5f;

	public void RangedHit () {
		StartCoroutine (DieNow ());
	}

	private IEnumerator DieNow()
	{
		Instantiate (explosion1, transform.position, Quaternion.identity);
		yield return new WaitForSeconds (1f);
		Instantiate (explosion2, transform.position, Quaternion.identity);
		tutorialManager.GetComponent<TutorialManager> ().botsMurdered += 1;
		Destroy(gameObject);
	}
		
	void Update() {
		transform.LookAt (player.transform);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
		//calculate what the new Y position will be
		float newY = Mathf.Sin(Time.time * speed);
		//set the object's Y to the new calculated Y
		transform.position = new Vector3(transform.position.x, transform.position.y + (newY * 0.01f), transform.position.z);
	}
}
