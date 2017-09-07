using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHUD : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

	[SerializeField]
	GameObject SkillUI;

	[SerializeField]
	GameObject compactHUD;

	[SerializeField]
	GameObject gravityBlock;

    [SerializeField]
    MonoBehaviour[] playerScripts;

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

		if (Input.GetKeyDown(KeyCode.X))
		{
			SkillUI.SetActive(true);
		}

		else if (Input.GetKeyUp(KeyCode.X))
		{
			SkillUI.SetActive(false);
		}

        if (pauseMenu.activeSelf)
        {
            //playerAnimator.enabled = false;
            //playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

			compactHUD.SetActive(false);

			//disable controls
			for (int i = 0; i < playerScripts.Length; i++) {
					playerScripts [i].enabled = false;
				}
			gravityBlock.SetActive (false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!pauseMenu.activeSelf)
        {
            //playerRigidbody.constraints = RigidbodyConstraints.None;
            //playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //playerAnimator.enabled = true;
			compactHUD.SetActive(true);

			//enable controls
			for (int i = 0; i < playerScripts.Length; i++) {
				playerScripts [i].enabled = true;
			}
			gravityBlock.SetActive (true);

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
