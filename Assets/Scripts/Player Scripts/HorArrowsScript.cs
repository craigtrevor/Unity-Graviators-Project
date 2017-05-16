using UnityEngine;
using System.Collections;

public class HorArrowsScript : MonoBehaviour {

    public GameObject rotationBlock;

    // Use this for initialization
    private void Start () {
	
	}
	
	// Update is called once per frame
	private void Update () {

        //Rotate HorArrows relative to RotationBlock
        transform.rotation = Quaternion.LookRotation(rotationBlock.transform.forward, rotationBlock.transform.up);

    }
}
