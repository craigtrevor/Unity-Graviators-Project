using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Network_Manager : NetworkManager {

    private GameObject buttonUI;
    Text characterTitle; 
    public string characterName;

    [SerializeField]
    private Button[] characterButtonArray;

    [SerializeField]
    GameObject ErrorNoName;

    [SerializeField]
    GameObject Sparkus;

    [SerializeField]
    GameObject unitD1;

    int characterIndex = 0;

    void Start()
    {
        characterName = "Err:NoName";
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (Network_SceneManager.instance.sceneName == "Lobby_Scene")
        {
            Array.Clear(characterButtonArray, 0, characterButtonArray.Length);
        }
    }

    void LateUpdate()
    {
        if (Network_SceneManager.instance.sceneName == "Character_Select")
        {
            IntializeUI();
        }
    }

    void IntializeUI()
    {
        buttonUI = GameObject.FindGameObjectWithTag("UI");
        characterButtonArray = buttonUI.GetComponentsInChildren<Button>();

        characterTitle = GameObject.Find("Character_Text").GetComponent<Text>();

        AddListeners();
    }

    void AddListeners()
    {
        characterButtonArray[0].onClick.AddListener(delegate { CharacterSelector(characterButtonArray[0].name); });
        characterButtonArray[1].onClick.AddListener(delegate { CharacterSelector(characterButtonArray[1].name); });
        characterButtonArray[2].onClick.AddListener(delegate { CharacterSelector(characterButtonArray[2].name); });
    }

    void CharacterSelector(string buttonName)
    {
        switch (buttonName)
        { 
            case "ErrNoName_btn":
                characterIndex = 0;
                characterName = "Err:NoName";

                ErrorNoName.SetActive(true);
                Sparkus.SetActive(false);
                unitD1.SetActive(false);

                break;

            case "Sparkus_btn":

                characterIndex = 1;
                characterName = "Sparkus";

                ErrorNoName.SetActive(false);
                Sparkus.SetActive(true);
                unitD1.SetActive(false);

                break;

            case "UnitD1_btn":
                characterIndex = 2;
                characterName = "Unit-D1";

                ErrorNoName.SetActive(false);
                Sparkus.SetActive(false);
                unitD1.SetActive(true);
                break;
        }

        characterTitle.text = characterName;
        playerPrefab = spawnPrefabs[characterIndex];
    }

    //public override void OnClientConnect(NetworkConnection conn)
    //{

    //    IntegerMessage msg = new IntegerMessage(characterIndex);

    //    if (!clientLoadedScene)
    //    {
    //        // Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
    //        ClientScene.Ready(conn);
    //        if (autoCreatePlayer)
    //        {
    //            ClientScene.AddPlayer(conn, 0, msg);
    //        }
    //    }
    //}

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    //{
    //    int id = 0;

    //    if (extraMessageReader != null)
    //    {
    //        IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage>();
    //    }

    //    GameObject playerPrefab = spawnPrefabs[id];

    //    GameObject player;
    //    Transform startPos = GetStartPosition();

    //    if (startPos != null)
    //    {
    //        player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
    //    }
    //    else
    //    {
    //        player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    //    }

    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
}
