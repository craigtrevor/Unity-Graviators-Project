using UnityEngine;
using System.Collections;

public class VertArrowsScript : MonoBehaviour {

    private float yRot = 0;

    public GameObject cameraPivot;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        yRot = transform.localRotation.eulerAngles.y; //Set yRot to local rotation on y-axis

    }

    private void FixedUpdate() {
        //transform.rotation = cameraPivot.transform.rotation; //Set vertArrows to cameraPivot rotation
        //transform.localRotation = Quaternion.Euler(0f, cameraPivot.transform.localRotation.y, 0f);
    }

    public float GetYRot() {
        return yRot;
    }
}
