using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoOfCPUs : MonoBehaviour {

	public Network_Manager networkManager;

	void Update () {
		this.GetComponent<Text> ().text = networkManager.noOfCPUs + "";
	}
}
