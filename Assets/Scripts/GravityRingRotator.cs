using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRingRotator : MonoBehaviour {

    private Vector3 targetAngle;

    private Vector3 currentAngle;
    private Vector3 startAngle;

    public int direction;

    public GameObject AKey;
    public GameObject DKey;
    public GameObject WKey;
    public GameObject SKey;
    public GameObject JumpKey;

    GameObject ForwardKey;
    GameObject RightKey;
    GameObject BackwardKey;
    GameObject LeftKey;

    public Material normalMat;
    public Material glowMat;

    void Start() {
        startAngle = Vector3.zero;
        currentAngle = startAngle;

        direction = -1;

        ForwardKey = WKey;
        RightKey = DKey;
        BackwardKey = SKey;
        LeftKey = AKey;

    }

    void Update() {

        lightRing(direction);

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, (Time.deltaTime * 10)),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, (Time.deltaTime * 10)),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, (Time.deltaTime * 10)));

        //transform.localEulerAngles = currentAngle;
    }

    public void lightRing(int direction) {

        resetRing();

        if (direction == 0) { //Up
            targetAngle = Vector3.zero;
            JumpKey.GetComponent<Renderer>().material = glowMat;
        }

        if (direction == 1) { //Forward
            targetAngle = new Vector3(20, 0, 0);
            ForwardKey.GetComponent<Renderer>().material = glowMat;
        }

        if (direction == 2) { //Right
            targetAngle = new Vector3(0, 0, -20);
            RightKey.GetComponent<Renderer>().material = glowMat;
        }

        if (direction == 3) { //Backward
            targetAngle = new Vector3(-20, 0, 0);
            BackwardKey.GetComponent<Renderer>().material = glowMat;
        }

        if (direction == 4) { //Left
            targetAngle = new Vector3(0, 0, 20);
            LeftKey.GetComponent<Renderer>().material = glowMat;
        }

    }

    public void resetRing() {
        targetAngle = Vector3.zero;

        JumpKey.GetComponent<Renderer>().material = normalMat;
        WKey.GetComponent<Renderer>().material = normalMat;
        DKey.GetComponent<Renderer>().material = normalMat;
        SKey.GetComponent<Renderer>().material = normalMat;
        AKey.GetComponent<Renderer>().material = normalMat;
    }

    public void SetKeys(int quadrant) {

        if (quadrant == 1) {
            ForwardKey = WKey;
            RightKey = DKey;
            BackwardKey = SKey;
            LeftKey = AKey;
        } else if (quadrant == 2) {
            ForwardKey = DKey;
            RightKey = SKey;
            BackwardKey = AKey;
            LeftKey = WKey;
        } else if (quadrant == 3) {
            ForwardKey = SKey;
            RightKey = AKey;
            BackwardKey = WKey;
            LeftKey = DKey;
        } else if (quadrant == 4) {
            ForwardKey = AKey;
            RightKey = WKey;
            BackwardKey = DKey;
            LeftKey = SKey;
        }


    }
}
