using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_SinglePlayerPauseMenu : MonoBehaviour {

	public static bool isOn = false;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject playerMenu;

    [SerializeField]
    private Canvas playerHUD;

    void Start()
    {
        isOn = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		isOn = pauseMenu.activeSelf;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();         
        }
    }

    void TogglePauseMenu()
    {
        playerMenu = GameObject.FindWithTag("PlayerHUD");
        playerHUD = playerMenu.GetComponent<Canvas>();

        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            playerHUD.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!pauseMenu.activeSelf)
        {
            playerHUD.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ContinueGame()
    {
		
		playerHUD.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		pauseMenu.SetActive (false);

    }

    public void LeaveGame()
    {
		SceneManager.LoadScene("Main_Menu");
    }
}
