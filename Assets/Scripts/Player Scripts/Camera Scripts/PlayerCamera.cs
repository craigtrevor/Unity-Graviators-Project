using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    private Vector3 desiredLocalPosition, initialLocalPosition, displacementVector;
    public GameObject player;
    public GameObject vertArrows;
    public float cameraDisplacement, cameraZoom;

    public Vector3 raycastPoint;
    public LayerMask mask;

    // Use this for initialization
    void Start() {
        cameraDisplacement = 0f;
        cameraZoom = 2f;
        this.initialLocalPosition = this.transform.localPosition;
        this.desiredLocalPosition = this.initialLocalPosition;
    }

    // Update is called once per frame
    void Update() {

		if (Network_SceneManager.instance.sceneName == "Online_Scene_ArenaV2")
		{
			cameraDisplacement = player.GetComponent<PlayerController>().cameraDisplacement;
		}

		else if (Network_SceneManager.instance.sceneName == "Tutorial_Arena")
		{
			cameraDisplacement = player.GetComponent<Sp_Controller>().cameraDisplacement;
		}
        

        //displacementVector = new Vector3(0f, -cameraDisplacement, 0f);
        displacementVector = Vector3.Lerp(displacementVector, new Vector3(0f, -cameraDisplacement * 1f - 0.5f*cameraZoom+0.25f, 0f), Time.deltaTime * 15f);

        this.desiredLocalPosition = (this.initialLocalPosition - this.displacementVector)*cameraZoom;

        RaycastHit hit;
        Vector3 desiredPosition = this.transform.parent.position + (this.transform.parent.rotation * this.desiredLocalPosition);
        Vector3 playerPosition = this.transform.parent.position;
        Vector3 dir = (desiredPosition - playerPosition).normalized;
        float magnitude = (desiredPosition - playerPosition).magnitude;

        if (Physics.Raycast(playerPosition, dir * magnitude, out hit, magnitude) && cameraZoom <= 2.5f) {
            this.transform.position = Vector3.Lerp(hit.point, playerPosition, 0.2f);
        } else {
            this.transform.localPosition = this.desiredLocalPosition;
        }

        RaycastStuff();
    }

    void RaycastStuff() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, mask.value)) {
            //Debug.Log(hit.transform.gameObject.layer);
            LayerMask musk = hit.transform.gameObject.layer;
            //Debug.Log(LayerMask.LayerToName(musk));
            raycastPoint = hit.point;
            //Debug.DrawLine(player.transform.position, hit.point);

        }
    }
}
