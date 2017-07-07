﻿using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHUD : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

    [SerializeField]
    GameObject[] playerHUD;

    [SerializeField]
    MonoBehaviour[] playerScripts;

    [SerializeField]
    Rigidbody playerRigidbody;

    [SerializeField]
    Animator playerAnimator;

    private Network_PlayerManager networkPlayerManager;

    private PlayerController playerController;

    public void SetController(PlayerController _controller)
    {
        playerController = _controller;
    }

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

        if (pauseMenu.activeSelf)
        {
            //playerAnimator.enabled = false;
            //playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

            for (int i = 0; i < playerHUD.Length; i++)
            {
                playerHUD[i].SetActive(false);
            }

            for (int i = 0; i < playerScripts.Length; i++)
            {
                playerScripts[i].enabled = false;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!pauseMenu.activeSelf)
        {
            //playerRigidbody.constraints = RigidbodyConstraints.None;
            //playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //playerAnimator.enabled = true;

            for (int i = 0; i < playerHUD.Length; i++)
            {
                playerHUD[i].SetActive(true);
            }

            for (int i = 0; i < playerScripts.Length; i++)
            {
                playerScripts[i].enabled = true;
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