using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHUD : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

    [SerializeField]
    MonoBehaviour[] playerScripts;

    [SerializeField]
    GameObject[] disableObjects;

    [SerializeField]
    Network_PlayerManager networkPlayerManager;

    [SerializeField]
    Network_CombatManager combatManager;

    PlayerController playerController;

    public void SetPlayer(Network_PlayerManager _networkPlayerManager)
    {
        networkPlayerManager = _networkPlayerManager;
        playerController = networkPlayerManager.GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start ()
    {
        UI_PauseMenu.IsOn = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckInput();
        CheckPauseMenu();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }

        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    void CheckPauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            // disable controls
            for (int i = 0; i < playerScripts.Length; i++)
            {
                playerScripts[i].enabled = false;
            }

            // disable objects

            for (int i = 0; i < disableObjects.Length; i++)
            {
                disableObjects[i].SetActive(false);
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!pauseMenu.activeSelf)
        {
            // enable controls
            for (int i = 0; i < playerScripts.Length; i++)
            {
                playerScripts[i].enabled = true;
            }

            // enable objects

            for (int i = 0; i < disableObjects.Length; i++)
            {
                disableObjects[i].SetActive(true);
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        UI_PauseMenu.IsOn = pauseMenu.activeSelf;
    }
}
