using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Network_Manager : NetworkManager {

    [Header("Character Select Settings")]

    [SerializeField]
    GameObject characterSelectRoom;

    [SerializeField]
    GameObject characterSelectHUD;

    [SerializeField]
    Text characterTitle;

    [SerializeField]
    int characterIndex;

    public string characterName;
    public string characterID;

    [SerializeField]
    GameObject[] characterCustomization;

    public string customzationName;

	int arrayCount;
    int arrayMax;
    int arrayMin;

    bool joinedLobby = false;

    AudioSource introAudioSource;
    bool isPlaying;

    [Header("CPU Settings")]

    [SerializeField]
	public int noOfCPUs = 0;

	[SerializeField]
	public int maxCPUs = 4;

    void Start()
    {
        characterIndex = 0;
        arrayCount = 0;
        arrayMin = 0;
        arrayMax = 5;

        characterName = "Err:NoName";
        characterID = "ERNN";
        customzationName = "empty hat";

        introAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);

        if (scene.name == "Character_Select_V2")
        {
            ChooseCustomizationViaKeyboard();
            ActivateCharacterSelectRoom();
        }

        else if (scene.name != "Character_Select_V2")
        {
            characterSelectHUD.SetActive(false);
            characterSelectRoom.SetActive(false);
        }

        if (scene.name == "Main_Menu")
        {
            noOfCPUs = 0;
        }

        if (scene.name == "JoinOfflineMode_Scene")
        {
            noOfCPUs = 1;
            joinedLobby = true;
        }

        if (scene.name == "Lobby_Scene" || scene.name == "JoinLAN_Scene")
        {
            noOfCPUs = 0;
            joinedLobby = true;
        }

        if (scene.name == "Title_Screen")
        {
            Destroy(this.gameObject);
        }
    }

    void ActivateCharacterSelectRoom()
    {
        characterSelectRoom.SetActive(true);
        characterSelectHUD.SetActive(true);
    }

	public void CharacterSelector(string buttonName)
	{
		switch (buttonName)
		{
		case "ErrNoName_btn":
                characterName = "Err:NoName";
				characterID = "ERNN";
			    characterIndex = 0;
                RemoveCustomization();
                arrayCount = 0;
                arrayMin = 0;
                arrayMax = 5;
                UpdateCustomization();
			    
                break;

		case "Sparkus_btn":
                characterName = "Sparkus";
				characterID = "SPKS";
                characterIndex = 5;
                RemoveCustomization();
                arrayCount = 5;
                arrayMin = 5;
                arrayMax = 10;
                UpdateCustomization();

                break;

		case "UnitD1_btn":
                characterName = "Unit-D1";
                characterID = "UT-D1";
                characterIndex = 10;
                RemoveCustomization();
                arrayCount = 10;
                arrayMin = 10;
                arrayMax = 15;
                UpdateCustomization();

                break;
		}

		characterTitle.text = characterName;
		playerPrefab = spawnPrefabs[characterIndex];
	}

    public void ChooseCustomizationViaRightButtons()
    {
        RemoveCustomization();

        arrayCount++;

        if (arrayCount >= arrayMax)
        {
			arrayCount = arrayMin;
        }

        UpdateCustomization();
    }

    public void ChooseCustomizationViaLeftButtons()
    {
        RemoveCustomization();

        arrayCount--;
  
        if (arrayCount <= arrayMin)
        {
			arrayCount = arrayMax;
        }

        UpdateCustomization();
    }

    void ChooseCustomizationViaKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
			ChooseCustomizationViaRightButtons ();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
			ChooseCustomizationViaLeftButtons ();
        }
    }

    void RemoveCustomization()
    {
        characterCustomization[arrayCount].SetActive(false);
    }


    void UpdateCustomization()
    {
        characterCustomization[arrayCount].SetActive(true);
        customzationName = characterCustomization[arrayCount].transform.name;
        characterIndex = arrayCount;
    }

    public void BeginIntroduction()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayIntroduction());
        }
    }

    IEnumerator PlayIntroduction()
    {
        isPlaying = true;

        if (characterID == "ERNN")
        {
            introAudioSource.clip = (AudioClip) Resources.Load("Noname introduction");
            introAudioSource.Play();
        }

        else if (characterID == "SPKS")
        {
            introAudioSource.clip = (AudioClip)Resources.Load("Sparkus introduction");
            introAudioSource.Play();
        }

        else if (characterID == "UT-D1")
        {
            introAudioSource.clip = (AudioClip)Resources.Load("D1 introduction");
            introAudioSource.Play();
        }

        yield return new WaitForSeconds(5);

        isPlaying = false;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        IntegerMessage msg = new IntegerMessage(characterIndex);
        characterSelectHUD.SetActive(false);

        //Debug.Log(msg);

        ClientScene.AddPlayer(conn, 0, msg);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        if (extraMessageReader != null)
        {
            IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage>();
            characterIndex = i.value;
        }

        GameObject playerPrefab = spawnPrefabs[characterIndex];
        Debug.Log("Player played as " + characterName);

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

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //base.OnClientSceneChanged(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //base.OnClientDisconnect(conn);
        SceneManager.LoadScene("Main_Menu");
        Destroy(this.gameObject);
    }

    public void IncreaseCPUs () {
		if (noOfCPUs < maxCPUs) {
			noOfCPUs += 1;
		}
	}
	public void DecreaseCPUs () {
		if (noOfCPUs > 0) {
			noOfCPUs -= 1;
		}
	}

    public void DestorySelf(bool isDestroy)
    {
        Destroy(this.gameObject);
    }
}
