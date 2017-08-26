﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    private Vector3 desiredLocalPosition, initialLocalPosition, displacementVector;
    public GameObject player;
    public GameObject vertArrows;
    float cameraDisplacement;

    public Vector3 raycastPoint;

    // Use this for initialization
    void Start() {
        cameraDisplacement = 0f;
        this.initialLocalPosition = this.transform.localPosition;
        this.desiredLocalPosition = this.initialLocalPosition;
    }

    // Update is called once per frame
    void Update() {
        cameraDisplacement = player.GetComponent<PlayerController>().cameraDisplacement;

        //displacementVector = new Vector3(0f, -cameraDisplacement, 0f);
        displacementVector = Vector3.Lerp(displacementVector, new Vector3(0f, cameraDisplacement * 2f, -cameraDisplacement), Time.deltaTime * 10);

        this.desiredLocalPosition = this.initialLocalPosition - this.displacementVector;

        RaycastHit hit;
        Vector3 desiredPosition = this.transform.parent.position + (this.transform.parent.rotation * this.desiredLocalPosition);
        Vector3 playerPosition = this.transform.parent.position;
        Vector3 dir = (desiredPosition - playerPosition).normalized;
        float magnitude = (desiredPosition - playerPosition).magnitude;

        if (Physics.Raycast(playerPosition, dir * magnitude, out hit, magnitude)) {
            this.transform.position = Vector3.Lerp(hit.point, playerPosition, 0.2f);
        } else {
            this.transform.localPosition = this.desiredLocalPosition;
        }


        TestStuff();
    }

    void TestStuff() {
        RaycastHit hit;
        LayerMask mask = ~(1 << 30);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, mask.value)) {
            //Debug.Log(hit.transform.gameObject.layer);
            LayerMask musk = hit.transform.gameObject.layer;
            //Debug.Log(LayerMask.LayerToName(musk));
            raycastPoint = hit.point;

        }
    }
}
