using System;
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
    GameObject[] CharacterSpotlight;

    [SerializeField]
    GameObject[] ERNNCustomization;
    [SerializeField]
    GameObject[] SPKSCustomization;
    [SerializeField]
    GameObject[] UT_D1Customization;

    [SerializeField]
    GameObject[] CharacterCustomizationButtons;

    public string customzationName;

    int arrayCount;
    int arrayMax;
    int arrayMin;

	[SerializeField]
	public int noOfCPUs = 0;

	[SerializeField]
	public int maxCPUs = 4;

    void Start()
    {
        characterIndex = 0;
        arrayMax = 4;
        arrayMin = 0;
        arrayCount = 0;
        characterName = "Err:NoName";
        characterID = "ERNN";
        customzationName = "empty hat";

        CharacterCustomizationButtons[0].SetActive(true);
        CharacterCustomizationButtons[1].SetActive(true);
        CharacterCustomizationButtons[2].SetActive(false);
        CharacterCustomizationButtons[3].SetActive(false);
        CharacterCustomizationButtons[4].SetActive(false);
        CharacterCustomizationButtons[5].SetActive(false);

        CharacterSpotlight[0].SetActive(true);
        CharacterSpotlight[1].SetActive(false);
        CharacterSpotlight[2].SetActive(false);

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
            ChooseCustomizationViaKeyboard();
            ChooseCustomizationViaRightButtons();
            ChooseCustomizationViaLeftButtons();
        }

        else if (Network_SceneManager.instance.sceneName != "Character_Select")
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
                RemoveCustomization();
                arrayCount = 0;
                customzationName = "empty hat";
                characterName = "Err:NoName";
                characterID = "ERNN";

                CharacterSpotlight[0].SetActive(true);
                CharacterSpotlight[1].SetActive(false);
                CharacterSpotlight[2].SetActive(false);

                CharacterCustomizationButtons[0].SetActive(true);
                CharacterCustomizationButtons[1].SetActive(true);
                CharacterCustomizationButtons[2].SetActive(false);
                CharacterCustomizationButtons[3].SetActive(false);
                CharacterCustomizationButtons[4].SetActive(false);
                CharacterCustomizationButtons[5].SetActive(false);

                break;

            case "Sparkus_btn":

                characterIndex = 1;
                RemoveCustomization();
                arrayCount = 0;
                customzationName = "empty hat";
                characterName = "Sparkus";
                characterID = "SPKS";

                CharacterCustomizationButtons[0].SetActive(false);
                CharacterCustomizationButtons[1].SetActive(false);
                CharacterCustomizationButtons[2].SetActive(true);
                CharacterCustomizationButtons[3].SetActive(true);
                CharacterCustomizationButtons[4].SetActive(false);
                CharacterCustomizationButtons[5].SetActive(false);

                CharacterSpotlight[0].SetActive(false);
                CharacterSpotlight[1].SetActive(true);
                CharacterSpotlight[2].SetActive(false);

                break;

            case "UnitD1_btn":
                characterIndex = 2;
                RemoveCustomization();
                arrayCount = 0;
                customzationName = "empty hat";
                characterName = "Unit-D1";
                characterID = "UT-D1";

                CharacterCustomizationButtons[0].SetActive(false);
                CharacterCustomizationButtons[1].SetActive(false);
                CharacterCustomizationButtons[2].SetActive(false);
                CharacterCustomizationButtons[3].SetActive(false);
                CharacterCustomizationButtons[4].SetActive(true);
                CharacterCustomizationButtons[5].SetActive(true);

                CharacterSpotlight[0].SetActive(false);
                CharacterSpotlight[1].SetActive(false);
                CharacterSpotlight[2].SetActive(true);
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
            arrayCount = 1;
            customzationName = "empty hat";
        }

        UpdateCustomization();
    }

    public void ChooseCustomizationViaLeftButtons()
    {
        RemoveCustomization();

        arrayCount--;

        if (arrayCount <= arrayMin)
        {
            arrayCount = 3;
        }

        UpdateCustomization();
    }

    void ChooseCustomizationViaKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RemoveCustomization();

            arrayCount++;

            if (arrayCount >= arrayMax)
            {
                arrayCount = 1;
                customzationName = "empty hat";
            }

            UpdateCustomization();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RemoveCustomization();

            arrayCount--;

            if (arrayCount <= arrayMin)
            {
                arrayCount = 3;
            }

            UpdateCustomization();
        }
    }

    void RemoveCustomization()
    {
        ERNNCustomization[arrayCount].SetActive(false);
        SPKSCustomization[arrayCount].SetActive(false);
        UT_D1Customization[arrayCount].SetActive(false);
    }

    void UpdateCustomization()
    {
        if (characterID == "ERNN")
        {
            ERNNCustomization[arrayCount].SetActive(true);
            customzationName = ERNNCustomization[arrayCount].transform.name;
        }

        else if (characterID == "SPKS")
        {
            SPKSCustomization[arrayCount].SetActive(true);
            customzationName = SPKSCustomization[arrayCount].transform.name;
        }

        else if (characterID == "UT-D1")
        {
            UT_D1Customization[arrayCount].SetActive(true);
            customzationName = UT_D1Customization[arrayCount].transform.name;
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

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //base.OnClientSceneChanged(conn);
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
}
