﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Network_PlayerManager))]
public class Network_PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    GameObject[] gameobjectsToDisable;

    [SerializeField]
    string joinedGameString;

    [SerializeField]
    Text joinedGameText;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    Transform playerModel;

    [SerializeField]
    GameObject gravityBlock;

    [HideInInspector]
    public GameObject playerUIInstance;

    Camera sceneCamera;
    public NetworkAnimator netAnim;

    Network_Manager networkManagerScript;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            DisableGameObjects();
            AssignRemoteLayer();
        }

        else
        {
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            GetComponent<Network_PlayerManager>().SetupPlayer();

            string _username = "Loading...";
            if (UI_UserAccountManager.IsLoggedIn)
                _username = UI_UserAccountManager.LoggedIn_Username;
            else
                _username = transform.name;

            CmdSetUsername(transform.name, _username);
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void DisableGameObjects()
    {
        for (int i = 0; i < gameobjectsToDisable.Length; i++)
        {
            gameobjectsToDisable[i].SetActive(false);
        }
    }

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        Network_GameManager.UnRegisterPlayer(transform.name);
    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Network_PlayerManager player = Network_GameManager.GetPlayer(playerID);

        if (player != null)
        {
            Debug.Log(username + " has joined!");
            player.username = username;

            RpcJoinedGame(playerID, username);
        }
    }

    [ClientRpc]
    void RpcJoinedGame(string playerID, string username)
    {
        if (!isLocalPlayer)
        {
            joinedGameText = GameObject.FindWithTag("JoinedGame").GetComponent<Text>() as Text;
            joinedGameString = username + " has joined!";
            joinedGameText.text = joinedGameString;

            StartCoroutine(UpdateJoinGameText(username));
        }
    }

    IEnumerator UpdateJoinGameText(string username)
    {
        yield return new WaitForSeconds(5f);
        joinedGameText.text = "";
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Network_PlayerManager _player = GetComponent<Network_PlayerManager>();

        Network_GameManager.RegisterPlayer(_netID, _player);
    }

    public override void PreStartClient()
    {
        for (int i = 0; i < 12; i++)
        {
            netAnim.SetParameterAutoSend(i, true);
        }
    }

    public override void OnStartLocalPlayer()
    {
        for (int i = 0; i < 12; i++)
        {
            netAnim.SetParameterAutoSend(i, true);
        }
    }
}
