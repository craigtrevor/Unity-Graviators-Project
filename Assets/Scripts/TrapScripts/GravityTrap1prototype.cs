using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrap1prototype : MonoBehaviour {
	//Defining objects and variables
	public GameObject gravityAxis;
	public GameObject gravityBlock;
	public GameObject rotationBlock;
	public GameObject controller;

	GravityAxisScript gravityAxisScript;

	private string gravityDir;

	Quaternion playerRotation;

	private PlayerController playerControllerScript;
	//private PlayerCameraController cameraScript;

	// Use this for initialization
	void Start () {
		playerControllerScript = controller.GetComponent<PlayerController>();
		//cameraScript = camera.GetComponent<PlayerCameraController>();

		gravityDir = "y";

	}

	/*private void OnTriggerEnter(Gravitytrap1 other) {
		gravityAxisScript.GravityUp ();
		Debug.LogError("Hello! I am the gravity trap~!");
		print("Hello! I am the gravity trap~!");
		//rotationBlock.transform.Rotate(0, 0, 180, Space.World);
	}*/

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			print ("I'm in");
		//	gravity = "y";
			//gravityAxisScript.GravityUp ();
			//if (gravity == "y") {
			//	gravityBlock.transform.rotation = Quaternion.Euler (0, 0, 180);
			//	rotationBlock.transform.Rotate(0, 0, 180, Space.World);
			//}

			//Check gravity
			if (gravityDir == "-y" || gravityDir == "y") { //If y gravities
				rotationBlock.transform.Rotate(0, 0, 180, Space.World);
				gravityBlock.transform.rotation = Quaternion.Euler (0, 0, 180);

				controller.transform.rotation = rotationBlock.transform.rotation;
			}// else { //If z or x gravities
				//rotationBlock.transform.Rotate(0, 180, 0, Space.World);
			//	gravityBlock.transform.rotation = Quaternion.Euler (0, 180, 0);
			//} //End if (gravity)

			//Lerp playerRotation towards rotation block and apply it to player
			playerRotation = Quaternion.Lerp(controller.transform.rotation, rotationBlock.transform.rotation, Time.deltaTime * 10);
			//controller.transform.rotation = playerRotation;
			playerControllerScript.targetRotation = playerRotation;

			//Check if full rotated
			if (Mathf.Abs(controller.transform.eulerAngles.x - rotationBlock.transform.eulerAngles.x) <= 1 &&
				Mathf.Abs(controller.transform.eulerAngles.y - rotationBlock.transform.eulerAngles.y) <= 1 &&
				Mathf.Abs(controller.transform.eulerAngles.z - rotationBlock.transform.eulerAngles.z) <= 1) {

				controller.transform.rotation = rotationBlock.transform.rotation;

		}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") {
			print ("I'm out!");

			if (gravityDir == "-y" || gravityDir == "y") {
				rotationBlock.transform.Rotate (0, 0, 180, Space.World);
				gravityBlock.transform.rotation = Quaternion.Euler (0, 0, 180);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
