using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GravityCharge : MonoBehaviour {
    //The following references are for the gravity charge UI system
    public RawImage GravFull;
    public RawImage GravFull1;
    public RawImage GravFull2;
    public RawImage GravFull3;
    public RawImage GravFull4;

    public RawImage GravEmpty;
    public RawImage GravEmpty1;
    public RawImage GravEmpty2;
    public RawImage GravEmpty3;
    public RawImage GravEmpty4;

    public GameObject gravityAxis;
	public int gravCharge;
    //GravityAxisScript gravityAxisScript;

    void Start()
    {
        GravityAxisScript gravityAxisScript = gravityAxis.GetComponent<GravityAxisScript>();

        //canvasUI = GameObject.FindGameObjectWithTag("UI");
        //GravityAxisScript gravityAxisScript = canvasUI.GetComponent<GravityAxisScript>();
        gravCharge = gravityAxisScript.gravityCharge;
    }

    public void Update()
	{
        //set the variable here
        GravityAxisScript gravityAxisScript = gravityAxis.GetComponent<GravityAxisScript>();
        gravCharge = gravityAxisScript.gravityCharge;

        if (gravCharge < 1) {
			GravFull.gameObject.SetActive (false);
			GravEmpty.gameObject.SetActive (true);
		} else {
			GravFull.gameObject.SetActive (true);
			GravEmpty.gameObject.SetActive (false);
		}

		if (gravCharge < 2) {
			GravFull1.gameObject.SetActive (false);
			GravEmpty1.gameObject.SetActive (true);
		} else {
			GravFull1.gameObject.SetActive (true);
			GravEmpty1.gameObject.SetActive (false);
		}

		if (gravCharge < 3) {
			GravFull2.gameObject.SetActive (false);
			GravEmpty2.gameObject.SetActive (true);
		} else {
			GravFull2.gameObject.SetActive (true);
			GravEmpty2.gameObject.SetActive (false);
		}

		if (gravCharge < 4) {
			GravFull3.gameObject.SetActive (false);
			GravEmpty3.gameObject.SetActive (true);
		} else {
			GravFull3.gameObject.SetActive (true);
			GravEmpty3.gameObject.SetActive (false);
		}

		if (gravCharge == 5) {
			GravFull4.gameObject.SetActive (true);
			GravEmpty4.gameObject.SetActive (false);
		} else {
			GravFull4.gameObject.SetActive (false);
			GravEmpty4.gameObject.SetActive (true);
		}
	}
}