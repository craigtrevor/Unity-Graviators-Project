using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class UI_PauseMenu : MonoBehaviour {

    public static bool IsOn = false;

    //[SerializeField]
    //private GameObject pauseMenu;

    //[SerializeField]
    //private GameObject playerMenu;

    //[SerializeField]
    //private Canvas playerHUD;

    private NetworkManager networkManager;

    //void Awake()
    //{
    //    isOn = false;
    //    networkManager = NetworkManager.singleton;
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }
	
	//// Update is called once per frame
	//void Update ()
 //   {
 //       if (Input.GetKeyDown(KeyCode.Escape))
 //       {
 //           TogglePauseMenu();
 //       }
 //   }

    //void TogglePauseMenu()
    //{
    //    playerMenu = GameObject.FindWithTag("PlayerHUD");
    //    playerHUD = playerMenu.GetComponent<Canvas>();

    //    isOn = pauseMenu.activeSelf;
    //    pauseMenu.SetActive(!pauseMenu.activeSelf);

    //    if (pauseMenu.activeSelf)
    //    {
    //        playerHUD.enabled = false;
    //        Cursor.visible = true;
    //        Cursor.lockState = CursorLockMode.None;
    //    }

    //    if (!pauseMenu.activeSelf)
    //    {
    //        playerHUD.enabled = true;
    //        Cursor.visible = false;
    //        Cursor.lockState = CursorLockMode.Locked;
    //    }
    //}

    public void LeaveRoom() 
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopClient();
    }
}
