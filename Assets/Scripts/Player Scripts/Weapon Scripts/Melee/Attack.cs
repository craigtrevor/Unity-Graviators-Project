using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public GameObject meleehitboxObject;
	public Transform MeleeSpawn;
	public bool Attaking = false;

	public float AttackWait = 0.0f;
	public float MeleeTime = 1.0f;
	private bool CanAttack = true;

	private GameObject melee;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1") && CanAttack == true) 
		{
            Attaking = true;
            startAttack();
            //CmdstartAttack();
		}

        melee.transform.position = MeleeSpawn.transform.position;
    }


    //public void CmdSpawn()
    //{
    //    GameObject go = (GameObject)Instantiate(otherPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    //    NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    //}

    void startAttack()
    {
        CanAttack = false;
        Attaking = false;
        melee = Instantiate(meleehitboxObject, MeleeSpawn.position, MeleeSpawn.rotation, this.gameObject.transform);
        Destroy(melee, MeleeTime);

        StartCoroutine(attackDelay());
    }

 //   [Command]
 //   void CmdstartAttack()
	//{
	//	melee = Instantiate(meleehitboxObject, MeleeSpawn.position, MeleeSpawn.rotation, this.gameObject.transform);
 //       NetworkServer.SpawnWithClientAuthority(meleehitboxObject, connectionToClient);

 //       CanAttack = false;
	//	Attaking = false;
	//	Destroy (melee, MeleeTime);

	//	StartCoroutine (attackDelay());
	//}

	IEnumerator attackDelay()
	{
		yield return new WaitForSeconds (AttackWait);
		CanAttack = true;
	}
}
