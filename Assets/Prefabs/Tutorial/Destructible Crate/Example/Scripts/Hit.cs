using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Hit : MonoBehaviour {

	public GameObject DestroyedObject;

	private const string PLAYER_TAG = "Player";
	private const string THROWINGSWORD_TAG = "ThrowingSword";
	private const string UNITD1RANGEWEAPON_TAG = "UnitD1_RangedWeapon";
	private const string SPARKUSRANGEWEAPON_TAG = "Sparkus_RangedWeapon";


	void OnCollisionEnter( Collision hitcol ) {
		if (hitcol.gameObject.tag == PLAYER_TAG) {
			Network_CombatManager playerattacking = hitcol.gameObject.GetComponent<Network_CombatManager> ();
			if (playerattacking.isAttacking == true) {
				//Debug.Log ("collided with player now i blow up");
				DestroyIt ();
			} 
		} else if (hitcol.gameObject.tag == THROWINGSWORD_TAG || hitcol.gameObject.tag == UNITD1RANGEWEAPON_TAG || hitcol.gameObject.tag == SPARKUSRANGEWEAPON_TAG) 
		{
			DestroyIt ();
		} 
	}


	void DestroyIt(){
		if(DestroyedObject) {
			GameObject destoryedCrate = Instantiate(DestroyedObject, this.transform.position, this.transform.rotation) as GameObject;
			//GameObject destoryedCrate = Instantiate(DestroyedObject
			NetworkServer.Spawn (destoryedCrate);
			NetworkServer.Destroy (gameObject);
		}
		Destroy(gameObject);

	}
}