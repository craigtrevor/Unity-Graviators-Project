using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_Bot_CollisionChecker : MonoBehaviour {

	public Network_Bot netBot;
	public string direction;

	void OnTriggerStay (Collider collider) {
		if (collider.gameObject.tag != "Player" || collider.gameObject.tag != "NetBot") {
			SetBool (true);
		}
	}

	void OnTriggerExit () {
		SetBool(false);
	}

	void SetBool(bool setBool) {
		if (direction == "forward") {
			netBot.frontColliding = setBool;
		}
		if (direction == "backward") {
			netBot.backColliding = setBool;
		}
		if (direction == "left") {
			netBot.leftColliding = setBool;
		}
		if (direction == "right") {
			netBot.rightColliding = setBool;
		}
		if (direction == "jump") {
			netBot.jumpColliding = setBool;
		}
		if (direction == "ground") {
			if (setBool == true) {
				netBot.isFalling = false;
			}
			if (setBool == false) {
				netBot.isFalling = true;
			}
		}
	}
}
