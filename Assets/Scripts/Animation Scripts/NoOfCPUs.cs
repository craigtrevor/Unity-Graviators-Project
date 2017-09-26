using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoOfCPUs : MonoBehaviour {

	public Network_Manager networkManager;
	public Text noOfCPUsText;
	public Button increaseButton;
	public Button decreaseButton;

	void Start () {
		networkManager = GameObject.FindGameObjectWithTag ("NetManager").GetComponent<Network_Manager>();
		increaseButton.onClick.AddListener (Increase);
		decreaseButton.onClick.AddListener (Decrease);
	}

	void Update () {
		noOfCPUsText.text = networkManager.noOfCPUs + "";
	}

	public void Increase() {
		networkManager.IncreaseCPUs ();
	}

	public void Decrease() {
		networkManager.DecreaseCPUs ();
	}
}
