﻿using System.Collections;
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

    public int gravityCharge;
    
    // Use this for initialization
    void Start()
    {
        gravityCharge = GameObject.Find("3PSController").GetComponent<GravityAxisScript>().gravityCharge;
    }
	// Update is called once per frame
	void Update () {
        UIGravUpdate();
	}
    public void UIGravUpdate()
    {
      /*  GravFull.gameObject.SetActive(true);
        GravFull1.gameObject.SetActive(true);
        GravFull2.gameObject.SetActive(true);
        GravFull3.gameObject.SetActive(true);
        GravFull4.gameObject.SetActive(true);*/

        if (gravityCharge == 2000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(false);
            GravFull2.gameObject.SetActive(false);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(true);
            GravEmpty2.gameObject.SetActive(true);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);

        }
        else if (gravityCharge <= 2000)
        {
            GravFull.gameObject.SetActive(false);
            GravFull1.gameObject.SetActive(false);
            GravFull2.gameObject.SetActive(false);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(true);
            GravEmpty1.gameObject.SetActive(true);
            GravEmpty2.gameObject.SetActive(true);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);
        }
        if (gravityCharge == 4000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(false);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(true);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);
        }
        else if (gravityCharge >= 2000 && gravityCharge <= 4000)
        {
            GravFull.gameObject.SetActive(false);
            GravFull1.gameObject.SetActive(false);
            GravFull2.gameObject.SetActive(false);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(true);
            GravEmpty1.gameObject.SetActive(true);
            GravEmpty2.gameObject.SetActive(true);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);
        }
        if (gravityCharge == 6000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(true);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(false);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);
        }
        else if (gravityCharge >= 4000 && gravityCharge <= 6000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(false);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(true);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);
        }
        if (gravityCharge == 8000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(true);
            GravFull3.gameObject.SetActive(true);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(false);
            GravEmpty3.gameObject.SetActive(false);
            GravEmpty4.gameObject.SetActive(true);
        }
        else if (gravityCharge >= 6000 && gravityCharge <= 8000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(true);
            GravFull3.gameObject.SetActive(false);
            GravFull4.gameObject.SetActive(false);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(false);
            GravEmpty3.gameObject.SetActive(true);
            GravEmpty4.gameObject.SetActive(true);
        }
        if (gravityCharge == 10000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(true);
            GravFull3.gameObject.SetActive(true);
            GravFull4.gameObject.SetActive(true);
        
            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(false);
            GravEmpty3.gameObject.SetActive(false);
            GravEmpty4.gameObject.SetActive(false);
        }
        else if (gravityCharge >= 8000 && gravityCharge <= 10000)
        {
            GravFull.gameObject.SetActive(true);
            GravFull1.gameObject.SetActive(true);
            GravFull2.gameObject.SetActive(true);
            GravFull3.gameObject.SetActive(true);
            GravFull4.gameObject.SetActive(true);

            GravEmpty.gameObject.SetActive(false);
            GravEmpty1.gameObject.SetActive(false);
            GravEmpty2.gameObject.SetActive(false);
            GravEmpty3.gameObject.SetActive(false);
            GravEmpty4.gameObject.SetActive(false);
        }
    }
}
