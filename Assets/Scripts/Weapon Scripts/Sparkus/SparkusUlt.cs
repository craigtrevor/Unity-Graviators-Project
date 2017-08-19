using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour {

    public GameObject electricBeam;
    public Transform targetTransform;

    GameObject playStun;

    // Use this for initialization
    void Start() {
        playStun = (GameObject)Instantiate(electricBeam, targetTransform.position + targetTransform.forward, Quaternion.Euler(targetTransform.eulerAngles.x, targetTransform.eulerAngles.y, targetTransform.eulerAngles.z), targetTransform);
    }

    // Update is called once per frame
    void Update() {
        ultInput();
    }

    void ultInput() {


        if (Input.GetKey(KeyCode.F)) {
            playStun.GetComponent<ParticleSystem>().Emit(5); // destory particles please
            ultDamage();
        }
        else {

        }

    }



    void ultDamage() {
        if (Physics.Raycast(targetTransform.position, targetTransform.forward, 17f)) {
            Debug.Log(">:3");
        }
        else {
            Debug.Log("<:Ɛ");
        }
    }
}
