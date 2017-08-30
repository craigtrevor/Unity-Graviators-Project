using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMat : MonoBehaviour {

    GameObject[] keys;
    Vector3 initScale;
    int gravityCharge;

    public Material chargedKey, unchargedKey;

    // Use this for initialization
    void Start() {
        keys = GameObject.FindGameObjectsWithTag("key");
        initScale = keys[0].transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        gravityCharge = GetComponent<GravityAxisScript>().gravityCharge;

        foreach (GameObject key in keys) {
            if (key.transform.IsChildOf(this.transform)) {
                ChangeMat(key.GetComponent<Renderer>());
                key.transform.localScale = ChangeScale(key.transform.localScale);
            }            
        }
    }

    void ChangeMat(Renderer thisRenderer) {        

        if (gravityCharge == 0) {
            //mat uncharged
            thisRenderer.material = unchargedKey;
        } else {
            //mat charged
            thisRenderer.material = chargedKey;
        }
    }

    Vector3 ChangeScale(Vector3 scale) {

        if (gravityCharge == 0) {
            //mat uncharged
            return Vector3.zero;
        } else {
            //mat charged
            return Vector3.Lerp(scale, initScale, Time.deltaTime * 10f);
        }
    }
}
