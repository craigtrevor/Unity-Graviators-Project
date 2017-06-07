﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamScript : MonoBehaviour {

    int sensitivty = 1;
    Space space = Space.Self;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            sensitivty++;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            sensitivty = Mathf.Max(1, sensitivty - 1);
        }

        if (Input.GetKey(",")) {
            space = Space.Self;
        }
        if (Input.GetKey(".")) {
            space = Space.World;
        }


        if (Input.GetKey("w")) {
            transform.Translate(Vector3.forward * Time.deltaTime * sensitivty, space);
        }
        if (Input.GetKey("a")) {
            transform.Translate(Vector3.left * Time.deltaTime * sensitivty, space);
        }
        if (Input.GetKey("s")) {
            transform.Translate(Vector3.back * Time.deltaTime * sensitivty, space);
        }
        if (Input.GetKey("d")) {
            transform.Translate(Vector3.right * Time.deltaTime * sensitivty, space);
        }

        if (Input.GetKey("e")) {
            transform.Rotate(0f, 0.1f * sensitivty, 0f, Space.World);
        }
        if (Input.GetKey("q")) {
            transform.Rotate(0f, 0.1f * -sensitivty, 0f, Space.World);
        }

        if (Input.GetKey("r")) {
            transform.Translate(Vector3.up * Time.deltaTime * sensitivty, space);
        }
        if (Input.GetKey("f")) {
            transform.Translate(Vector3.down * Time.deltaTime * sensitivty, space);
        }

        if (Input.GetKey("t")) {
            transform.Rotate(0.1f * -sensitivty, 0f, 0f, Space.Self);
        }
        if (Input.GetKey("g")) {
            transform.Rotate(0.1f * sensitivty, 0f, 0f, Space.Self);
        }


        if (Input.GetKey("z")) {
            transform.Rotate(0f, 0f, 0.1f * -sensitivty, Space.Self);
        }
        if (Input.GetKey("c")) {
            transform.Rotate(0f, 0f, 0.1f * sensitivty, Space.Self);
        }
        if (Input.GetKey("x")) {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        }

        if (Input.GetKey("p")) {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}