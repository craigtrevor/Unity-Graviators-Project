using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour {

    public GameObject electricBeam;
    Transform playerTransform;

    // Use this for initialization
    void Start() {
        playerTransform = transform.GetChild(3);
    }

    // Update is called once per frame
    void Update() {
        ultInput(); //HEAD STUFF (UP DOWN) ALSO DESTORY PARTICLE
    }

    void ultInput() {
        GameObject playStun = (GameObject)Instantiate(electricBeam, playerTransform.position + playerTransform.forward, Quaternion.Euler(playerTransform.rotation.eulerAngles + new Vector3(0f, -90f, 0f)), playerTransform);
        if (Input.GetKey(KeyCode.F)) {
            playStun.GetComponent<ParticleSystem>().Emit(5);
        } else {
            Debug.Log("rip");
        }

    }
}
