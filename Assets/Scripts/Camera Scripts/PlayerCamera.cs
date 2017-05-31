using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 desiredLocalPosition;

    // Use this for initialization
    void Start()
    {
        this.desiredLocalPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 desiredPosition = this.transform.parent.position + (this.transform.parent.rotation * this.desiredLocalPosition);
        Vector3 playerPosition = this.transform.parent.position;
        Vector3 dir = (desiredPosition - playerPosition).normalized;
        float magnitude = (desiredPosition - playerPosition).magnitude;

        if (Physics.Raycast(playerPosition, dir * magnitude, out hit, magnitude))
        {
            this.transform.position = Vector3.Lerp(hit.point, playerPosition, 0.2f);
        }
        else
        {
            this.transform.localPosition = this.desiredLocalPosition;
        }
    }
}
