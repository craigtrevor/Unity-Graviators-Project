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
    
	public int gravCharge;

    public void Update()
	{
		//set the variable here
		//gravCharge = 

		if (gravCharge < 2000) {
			GravFull.gameObject.SetActive (false);
			GravEmpty.gameObject.SetActive (true);
		} else {
			GravFull.gameObject.SetActive (true);
			GravEmpty.gameObject.SetActive (false);
		}

		if (gravCharge < 4000) {
			GravFull1.gameObject.SetActive (false);
			GravEmpty1.gameObject.SetActive (true);
		} else {
			GravFull1.gameObject.SetActive (true);
			GravEmpty1.gameObject.SetActive (false);
		}

		if (gravCharge < 6000) {
			GravFull2.gameObject.SetActive (false);
			GravEmpty2.gameObject.SetActive (true);
		} else {
			GravFull2.gameObject.SetActive (true);
			GravEmpty2.gameObject.SetActive (false);
		}

		if (gravCharge < 8000) {
			GravFull3.gameObject.SetActive (false);
			GravEmpty3.gameObject.SetActive (true);
		} else {
			GravFull3.gameObject.SetActive (true);
			GravEmpty3.gameObject.SetActive (false);
		}

		if (gravCharge == 10000) {
			GravFull4.gameObject.SetActive (true);
			GravEmpty4.gameObject.SetActive (false);
		} else {
			GravFull4.gameObject.SetActive (false);
			GravEmpty4.gameObject.SetActive (true);
		}
	}
}