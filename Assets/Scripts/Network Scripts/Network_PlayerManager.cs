
using UnityEngine;
using UnityEngine.Networking;
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

    [SerializeField]
    public int killStats;
    public int deathStats;
    private int randomSound;

    public string playerCharacterID;

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

    [SerializeField]
    GameObject[] ERNNCustomization;
    [SerializeField]
    GameObject[] SPKSCustomization;
    [SerializeField]
    GameObject[] UT_D1Customization;

    private bool firstSetup = true;
    private bool firstPlay = false;
    private bool deathbyPlayer;
    private bool deathbyTrap;

    // Player Animator & Model
    public Animator playerAnim;
    public Transform playerModelTransform;

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

    // Network Booleans
    public bool isPlayerServer;

    public override void PreStartClient()
    {
        netManagerGameObject = GameObject.FindGameObjectWithTag("NetManager");
        networkManagerScript = netManagerGameObject.GetComponent<Network_Manager>();
        playerCharacterID = networkManagerScript.characterID;
		particleManager = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<ParticleManager>();
    }

    public override void OnStartLocalPlayer()
    {
        if (!firstPlay)
        {
            networkSoundscape = transform.GetComponent<Network_Soundscape>();
            //networkSoundscape.PlayNonNetworkedSound(16, 4);
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

    private void Update()
    {
        MuteNarration();
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            Network_GameManager.instance.SetSceneCameraActive(false);
        }

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

    [ClientRpc]
    public void RpcTakeDamage(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        Debug.Log("Taken damage");

        currentHealth -= _amount;

		playerAnim.SetTrigger ("Flinch");

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

		//particles

		GameObject playHitParticle = Instantiate(particleManager.GetParticle("hitParticle"), this.transform.position, this.transform.rotation);

        if (currentHealth <= 0)
        {
            Die(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcTakeTrapDamage(float _amount, string _sourceID)
    {
        if (isDead)
            return;
		currentHealth -= _amount;

		playerAnim.SetTrigger ("Flinch");

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            DieFromTrap(_sourceID);
        }
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

    private void Die(string _sourceID)
    {
        Network_PlayerManager sourcePlayer = Network_GameManager.GetPlayer(_sourceID);

        isDead = true;
        deathbyPlayer = true;
        deathbyTrap = false;

        if (sourcePlayer != null)
        {
            if (isLocalPlayer)
            {
                randomSound = Random.Range(20, 23);
                networkSoundscape.PlayNonNetworkedSound(randomSound, 4, 1f);
            }

            sourcePlayer.killStats++;
            Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
            DisablePlayer();
        }

        if (sourcePlayer.killStats == 10)
        {
            //if (playerCharacterID == "ERNN")
            //{
            //    networkSoundscape.PlayNonNetworkedSound(20, 4, 1f);
            //}

            //if (playerCharacterID == "SPKS")
            //{
            //    networkSoundscape.PlayNonNetworkedSound(21, 4, 1f);
            //}

            //if (playerCharacterID == "UT-D1")
            //{
            //    networkSoundscape.PlayNonNetworkedSound(22, 4, 1f);
            //}

            Network_SceneManager.instance.wonMatch = true;
            Network_SceneManager.instance.playerUsername = username;

            StartCoroutine(EndGame());
        }

        deathStats++;
    }

    private void DieFromTrap(string _sourceID)
    {
        isDead = true;
        deathbyPlayer = false;
        deathbyTrap = true;

        deathStats++;

		// spawn corpse on death
		Debug.Log (particleManager.GetParticle("deathParticle"));
		GameObject playDeathParticle = Instantiate(particleManager.GetParticle("deathParticle"), this.transform.position, this.transform.rotation);
        DisablePlayer();

        if (!isServer)
            return;

        GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
        NetworkServer.Spawn(corpseobject);
    }

    private void DisablePlayer()
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

        StartCoroutine(Respawn());

        onDeath();
    }

    private IEnumerator Respawn()
    {
        if (isLocalPlayer)
        {
            if (deathbyPlayer)
            {
				yield return new WaitForSeconds(1f);
                deathCanvas[0].SetActive(true);
                yield return new WaitForSeconds(5f);
                deathCanvas[0].SetActive(false);
                deathCamera.SetActive(false);

                Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
                transform.position = _spawnPoint.position;
                transform.rotation = _spawnPoint.rotation;

                SetupPlayer();

                Debug.Log(transform.name + " respawned.");

                randomSound = Random.Range(18, 20);
                networkSoundscape.PlayNonNetworkedSound(randomSound, 4, 1f);
            }

            else if (deathbyTrap)
            {
				yield return new WaitForSeconds(1f);
                deathCanvas[1].SetActive(true);
                yield return new WaitForSeconds(5f);
                deathCanvas[1].SetActive(false);
                deathCamera.SetActive(false);

                Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
                transform.position = _spawnPoint.position;
                transform.rotation = _spawnPoint.rotation;

                SetupPlayer();

                Debug.Log(transform.name + " respawned.");

                randomSound = Random.Range(18, 20);
                networkSoundscape.PlayNonNetworkedSound(randomSound, 4, 1f);
            }
        }
    }

    void onDeath()
    {
        Debug.Log(transform.name + " is DEAD!");

        if (isLocalPlayer)
        {
            //Switch cameras
            //deathCamera.transform.position = this.transform.position;
            //deathCamera.transform.rotation = this.transform.rotation;
            deathCamera.transform.position = mainCamera.transform.position;
            deathCamera.transform.rotation = mainCamera.transform.rotation;
            deathCamera.SetActive(true);
            GetComponent<Rigidbody>().Sleep();
        }

        GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
				GameObject playDeathParticle = Instantiate(particleManager.GetParticle("deathParticle"), this.transform.position, this.transform.rotation);

        //Destroy(corpseobject, 5);

        if (!isServer)
            return;

        NetworkServer.Spawn(corpseobject);

    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        if (isLocalPlayer)
        {
            CheckCustomizations();
        }

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
    }

    void CheckCustomizations()
    {
        if (playerCharacterID == "ERNN")
        {
            if (networkManagerScript.customzationName == "empty hat")
            {
                ERNNCustomization[0].SetActive(true);
            }

            else if (networkManagerScript.customzationName == "samurai hat")
            {
                ERNNCustomization[1].SetActive(true);
            }

            else if (networkManagerScript.customzationName == "cowboy hat")
            {
                ERNNCustomization[2].SetActive(true);
            }
        }

        else if (playerCharacterID == "SPKS")
        {
            if (networkManagerScript.customzationName == "empty hat")
            {
                SPKSCustomization[0].SetActive(true);
            }

            else if (networkManagerScript.customzationName == "centurion")
            {
                SPKSCustomization[1].SetActive(true);
            }

            else if (networkManagerScript.customzationName == "fez")
            {
                SPKSCustomization[2].SetActive(true);
            }
        }

        else if (playerCharacterID == "UT-D1")
        {
            if (networkManagerScript.customzationName == "empty hat")
            {
                UT_D1Customization[0].SetActive(true);
            }

            else if (networkManagerScript.customzationName == "flower")
            {
                UT_D1Customization[1].SetActive(true);
            }

            else if (networkManagerScript.customzationName == "santa hat")
            {
                UT_D1Customization[2].SetActive(true);
            }
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        CmdMatchEnd();
    }

    [Command]
    public void CmdMatchEnd()
    {
        Debug.Log("Match has finished");

       // RpcEndMatch();

        NetworkManager.Shutdown();
       // SceneManager.LoadScene("EndMatch_Scene");
    }

    [ClientRpc]
    public void RpcEndMatch()
    {
        if (!isLocalPlayer)
        {
            if (Network_SceneManager.instance.wonMatch == true)
            {
                Network_SceneManager.instance.wonMatch = false;
            }
        }
    }

    void MuteNarration()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            narrationAudio.Stop();
        }
    }

	void OnTriggerStay(Collider col){
		if (col.tag == "SlowTrap") {
			GameObject playSlowParticle = Instantiate (particleManager.GetParticle("slowParticle"), this.transform.position, this.transform.rotation);
		}
	}
}