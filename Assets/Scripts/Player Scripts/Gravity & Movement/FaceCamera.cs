using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

    const string SPAWN_TAG = "SpawnTransform";

    public GameObject cameraToFace;

    public bool lerpFace;

    Vector3 raycastPoint;
    Vector3 relativePos;

    // Use this for initialization
    void Start() {
        lerpFace = false;
        relativePos = raycastPoint - transform.position;
    }

    void FixedUpdate() {

        if (this.tag == SPAWN_TAG) {
            //transform.rotation = Quaternion.Euler(cameraToFace.transform.eulerAngles.x - 5f, cameraToFace.transform.eulerAngles.y, cameraToFace.transform.eulerAngles.z); //Set rotation of text to grav cam

            raycastPoint = cameraToFace.GetComponent<PlayerCamera>().raycastPoint;

            if (lerpFace) {
                relativePos = Vector3.Lerp(relativePos, raycastPoint - transform.position, Time.deltaTime * 5f);
                Debug.Log("am lerping");
            } else {
                relativePos = raycastPoint - transform.position;
            }

            transform.rotation = Quaternion.LookRotation(relativePos);
            //Debug.DrawLine(this.transform.position, raycastPoint);
        } else {
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraToFace.transform.rotation, Time.deltaTime * 10); //Set rotation of text to grav cam
        }

    }
}
