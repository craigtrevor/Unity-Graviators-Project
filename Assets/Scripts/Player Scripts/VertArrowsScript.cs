using UnityEngine;
using System.Collections;

public class VertArrowsScript : MonoBehaviour {

    public float yRot = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
        //transform.rotation = player.transform.rotation; //Set vertArrows to player rotation
        yRot = transform.localRotation.eulerAngles.y; //Set yRot to local rotation on y-axis


    }
}
