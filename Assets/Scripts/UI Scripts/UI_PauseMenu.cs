using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class UI_PauseMenu : MonoBehaviour {

    public static bool isOn = false;

    [SerializeField]
    private GameObject pauseMenu;

    private NetworkManager networkManager;

	// Use this for initialization
	void Start ()
    {
        isOn = false;
        networkManager = NetworkManager.singleton;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
	}

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isOn = pauseMenu.activeSelf;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
