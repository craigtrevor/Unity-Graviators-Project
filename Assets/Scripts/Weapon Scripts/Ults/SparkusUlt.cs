using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SparkusUlt : NetworkBehaviour
{

    public Transform spawnTransform;
    public Camera camera;

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
        setTarget();
        ultInput();
    }

    void setTarget()
    {
        Physics.Raycast(spawnTransform.position, spawnTransform.forward, out hit, 15f * Mathf.Infinity, mask.value);

    }

    void ultInput()
    {
        Debug.DrawLine(spawnTransform.position, hit.point);
        if (Input.GetKey(KeyCode.F))
        {
            target = Vector3.Lerp(target, hit.point, Time.deltaTime * 5f);
        }
        else
        {
            target = hit.point;
        }


    }

}
