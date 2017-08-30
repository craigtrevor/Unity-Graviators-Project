using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Script : MonoBehaviour {

	public float health = 100;

	private bool dead = false;

	public Component[] Renders;
	public Behaviour[] DisableOnDeath;

	public GameObject corpse; // the player exploding on thier death, assigned in editor


	void Start () {
		Renders = GetComponentsInChildren<SkinnedMeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (health == 0 && dead == false) 
		{
			
			die ();

			// after 10 seconds un hide the bot and give full health
		}
	}

	public void die()
	{
		dead = true;
		for (int i = 0; i < Renders.Length; i++) 
		{
			Renders [i].GetComponent<SkinnedMeshRenderer> ().enabled = false;
		}
		for (int i = 0; i < DisableOnDeath.Length; i++) 
		{
			DisableOnDeath[i].enabled = false;
		}
		Instantiate(corpse, this.transform.position, this.transform.rotation);
		StartCoroutine (RespawnTimer());

	}

	public void respawn()
	{
		health = 100;
		dead = false;
		for (int i = 0; i < Renders.Length; i++) 
		{
			Renders [i].GetComponent<SkinnedMeshRenderer> ().enabled = true;
		}
		for (int i = 0; i < DisableOnDeath.Length; i++) 
		{
			DisableOnDeath [i].enabled = true;;
		}
	}

	IEnumerator RespawnTimer()
	{
		yield return new WaitForSeconds (5f);
		respawn();
	}
}