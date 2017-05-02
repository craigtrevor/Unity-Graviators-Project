using UnityEngine;
using System.Collections;

public class HorArrowsScript : MonoBehaviour {

    GameObject rotationBlock;

    // Use this for initialization
    void Start () {
        
        rotationBlock = GameObject.Find("RotationBlock");
	
	}
	
	// Update is called once per frame
	void Update () {

        //Rotate HorArrows relative to RotationBlock
        transform.rotation = Quaternion.LookRotation(rotationBlock.transform.forward, rotationBlock.transform.up);

    }
}
