using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetHit : NetworkBehaviour {

	[SerializeField]
	private Rigidbody RB;

    //private const string PLAYER_TAG = "Player";
    public string playerID;

	public bool gothit = false;
	public bool cangethit= true;

	public Transform cameraRotation;
	public int thrust = 1; //change this for speed
	public int distance = 50 ; // change this for distance travalled
	private float waittime = 0.01f;// the delay between movements 

    public int playerDamage;

    // Use this for initialization
    void Start ()
    {
		RB = GetComponentInChildren<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (cangethit == true) // can the player get hit
		{
			if (gothit == true) // was the player hit
			{
				StartCoroutine (changeback ());
				cangethit = false;
			}
		}

		//onDamage (1);
	}

    [Client]
	void OnTriggerEnter (Collider col)
	{
        if (gothit == false)
		{
			if (col.tag == "MeleeWeapon")
			{
                Debug.Log("I'm hitting it");

				if(col.transform.root != transform.root)
				{
                    Debug.Log("TRANSFORMERS");

                    Debug.Log("i am hurt");
                    //transform.GetComponentInChildren<Renderer> ().material.color = Color.red;

                    playerID = col.transform.parent.parent.parent.gameObject.name;
                    //playerDamage = GetComponent<Network_PlayerManager>().maxHealth = GetComponent<Network_PlayerManager>().maxHealth - GetComponentInChildren<DamageStorage>().Damage;

                    CmdPlayerAttacked(playerID, playerDamage);

                    // networkTakeDamage.PlayerAttack();

                    //playerDamage = GetComponent<Network_PlayerManager>().maxHealth = GetComponent<Network_PlayerManager>().maxHealth - col.GetComponent<DamageStorage>().Damage;
                    col.GetComponentInParent<Dash>().chargePercent += col.GetComponent<DamageStorage>().Damage;

                    gothit = true;

                    //if (col.GetComponentInChildren<DamageStorage>().Damage == GetComponentInChildren<DamageStorage>().Damage && col.GetComponent<Attack>().Attaking == true)
                    //{
                    //    Debug.Log("Knockback!");
                    //    knockBack();
                    //}
                    //else
                    //{
                    //    Debug.Log("i am hurt");
                    //    //transform.GetComponentInChildren<Renderer> ().material.color = Color.red;

                    //    playerID = col.transform.parent.parent.parent.gameObject.name;
                    //    playerDamage = GetComponent<Network_PlayerManager>().maxHealth = GetComponent<Network_PlayerManager>().maxHealth - GetComponentInChildren<DamageStorage>().Damage;

                    //    CmdPlayerAttacked(playerID, playerDamage);

                    //    // networkTakeDamage.PlayerAttack();

                    //    //playerDamage = GetComponent<Network_PlayerManager>().maxHealth = GetComponent<Network_PlayerManager>().maxHealth - col.GetComponent<DamageStorage>().Damage;
                    //    col.GetComponentInParent<Dash>().chargePercent += col.GetComponent<DamageStorage>().Damage;

                    //    gothit = true;
                    //}
                }
                }
                if (col.tag == "ThrowingSword")
                {
                    Debug.Log("i am hurt");
                    //transform.GetComponentInChildren<Renderer> ().material.color = Color.red;

                    //networkTakeDamage.PlayerAttack();

                    //playerDamage = GetComponent<Network_PlayerManager>().maxHealth = GetComponent<Network_PlayerManager>().maxHealth - col.GetComponentInChildren<DamageStorage>().Damage;

                    playerID = col.name;

                    CmdPlayerAttacked(playerID, playerDamage);

                    //GetComponent<Network_PlayerManager> ().maxHealth = GetComponent<Network_PlayerManager> ().maxHealth - col.GetComponent<damageRange> ().Damage;
                    gothit = true;
                }

                if (col.tag == "dashHitBox")
                {
                    //networkTakeDamage.PlayerAttack();
                    //playerDamage = GetComponent<Network_PlayerManager>().maxHealth = GetComponent<Network_PlayerManager>().maxHealth - col.GetComponentInChildren<DamageStorage>().Damage;
                    playerID = col.name;

                    CmdPlayerAttacked(playerID, playerDamage);

                    //GetComponent<Network_PlayerManager> ().maxHealth = GetComponent<Network_PlayerManager> ().maxHealth - col.GetComponent<DashDamage> ().damage;
            }					
        }
	}

    [Command]
    void CmdPlayerAttacked(string _playerID, int _damage)
    {
        Debug.Log("CMD");
        Debug.Log(_playerID + " has been hit.");

        Network_PlayerManager _networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
       // _networkPlayerStats.TakeDamage(_damage);
    }

    IEnumerator changeback(){
		yield return new WaitForSeconds (0.5f);
		Debug.Log ("I am ok now");
		//transform.GetComponent<Renderer> ().material.color = Color.white;
		gothit = false;
		cangethit = true;
	}

	void knockBack()
	{
		StartCoroutine (tinydelay ());

		Debug.Log ("i have been knocked back");
	}

	IEnumerator tinydelay()
	{	
		GetComponentInChildren<PlayerController> ().enabled = false;
		RB.AddForce(cameraRotation.forward*-thrust);
		yield return new WaitForSeconds (waittime);
		GetComponentInChildren<PlayerController> ().enabled = true;
	}

}
