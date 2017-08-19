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

    public float maxUltimateGain = 100;

    [SyncVar]
    public float currentUltimateGain;

    public float GetUltimatePct()
    {
        return currentUltimateGain / maxUltimateGain;
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
    GameObject[] ERNNCustomization;
    [SerializeField]
    GameObject[] SPKSCustomization;
    [SerializeField]
    GameObject[] UT_D1Customization;

    private bool firstSetup = true;

    // Player Animator & Model
    public Animator playerAnim;
    public Transform playerModelTransform;

    //Particles
    private ParticleSystem playHitParticle;
    public ParticleSystem hitParticle;

    private ParticleSystem playSlowParticle;
    public ParticleSystem slowParticle;

	public GameObject corpse; // the player exploding on thier death, assigned in editor
	private ParticleSystem playDeathParticle;
	public ParticleSystem deathParticle;

    [SerializeField]
    GameObject netManagerGameObject;

    // Scripts
    Network_Soundscape networkSoundscape;
    Network_Manager networkManagerScript;

    void Start()
    {
        networkSoundscape = transform.GetComponent<Network_Soundscape>();
        netManagerGameObject = GameObject.FindGameObjectWithTag("NetManager");
        networkManagerScript = netManagerGameObject.GetComponent<Network_Manager>();
        playerCharacterID = networkManagerScript.characterID;
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

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

		//particles
        ParticleSystem playHitParticle = (ParticleSystem)Instantiate(hitParticle, this.transform.position, this.transform.rotation);
		hitParticle.Emit(1);

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

        Debug.Log("Taken damage");

		currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            DieFromTrap(_sourceID);
        }
    }

    [ClientRpc]
    public void RpcHealthRegenerate(float _amount, string _sourceID)
    {
        Debug.Log("Healing!");

		currentHealth += _amount /** Time.deltaTime*/;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    [ClientRpc]
    public void RpcUltimateCharging(float _amount, string _sourceID)
    {
        currentUltimateGain += _amount;

        if (currentUltimateGain >= maxUltimateGain)
        {
            currentUltimateGain = maxUltimateGain;
        }
    }

    private void Die(string _sourceID)
    {
        Network_PlayerManager sourcePlayer = Network_GameManager.GetPlayer(_sourceID);

        isDead = true;

        if (sourcePlayer != null)
        {
            sourcePlayer.killStats++;
            Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        if (sourcePlayer.killStats == 10)
        {
            Debug.Log("YAY!");

            if (playerCharacterID == "ERNN")
            {
                networkSoundscape.PlayNonNetworkedSound(4, 4);
            }

            if (playerCharacterID == "SPKS")
            {
                networkSoundscape.PlayNonNetworkedSound(5, 4);
            }

            if (playerCharacterID == "UT-D1")
            {
                networkSoundscape.PlayNonNetworkedSound(6, 4);
            }

            StartCoroutine(EndGame());
        }

        deathStats++;

        DisablePlayer();
    }

    private void DieFromTrap(string _sourceID)
    {
        isDead = true;

        deathStats++;

		// spawn corpse on death
		GameObject corpseobject = Instantiate (corpse, this.transform.position, this.transform.rotation) as GameObject;
		NetworkServer.Spawn(corpseobject);
		//ParticleSystem playDeathParticle = (ParticleSystem)Instantiate(deathParticle, this.transform.position, this.transform.rotation);
//		if (!particleSystemPlayed) 
//		{ 
//			playDeathParticle.Emit(0);
//			particleSystemPlayed = true;
//		}
//		if (particleSystemPlayed == true)
//		{
//			Destroy(playDeathParticle);
//		}
		//Destroy(corpseobject, 5);
		// end of spawn corpse on death

        DisablePlayer();
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

        Debug.Log(transform.name + " is DEAD!");

        GameObject corpseobject = Instantiate(corpse, this.transform.position, this.transform.rotation) as GameObject;
        NetworkServer.Spawn(corpseobject);
        Destroy(corpseobject, 5);

        ParticleSystem playDeathParticle = (ParticleSystem)Instantiate(deathParticle, this.transform.position, this.transform.rotation);

        //Switch cameras
        if (isLocalPlayer)
        {
            Network_GameManager.instance.SetSceneCameraActive(true);
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(Network_GameManager.instance.networkMatchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned.");

        randomSound = Random.Range(7, 13);

        networkSoundscape.PlayNonNetworkedSound(randomSound, 4);
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

        networkSoundscape.PlayNonNetworkedSound(16, 4);
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

        NetworkManager.Shutdown();
    }


    void OnTriggerStay(Collider col)
    {
        if (col.tag == "SlowTrap")
        {
            ParticleSystem playSlowParticle = (ParticleSystem)Instantiate(slowParticle, this.transform.position + Vector3.down, this.transform.rotation);
            playSlowParticle.Emit(1);

			/*playerAnim = GetComponent<Animator>();


			playerAnim.speed = 0.0f;*/

			Debug.Log("My animation should be slowed down...");

           
	}
}
}