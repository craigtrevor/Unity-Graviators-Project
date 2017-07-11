using UnityEngine;
using System.Collections;

public class VertArrowsScript : MonoBehaviour {

    private float yRot = 0;

    [SerializeField]
    GameObject cameraPivot;

    // Use this for initialization
    void Start() {

        cameraPivot = GameObject.FindGameObjectWithTag("MainCamera");

    }

    // Update is called once per frame
    void Update() {

        yRot = transform.localRotation.eulerAngles.y; //Set yRot to local rotation on y-axis
        
    }

    private void FixedUpdate() {
        transform.rotation = cameraPivot.transform.rotation; //Set vertArrows to cameraPivot rotation
    }

    public float GetYRot() {
        return yRot;
    }
}
