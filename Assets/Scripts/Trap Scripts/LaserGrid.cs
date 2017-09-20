using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LaserGrid : NetworkBehaviour {

	private string sourceID;

	public Vector3 StartPoint;
	public Vector3 EndPoint;

	public bool towardsEnd;

	public GameObject VertLeft;
	public GameObject VertMid;
	public GameObject VertRight;

	public GameObject HorzTop;
	public GameObject HorzMid;
	public GameObject HorzBottom;

	public float speed = 5;

	public bool damageTick;
	public List<GameObject> affectedList = new List<GameObject> ();
	public List<GameObject> affectedBotList = new List<GameObject> ();

	void SetInitialReferences(string _sourceID)
	{
		sourceID = _sourceID;
	}

	void Start() {
		ChangeLaserLayout ();
	}

	void Update () {
		float step = speed * Time.deltaTime;
		if (towardsEnd) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, EndPoint, step);
		}
		if (!towardsEnd) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, StartPoint, step);
		}
		if (Vector3.Distance (transform.localPosition, EndPoint) < 1) {
			ChangeLaserLayout ();
			towardsEnd = false;
		}
		if (Vector3.Distance (transform.localPosition, StartPoint) < 1) {
			ChangeLaserLayout ();
			towardsEnd = true;
		}

		if (damageTick) {
			StartCoroutine (SlowDamage ());
		}

		for (int i = 0; i < affectedList.Count; i++) {
			if (affectedList [i].GetComponent<Network_PlayerManager> ().isDead) {
				affectedList.Remove (affectedList [i]);
			}
		}
		for (int i = 0; i < affectedBotList.Count; i++) {
			if (affectedBotList [i] == null) {
				affectedBotList.Remove (affectedBotList [i]);
			}
		}
	}

	void ChangeLaserLayout() {
		if (Random.Range (0, 2) == 0) {
			HorzTop.SetActive (false);
			HorzMid.SetActive (false);
			HorzBottom.SetActive (false);

			if (Random.Range (0, 2) == 0) {
				VertMid.SetActive (true);
			} else {
				VertMid.SetActive (false);
			}
			if (Random.Range (0, 2) == 0) {
				VertLeft.SetActive (true);
				VertRight.SetActive (false);
			} else {
				VertLeft.SetActive (false);
				VertRight.SetActive (true);
			}
				
		} else {
			VertLeft.SetActive (false);
			VertMid.SetActive (false);
			VertRight.SetActive (false);

			if (Random.Range (0, 2) == 0) {
				HorzMid.SetActive (true);
			} else {
				HorzMid.SetActive (false);
			}
			if (Random.Range (0, 2) == 0) {
				HorzTop.SetActive (true);
				HorzBottom.SetActive (false);
			} else {
				HorzTop.SetActive (false);
				HorzBottom.SetActive (true);
			}
		}
	}
		
	public bool NameInList(GameObject toCheck, List<GameObject> listToCheck) {
		for (int i = 0; i < listToCheck.Count; i++) {
			if (listToCheck [i] == toCheck) {
				return true;
			}
		} 
		return false;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (!NameInList (collider.gameObject, affectedList)) {
				affectedList.Add (collider.gameObject);
				CmdTakeDamage (collider.gameObject.name, 30, sourceID);
			}
		}
		if (collider.gameObject.tag == "NetBot") {
			if (!NameInList (collider.gameObject, affectedBotList)) {
				affectedBotList.Add (collider.gameObject);
				collider.gameObject.GetComponent<Network_Bot>().TakeDamage(30);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			if (NameInList (collider.gameObject, affectedList)) {
				affectedList.Remove (collider.gameObject);
			}
		}
		if (collider.gameObject.tag == "NetBot") {
			if (NameInList (collider.gameObject, affectedBotList)) {
				affectedBotList.Remove (collider.gameObject);
			}
		}
	}

	IEnumerator SlowDamage() {
		damageTick = false;
		yield return new WaitForSeconds (0.1f);
		damageTick = true;
		for (int i = 0; i < affectedList.Count; i++) {
			CmdTakeDamage (affectedList[i].name, 30, sourceID);
		}
		for (int i = 0; i < affectedBotList.Count; i++) {
			affectedBotList[i].GetComponent<Network_Bot>().TakeDamage(30);
		}
	}

	[Command]
	void CmdTakeDamage(string _playerID, float _damage, string _sourceID)
	{
		Network_PlayerManager networkPlayerStats = Network_GameManager.GetPlayer(_playerID);
		networkPlayerStats.RpcTakeTrapDamage(_damage, _sourceID);
	}
}
