﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Network_Manager : NetworkManager {

    [SerializeField]
    GameObject characterSelectHUD;

    [SerializeField]
    Text characterTitle; 

    [SerializeField]
    private Button[] characterButtonArray;

    [SerializeField]
    int characterIndex;

    public string characterName;
    public string characterID;

    [SerializeField]
    GameObject ErrorNoName;

    [SerializeField]
    GameObject Sparkus;

    [SerializeField]
    GameObject unitD1;

    [SerializeField]
    GameObject[] ERNNCustomization;
    [SerializeField]
    GameObject[] SPKSCustomization;
    [SerializeField]
    GameObject[] UT_D1Customization;

    int arrayCount;
    int arrayMax;

    void Start()
    {
        characterIndex = 0;
        arrayMax = 3;
        characterName = "Err:NoName";
        characterID = "ERNN";

        characterButtonArray[0].onClick.AddListener(delegate { CharacterSelector(characterButtonArray[0].name); });
        characterButtonArray[1].onClick.AddListener(delegate { CharacterSelector(characterButtonArray[1].name); });
        characterButtonArray[2].onClick.AddListener(delegate { CharacterSelector(characterButtonArray[2].name); });
    }

    void Update()
    {
        SceneChecker();
    }

    void SceneChecker()
    {
        if (Network_SceneManager.instance.sceneName == "Character_Select")
        {
            characterSelectHUD.SetActive(true);
            ChooseCustomization();
        }

        if (Network_SceneManager.instance.sceneName == "Lobby_Scene")
        {
            characterSelectHUD.SetActive(false);
        }
    }

    void CharacterSelector(string buttonName)
    {
        switch (buttonName)
        { 
            case "ErrNoName_btn":
                characterIndex = 0;
                characterName = "Err:NoName";
                characterID = "ERNN";
                ErrorNoName.SetActive(true);
                Sparkus.SetActive(false);
                unitD1.SetActive(false);

                break;

            case "Sparkus_btn":

                characterIndex = 1;
                characterName = "Sparkus";
                characterID = "SPKS";
                ErrorNoName.SetActive(false);
                Sparkus.SetActive(true);
                unitD1.SetActive(false);

                break;

            case "UnitD1_btn":
                characterIndex = 2;
                characterName = "Unit-D1";
                characterID = "UT-D1";
                ErrorNoName.SetActive(false);
                Sparkus.SetActive(false);
                unitD1.SetActive(true);
                break;
        }

        characterTitle.text = characterName;
        playerPrefab = spawnPrefabs[characterIndex];
    }

    void ChooseCustomization()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ERNNCustomization[arrayCount].SetActive(false);
            SPKSCustomization[arrayCount].SetActive(false);
            UT_D1Customization[arrayCount].SetActive(false);

            arrayCount++;

            if (arrayCount >= arrayMax)
            {
                arrayCount = 0;
            }

            UpdateCustomization();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ERNNCustomization[arrayCount].SetActive(false);
            SPKSCustomization[arrayCount].SetActive(false);
            UT_D1Customization[arrayCount].SetActive(false);

            arrayCount--;

            if (arrayCount <= 0)
            {
                arrayCount = 0;
            }

            UpdateCustomization();
        }
    }

    void UpdateCustomization()
    {
        if (characterID == "ERNN")
        {
            ERNNCustomization[arrayCount].SetActive(true);
        }

        else if (characterID == "SPKS")
        {
            SPKSCustomization[arrayCount].SetActive(true);
        }

        else if (characterID == "UT-D1")
        {
            UT_D1Customization[arrayCount].SetActive(true);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        IntegerMessage msg = new IntegerMessage(characterIndex);
        characterSelectHUD.SetActive(false);

        Debug.Log(msg);

        ClientScene.AddPlayer(conn, 0, msg);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        if (extraMessageReader != null)
        {
            IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage>();
            characterIndex = i.value;
        }

        Debug.Log(characterIndex);

        GameObject playerPrefab = spawnPrefabs[characterIndex];

        GameObject player;
        Transform startPos = GetStartPosition();

        if (startPos != null)
        {
            player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
        }
        else
        {
            player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
