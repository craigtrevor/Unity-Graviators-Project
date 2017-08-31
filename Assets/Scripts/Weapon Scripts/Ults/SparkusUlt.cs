using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour
{



    public Transform spawnTransform;
	public GameObject camera;

	public float laserLength;

	RaycastHit hit;
	LayerMask mask = ~(1 << 30);

    Vector3 target;

    // Use this for initialization
    void Start()
    {
        if (Physics.Raycast(spawnTransform.position, spawnTransform.forward, out hit, 15f * Mathf.Infinity, mask.value))
        {
            target = hit.point;

        }
    }

    // Update is called once per frame
    void Update()
    {
        //ultInput();
    }

	void FixedUpdate() {
		SetAim ();
	}

    void SetAim()
	{

		Color colour;
		Color greenButDarker = new Color(0f, 0.3f, 0f);
		Color redButDarker = new Color (0.3f, 0f, 0f);

		if (Physics.Raycast (spawnTransform.position, spawnTransform.forward, out hit, Mathf.Infinity, mask.value)) {


			if (Input.GetKey (KeyCode.F)) {			
				target = Vector3.Lerp (target, hit.point, Time.deltaTime * 4f);

				if (hit.distance < laserLength) {
					colour = Color.red;
				} else {
					colour = redButDarker;
				}
			} else {
				target = hit.point;

				if (hit.distance < laserLength) {
					colour = Color.green;
				} else {
					colour = greenButDarker;
				}
			}	

			Debug.DrawLine (spawnTransform.position, target, colour);
			DrawDebugAxis (target);
		}
	}

	void DrawDebugAxis(Vector3 centre) {
		Debug.DrawLine (centre, centre+Vector3.forward, Color.blue);
		Debug.DrawLine (centre, centre+Vector3.right, Color.red);
		Debug.DrawLine (centre, centre+Vector3.up, Color.green);
	}
}
