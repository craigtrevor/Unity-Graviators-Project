using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Network_SyncRot : NetworkBehaviour {

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform camTransform;

    [SyncVar(hook = "OnPlayerRotSynced")]
    private Quaternion syncPlayerRotation;

    [SyncVar(hook = "OnCamRotSynced")]
    private float syncCamRotation;

    private Quaternion lastPlayerRot;
    private float lastCamRot;
    private float lerpRate = 20;
    private float threshold = 1;
    private float closeEnough = 0.4f;

    [SerializeField]
    private bool useHistoricalInterpolation;

    private List<Quaternion> syncPlayerRotList = new List<Quaternion>();
    private List<float> syncCamRotList = new List<float>();


    void Update()
    {
        LerpRotations();
    }

    void FixedUpdate()
    {
        TransmitRotations();
    }

    void LerpRotations()
    {
        if (!isLocalPlayer)
        {
            if (useHistoricalInterpolation)
            {
                HistoricalInterpolation();
            }

            else
            {
                OrdinaryLerping();
            }
        }
    }
   
    void HistoricalInterpolation()
    {
        if(syncPlayerRotList.Count > 0)
        {
            LerpPlayerRot(syncPlayerRotList[0]);

            //if ((playerTransform.rotation == syncPlayerRotList[0]) < closeEnough)
            //{

            //}

            //Debug.Log(syncPlayerRotList.Count.ToString() + " syncPlayerRotList Count");
        }

        if (syncCamRotList.Count > 0)
        {
            LerpCamRot(syncCamRotList[0]);

            if (Mathf.Abs(camTransform.localEulerAngles.x - syncCamRotList[0]) < closeEnough)
            {
                syncCamRotList.RemoveAt(0);
            }

            //Debug.Log(syncCamRotList.Count.ToString() + " syncCamRotList Count");
        }
    }

    void OrdinaryLerping()
    {
        LerpPlayerRot(syncPlayerRotation);
        LerpCamRot(syncCamRotation);
    }

    void LerpPlayerRot(Quaternion rotAngle)
    {
        playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, rotAngle, Time.deltaTime * lerpRate);
    }

    void LerpCamRot(float rotAngle)
    {
        Vector3 camNewRot = new Vector3(rotAngle, 0, 0);
        camTransform.localRotation = Quaternion.Lerp(camTransform.localRotation, Quaternion.Euler(camNewRot), lerpRate * Time.deltaTime);
    }

   [Command]
   void CmdProvideRotationsToServer(Quaternion playerRot, float camRot)
    {
        syncPlayerRotation = playerRot;
        syncCamRotation = camRot;
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold || CheckIfBeyondThreshold(camTransform.localEulerAngles.x , lastCamRot))
            {
                lastPlayerRot = playerTransform.rotation;
                lastCamRot = camTransform.localEulerAngles.x;
                CmdProvideRotationsToServer(lastPlayerRot, lastCamRot);
            }
        }
    }

    bool CheckIfBeyondThreshold (float rot1, float rot2)
    {
        if(Mathf.Abs(rot1 - rot2) > threshold)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    [Client]
    void OnPlayerRotSynced(Quaternion latestPlayerRot)
    {
        syncPlayerRotation = lastPlayerRot;
        syncPlayerRotList.Add(syncPlayerRotation);
    }

    [Client]
    void OnCamRotSynced(float latestCamRot)
    {
        syncCamRotation = latestCamRot;
        syncCamRotList.Add(syncCamRotation);
    }
}
