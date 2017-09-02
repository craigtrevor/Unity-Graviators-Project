using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer_ThrownSword : MonoBehaviour {

	public int swordDamage = 30;
	public float rotateSpeed = 500;

	public bool dying;

	private const string PLAYER_TAG = "Player";
	private const string THROWINGSWORD_TAG = "ThrowingSword";

	public GameObject collideParticle;

	// Use this for initialization
	void Start () {
		
	}

	public void SetRight()
	{
		rotateSpeed *= -1;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (dying)
		{
			Die();
		}

		if (this.gameObject.tag == THROWINGSWORD_TAG && !dying)
		{
			transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		print ("hit");
		if (other.transform.root != transform.root && other.gameObject.tag == PLAYER_TAG) {
			if (this.gameObject.tag == THROWINGSWORD_TAG) {// if a throwing sword hit the player
				CmdTakeDamage (other.gameObject, swordDamage);
				transform.SetParent (other.gameObject.transform);
				transform.position = other.contacts [0].point;
				GameObject temp = Instantiate (collideParticle, this.gameObject.transform);
				temp.transform.position = other.contacts [0].point;
				//PlayImpactSound ();
				Die ();
			}
		} else if (other.transform.root != transform.root && other.gameObject.tag != PLAYER_TAG) {
			if (this.gameObject.tag == THROWINGSWORD_TAG) {// if a throwing sword hit the player
				transform.SetParent (other.gameObject.transform);
				transform.position = other.contacts [0].point;
				GameObject temp = Instantiate (collideParticle, this.gameObject.transform);
				temp.transform.position = other.contacts [0].point;
				//PlayImpactSound();
				Die ();
			}
		}
	}
		
	void CmdTakeDamage(GameObject robot, float _damage)
	{
		if (robot.GetComponent<Drone_bot> () != null) {
			robot.GetComponent<Drone_bot> ().RangedHit ();
		}
		if (robot.GetComponent<Bot_Script> () != null) {
			robot.GetComponent<Bot_Script> ().RangedHit ();
		}
	}

	public void Die()
	{
		if (!dying)
		{
			Destroy(GetComponent<Rigidbody>());
			Destroy(GetComponent<BoxCollider>());

			dying = true;
			StartCoroutine(DieNow());
		}
	}

	private IEnumerator DieNow()
	{
		yield return new WaitForSeconds (5f);
		Destroy(gameObject);
	}
}
