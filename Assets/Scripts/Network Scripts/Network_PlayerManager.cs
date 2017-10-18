using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Network_PlayerSetup))]
public class Network_PlayerManager : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    public float maxHealth = 100;

    [SyncVar]
    private float currentHealth;

    public float GetHealthPct()
    {
        return currentHealth / maxHealth;
    }
		
	public float maxUltCharge = 100;

    [SyncVar]
    public float currentUltimateGain;

    public float GetUltimatePct()
    {
		return currentUltimateGain / maxUltCharge;
    }

    [SyncVar]
    public string username = "Loading...";

    public int killStats;
    public int deathStats;
    private int randomSound;
    private int soundCounter = 0;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    GameObject deathCamera;

    public GameObject mainCamera;

    [SerializeField]
    GameObject[] deathCanvas;
    int deathCanvasInt;

    public string playerCharacterID;

    private bool firstSetup = true;
    private bool firstPlay = false;
    private bool firstDeath = true;
    private bool deathbyPlayer;
    private bool deathbyTrap;
    private bool deathbyBot;
    private bool isPlaying = false;

    // Player Animator & Model
    public Animator playerAnim;
    public Transform playerModelTransform;
	public Network_CombatManager combatManager;

    //Gravity axis script
    GravityAxisScript gravityScript;

    //Particles
    public ParticleManager particleManager;

    private ParticleSystem playHitParticle;

    private ParticleSystem playSlowParticle;

	public GameObject corpse; // the player exploding on thier death, assigned in editor
	private ParticleSystem playDeathParticle;

    [SerializeField]
    AudioSource narrationAudio;

    [SerializeField]
    GameObject netManagerGameObject;

    // Scripts
    Network_Soundscape networkSoundscape;
    Network_Manager networkManagerScript;
    UI_PauseMenu pauseMenu;
    PlayerController playerControllerScript;

    // Network Booleans
    [HideInInspector]
    public bool isPlayerServer;

    public override void PreStartClient()
    {
        netManagerGameObject = GameObject.FindGameObjectWithTag("NetManager");
        networkManagerScript = netManagerGameObject.GetComponent<Network_Manager>();
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
		combatManager = this.gameObject.GetComponent<Network_CombatManager> ();
        playerControllerScript = GetComponentInChildren<PlayerController>();
        gravityScript = GetComponentInChildren<GravityAxisScript>();
        pauseMenu = GetComponentInChildren<UI_PauseMenu>();
        playerCharacterID = networkManagerScript.characterID;
    }

    public override void OnStartLocalPlayer()
    {
        if (!firstPlay)
        {
            networkSoundscape = transform.GetComponent<Network_Soundscape>();
            networkSoundscape.PlayNonNetworkedSound(18, 5, 0.2f);

            firstPlay = true;
        }

        if (isServer)
        {
            isPlayerServer = true;
        }

        else if (!isServer)
        {
            isPlayerServer = false;
        }
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            Network_GameManager.instance.SetSceneCameraActive(false);
        }

        //netMatchEnd = GameObject.Find("GameManager").GetComponent<Network_MatchEnd>();
        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        combatManager.isStunned = false;
        playerControllerScript.stunned = false;
        combatManager.StopAllCoroutines();
        playerControllerScript.ResetMoveSettings();

        currentHealth = maxHealth;

        GetComponent<Rigidbody>().detectCollisions = true;

        //CheckCustomizations();

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //Enable the gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        Collider _playerCol = playerModelTransform.parent.GetComponent<Collider>();
        if (_playerCol != null)
            _playerCol.enabled = true;
    }

    private void Update()
    {
        if (currentUltimateGain >= 100 && !isPlaying && networkSoundscape != null)
        {
            networkSoundscape.PlayNonNetworkedSound(30, 2, 0.5f);
            isPlaying = true;
        }

        else if (currentUltimateGain == 0)
        {
            isPlaying = false;
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        PlayerTakenDamage(_amount);

        //particles

        GameObject playHitParticle = Instantiate(particleManager.GetParticle("hitParticle"), this.transform.position, this.transform.rotation);

        if (currentHealth <= 0)
        {
            Die(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcTakeDamageByRange(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        PlayerTakenDamage(_amount);

        //particles

        GameObject playHitParticle = Instantiate(particleManager.GetParticle("hitParticle"), this.transform.position, this.transform.rotation);

        if (currentHealth <= 0)
        {
            DieByRange(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcTakeDamageByUlt(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        PlayerTakenDamage(_amount);

        //particles

        GameObject playHitParticle = Instantiate(particleManager.GetParticle("hitParticle"), this.transform.position, this.transform.rotation);

        if (currentHealth <= 0)
        {
            DieByUlt(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcTakDamageByTrap(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        PlayerTakenDamage(_amount);

        if (currentHealth <= 0)
        {
            DieByTrap(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcTakeDamageByBot(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        PlayerTakenDamage(_amount);

        if (currentHealth <= 0)
        {
            DieByBot(_sourceID);
        }
    }

    void PlayerTakenDamage(float _amount)
    {
        //Debug.Log("Taken damage");

        currentHealth -= _amount;

        if (!combatManager.isAttacking)
        {
            playerAnim.SetTrigger("Flinch");
        }

       // Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

	private void CancelAttacks() {
		combatManager.isAttacking = false;
		combatManager.isHitting = false;
		combatManager.isSlowed = false;
		combatManager.isStunned = false;
		combatManager.isRanging = false;
		combatManager.StopAllCoroutines ();
	}

    private void Die(string _sourceID)
    {
        Network_PlayerManager sourcePlayer = Network_GameManager.GetPlayer(_sourceID);

        playerModelTransform.parent.transform.GetComponent<PlayerController>().isShiftPressed = false;
        gravityScript.SetShiftPressed(false);

        isDead = true;
        deathbyPlayer = true;
        deathbyTrap = false;
        deathbyBot = false;

		CancelAttacks ();

        if (sourcePlayer != null)
        {
            sourcePlayer.killStats++;

            Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        if (killStats != Network_MatchEnd.instance.matchCount)
        {
            DisableComponents();
        }

        else if (killStats == Network_MatchEnd.instance.matchCount)
        {
            DisablePlayerOnMatchEnd();
        }

        Debug.Log("Player killed by player's melee");
        deathStats++;
    }

    private void DieByRange(string _sourceID)
    {
        Network_PlayerManager sourcePlayer = Network_GameManager.GetPlayer(_sourceID);

        playerModelTransform.parent.transform.GetComponent<PlayerController>().isShiftPressed = false;
        gravityScript.SetShiftPressed(false);

        isDead = true;
        deathbyPlayer = true;
        deathbyTrap = false;
        deathbyBot = false;

        CancelAttacks();

        if (sourcePlayer != null)
        {
            sourcePlayer.killStats++;

            Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        if (killStats != Network_MatchEnd.instance.matchCount)
        {
            DisableComponents();
        }

        else if (killStats == Network_MatchEnd.instance.matchCount)
        {
            DisablePlayerOnMatchEnd();
        }

        Debug.Log("Player killed by player's ranged weapon");
        deathStats++;
    }

    private void DieByUlt(string _sourceID)
    {
        Network_PlayerManager sourcePlayer = Network_GameManager.GetPlayer(_sourceID);

        playerModelTransform.parent.transform.GetComponent<PlayerController>().isShiftPressed = false;
        gravityScript.SetShiftPressed(false);

        isDead = true;
        deathbyPlayer = true;
        deathbyTrap = false;
        deathbyBot = false;

        CancelAttacks();

        if (sourcePlayer != null)
        {
            sourcePlayer.killStats++;

            Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        if (killStats != Network_MatchEnd.instance.matchCount)
        {
            DisableComponents();
        }

        else if (killStats == Network_MatchEnd.instance.matchCount)
        {
            DisablePlayerOnMatchEnd();
        }

        Debug.Log("Player killed by player's ultimate");
        deathStats++;
    }

    private void DieByTrap(string _sourceID)
    {
        playerModelTransform.parent.transform.GetComponent<PlayerController>().isShiftPressed = false;
        gravityScript.SetShiftPressed(false);
        
        isDead = true;
        deathbyPlayer = false;
        deathbyTrap = true;
        deathbyBot = false;

		CancelAttacks ();

        deathStats++;

        // spawn corpse on death
        ///Debug.Log(particleManager.GetParticle("deathParticle"));
        GameObject playDeathParticle = Instantiate(particleManager.GetParticle("deathParticle"), this.transform.position, this.transform.rotation);
        DisableComponents();

        Debug.Log("Player killed by trap");

        if (!isServer)
            return;

        GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
        NetworkServer.Spawn(corpseobject);
    }

    private void DieByBot(string _sourceID)
    {
		Network_Bot sourceBot = Network_GameManager.GetBot(_sourceID);

		playerModelTransform.parent.transform.GetComponent<PlayerController>().isShiftPressed = false;
        gravityScript.SetShiftPressed(false);

        isDead = true;
        deathbyPlayer = false;
        deathbyTrap = false;
        deathbyBot = true;

		CancelAttacks ();

        deathStats++;

        if (sourceBot != null)
		{
			sourceBot.killStats++;

			Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourceBot.username);
            DisableComponents();
		}

        Debug.Log("Player killed by bot");

        // spawn corpse on death
        //Debug.Log(particleManager.GetParticle("deathParticle"));
        GameObject playDeathParticle = Instantiate(particleManager.GetParticle("deathParticle"), this.transform.position, this.transform.rotation);
        DisableComponents();

        if (!isServer)
            return;

        GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
        NetworkServer.Spawn(corpseobject);
    }

    void DisableComponents()
    {
        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Collider _playerCol = playerModelTransform.parent.GetComponent<Collider>();
        if (_playerCol != null)
            _playerCol.enabled = false;

        onDeath();

        if (isLocalPlayer && soundCounter == 0)
        {
            randomSound = Random.Range(23, 26);
            networkSoundscape.PlayNonNetworkedSound(randomSound, 5, 0.2f);
            soundCounter++;
        }

        soundCounter--;
    }

    void onDeath()
    {
        //Debug.Log(transform.name + " is DEAD!");

        if (isLocalPlayer)
        {
            //Switch cameras
            //deathCamera.transform.position = this.transform.position;
            //deathCamera.transform.rotation = this.transform.rotation;
            deathCamera.transform.position = mainCamera.transform.position;
            deathCamera.transform.rotation = mainCamera.transform.rotation;
            deathCamera.SetActive(true);
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().detectCollisions = false;
        }

        GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
        GameObject playDeathParticle = Instantiate(particleManager.GetParticle("deathParticle"), this.transform.position, this.transform.rotation);

        //Destroy(corpseobject, 5);

        Respawn();

        if (!isServer)
            return;

        NetworkServer.Spawn(corpseobject);
    }


    void Respawn()
    {
        if (isLocalPlayer)
        {
            StartCoroutine(StartRespawning());
        }
    }


    IEnumerator StartRespawning()
    {
        if (deathbyPlayer || deathbyBot)
        {
            deathCanvasInt = 0;
        }

        else if (deathbyTrap)
        {
            deathCanvasInt = 1;
        }

        yield return new WaitForSeconds(1f);
        deathCanvas[deathCanvasInt].SetActive(true);
        yield return new WaitForSeconds(4f);

        PlayerRespawning();
    }

    void PlayerRespawning()
    {
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        StartCoroutine(InitializeSpawn());
    }

    IEnumerator InitializeSpawn()
    {
        yield return new WaitForSeconds(1f);

        deathCanvas[deathCanvasInt].SetActive(false);
        deathCamera.SetActive(false);

        randomSound = Random.Range(21, 22);
        networkSoundscape.PlayNonNetworkedSound(randomSound, 5, 1f);

        SetupPlayer();
    }

    [ClientRpc]
    public void RpcHealthFlatRegenerate(float _amount, string _sourceID)
    {
		currentHealth += _amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

	[ClientRpc]
	public void RpcHealthRegenerate(float _amount, string _sourceID)
	{
		currentHealth += _amount * Time.deltaTime / 10f;

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
		}
	}

    [ClientRpc]
    public void RpcUltimateFlatCharging(float _amount, string _sourceID)
    {
		currentUltimateGain += _amount;
        
		if (currentUltimateGain >= maxUltCharge)
        {
			currentUltimateGain = maxUltCharge;
        }
    }

	[ClientRpc]
	public void RpcUltimateCharging(float _amount, string _sourceID)
	{
		currentUltimateGain += _amount * Time.deltaTime / 3f;

		if (currentUltimateGain >= maxUltCharge)
		{
			currentUltimateGain = maxUltCharge;
        }
	}

    //void MuteNarration()
    //{
    //    if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        narrationAudio.Stop();
    //    }
    //}

	void OnTriggerStay(Collider col){
		if (col.tag == "SlowTrap") {
			GameObject playSlowParticle = Instantiate (particleManager.GetParticle("slowParticle"), this.transform.position, this.transform.rotation);
		}
	}

    public void DisablePlayerOnMatchEnd()
    {
        DisableComponentOnEndMatch();
    }

    void DisableComponentOnEndMatch()
    {
        //if (isLocalPlayer)
        //{
        //    randomSound = Random.Range(20, 23);
        //    networkSoundscape.PlayNonNetworkedSound(randomSound, 4, 1f);
        //}

        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        GetComponent<Rigidbody>().Sleep();
        GetComponent<Rigidbody>().detectCollisions = false;

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;
    }
}