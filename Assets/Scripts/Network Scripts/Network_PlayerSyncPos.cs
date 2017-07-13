using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Network_PlayerSyncPos : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;
    private Vector3 lastPos;

    [SerializeField]
    Transform myTransform;

    [SerializeField]
    float lerpRate = 15;
    private float threshold = 0.5f;

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer (Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }
}
