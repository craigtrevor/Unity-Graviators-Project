using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

    const string SPAWN_TAG = "SpawnTransform";

    public GameObject cameraToFace;

    // Use this for initialization
    void Start() {

    }

    void Update() {

        if (this.tag == SPAWN_TAG) {
            //transform.rotation = Quaternion.Euler(cameraToFace.transform.eulerAngles.x - 5f, cameraToFace.transform.eulerAngles.y, cameraToFace.transform.eulerAngles.z); //Set rotation of text to grav cam
            Vector3 raycastPoint = cameraToFace.GetComponent<PlayerCamera>().raycastPoint;
            Vector3 relativePos = raycastPoint - transform.position;

            transform.rotation = Quaternion.LookRotation(relativePos);
            //Debug.DrawLine(this.transform.position, raycastPoint);
        } else {
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraToFace.transform.rotation, Time.deltaTime * 10); //Set rotation of text to grav cam
        }

    }
}
