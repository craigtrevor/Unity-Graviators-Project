using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunParticleWobbler : MonoBehaviour {

	[SerializeField]
	private GameObject bigStars;

	[SerializeField]
	private GameObject littleStars;

	// Update is called once per frame
	void Update () {
		bigStars.transform.localEulerAngles = new Vector3 ((-Mathf.Sin(Mathf.PingPong(Time.time*Mathf.PI, Mathf.PI)) * 15) + 90, 0, 0);
		littleStars.transform.localEulerAngles = new Vector3 ((Mathf.Sin(Mathf.PingPong(Time.time*Mathf.PI, Mathf.PI)) * 15) + 90, 0, 0);
	}

	public void KillSelfAfter (float timeToDie) {
		StartCoroutine (KillSelf (timeToDie));
	}

	public IEnumerator KillSelf (float timeToDie) {
		yield return new WaitForSeconds (timeToDie);
		Destroy (this.gameObject);
	}

	void OnDisable() {
		Destroy (this.gameObject);
	}
}
