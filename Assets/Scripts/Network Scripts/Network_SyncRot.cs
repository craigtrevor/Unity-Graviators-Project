using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Network_SyncRot : NetworkBehaviour {

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform camTransform;

    [SyncVar(hook = "OnPlayerRotXSynced")]
    private float syncPlayerRotationX;

    [SyncVar(hook = "OnPlayerRotYSynced")]
    private float syncPlayerRotationY;

    [SyncVar(hook = "OnPlayerRotZSynced")]
    private float syncPlayerRotationZ;

    [SyncVar(hook = "OnCamRotSynced")]
    private float syncCamRotation;

    private float lastPlayerRotX;
    private float lastPlayerRotY;
    private float lastPlayerRotZ;
    private float lastCamRot;
    private float lerpRate = 20;
    private float threshold = 1;
    private float closeEnough = 0.4f;

    [SerializeField]
    private bool useHistoricalInterpolation;

    private List<float> syncPlayerRotXList = new List<float>();
    private List<float> syncPlayerRotYList = new List<float>();
    private List<float> syncPlayerRotZList = new List<float>();
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
        if(syncPlayerRotXList.Count > 0 || syncPlayerRotYList.Count > 0 || syncPlayerRotZList.Count > 0)
        {
            LerpPlayerRot(syncPlayerRotXList[0], syncPlayerRotYList[0], syncPlayerRotZList[0]);

            if (Mathf.Abs(playerTransform.localEulerAngles.x - syncPlayerRotXList[0]) < closeEnough)
            {
                syncPlayerRotXList.RemoveAt(0);
            }

            if (Mathf.Abs(playerTransform.localEulerAngles.y - syncPlayerRotYList[0]) < closeEnough)
            {
                syncPlayerRotYList.RemoveAt(0);
            }

            if (Mathf.Abs(playerTransform.localEulerAngles.z - syncPlayerRotZList[0]) < closeEnough)
            {
                syncPlayerRotZList.RemoveAt(0);
            }

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
        LerpPlayerRot(syncPlayerRotationX, syncPlayerRotationY, syncPlayerRotationZ);
        LerpCamRot(syncCamRotation);
    }

    void LerpPlayerRot(float rotAngleX, float rotAngleY, float rotAngleZ)
    {
        Vector3 playerNewRot = new Vector3(rotAngleX, rotAngleY, rotAngleZ);
        playerTransform.localRotation = Quaternion.Lerp(playerTransform.localRotation, Quaternion.Euler(playerNewRot), lerpRate * Time.deltaTime);
    }

    void LerpCamRot(float rotAngle)
    {
        Vector3 camNewRot = new Vector3(rotAngle, 0, 0);
        camTransform.localRotation = Quaternion.Lerp(camTransform.localRotation, Quaternion.Euler(camNewRot), lerpRate * Time.deltaTime);
    }

   [Command]
   void CmdProvideRotationsToServer(float playerRotX, float playerRotY, float playerRotZ, float camRot)
    {
        syncPlayerRotationX = playerRotX;
        syncPlayerRotationY = playerRotY;
        syncPlayerRotationZ = playerRotZ;
        syncCamRotation = camRot;
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            if (CheckIfBeyondThreshold(playerTransform.localEulerAngles.x, lastPlayerRotX) || CheckIfBeyondThreshold(playerTransform.localEulerAngles.y, lastPlayerRotY) || CheckIfBeyondThreshold(playerTransform.localEulerAngles.z, lastPlayerRotZ) || CheckIfBeyondThreshold(camTransform.localEulerAngles.x , lastCamRot))
            {
                lastPlayerRotX = playerTransform.localEulerAngles.x;
                lastPlayerRotY = playerTransform.localEulerAngles.y;
                lastPlayerRotZ = playerTransform.localEulerAngles.z;
                lastCamRot = camTransform.localEulerAngles.x;
                CmdProvideRotationsToServer(lastPlayerRotX, lastPlayerRotY, lastPlayerRotZ, lastCamRot);
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
    void OnPlayerRotXSynced(float latestPlayerRotX)
    {
        syncPlayerRotationX = latestPlayerRotX;
        syncPlayerRotXList.Add(syncPlayerRotationX);
    }

    [Client]
    void OnPlayerRotYSynced(float latestPlayerRotY)
    {
        syncPlayerRotationY = latestPlayerRotY;
        syncPlayerRotYList.Add(syncPlayerRotationY);
    }

    [Client]
    void OnPlayerRotZSynced(float latestPlayerRotZ)
    {
        syncPlayerRotationZ = latestPlayerRotZ;
        syncPlayerRotZList.Add(syncPlayerRotationZ);
    }

    [Client]
    void OnCamRotSynced(float latestCamRot)
    {
        syncCamRotation = latestCamRot;
        syncCamRotList.Add(syncCamRotation);
    }
}
