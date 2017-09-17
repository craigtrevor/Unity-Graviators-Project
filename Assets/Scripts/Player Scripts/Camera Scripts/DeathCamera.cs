using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamera : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        if (!Physics.Raycast(transform.position, -transform.forward, 1f)) {
            transform.Translate(-transform.forward * Time.deltaTime * 0.5f, Space.World);
        }   
    }
}
