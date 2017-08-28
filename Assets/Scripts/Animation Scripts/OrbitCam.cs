using UnityEngine;
using System.Collections;

public class OrbitCam : MonoBehaviour {

	// Move Camera Script
	public GameObject target;//the target object
	private float speedMod = 10.0f;//a speed modifier
	private Vector3 point;//the coord to the point where the camera looks at
	public Vector3 dragOrigin;
	public float dragSpeed = 0.5f;

	void Start() {
		point = target.transform.position;//get target's coords
		transform.LookAt (point);//makes the camera look to it
		dragOrigin = Vector3.zero;
	}
	
	void Update () {//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
		if (Input.GetMouseButton(1)) {
			transform.RotateAround (point,new Vector3(0.0f,Input.GetAxis ("Mouse X"),0.0f),20 * Time.deltaTime * speedMod);
		}

		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Input.GetAxis("Mouse ScrollWheel"));
		}

		if (Input.GetMouseButtonDown (0)) {
			dragOrigin = Input.mousePosition;
			return;
		}

		if (!Input.GetMouseButton (0)) return;

		Vector3 pos = Camera.main.ScreenToViewportPoint (Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3 (-pos.x * dragSpeed, 0, -pos.y * dragSpeed);

		transform.Translate (move, Space.World);
	}

	void LookAtMe(string lookat) {
		target = GameObject.Find (lookat);
		point = target.transform.position;
		transform.LookAt (point);
	}
}
