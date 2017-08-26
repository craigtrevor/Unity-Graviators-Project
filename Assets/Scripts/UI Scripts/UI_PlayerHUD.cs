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
    GameObject[] playerHUD;

	[SerializeField]
	GameObject compactHUD;

	[SerializeField]
	GameObject gravityBlock;

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    RectTransform ultBarFill;

    [SerializeField]
    MonoBehaviour[] playerScripts;

    [SerializeField]
    Network_PlayerManager networkPlayerManager;

    [SerializeField]
    Network_CombatManager combatManager;

    PlayerController playerController;

	public bool newHUD;

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

		if (newHUD) {
			for (int i = 0; i < playerHUD.Length; i++) {
				playerHUD [i].SetActive (false);
			}
			compactHUD.SetActive (true);
		}

		if (!newHUD) {
			for (int i = 0; i < playerHUD.Length; i++) {
				playerHUD [i].SetActive (true);
			}
			compactHUD.SetActive (false);
		}

		if (!newHUD) {
			SetHealthAmount (networkPlayerManager.GetHealthPct ());
			SetUltBar (networkPlayerManager.GetUltimatePct ());
		}

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

		if (Input.GetKeyDown(KeyCode.O))
		{
			if (newHUD == false) {
				newHUD = true;
			} else {
				newHUD = false;
			}

		}

        if (pauseMenu.activeSelf)
        {
            //playerAnimator.enabled = false;
            //playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

			if (newHUD) {
				compactHUD.SetActive(false);
			}

			if (!newHUD) {
				for (int i = 0; i < playerHUD.Length; i++) {
					playerHUD [i].SetActive (false);
				}
			}

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

			if (newHUD) {
				compactHUD.SetActive(true);
			}

			if (!newHUD) {
				for (int i = 0; i < playerHUD.Length; i++) {
					playerHUD [i].SetActive (true);
				}
			}

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

    void SetHealthAmount (float _amount)
    {
        healthBarFill.localScale = new Vector3(_amount, 0.3f, 1f);

    }
    void SetUltBar (float _ultAmount) {

		ultBarFill.localScale = new Vector3(_ultAmount, 1f, 1f);
    }
}
