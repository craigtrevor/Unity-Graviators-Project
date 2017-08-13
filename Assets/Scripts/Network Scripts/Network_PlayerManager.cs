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

    public int killStats;
    public int deathStats;

    public string playerCharacterID;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool firstSetup = true;

    [SerializeField]
    private int deaths;

    // Player Animator & Model
    public Animator playerAnim;
    public Transform playerModelTransform;

    //Particles
    private ParticleSystem playHitParticle;
    public ParticleSystem hitParticle;
    private bool hitParticleSystemPlayed = false;

    private ParticleSystem playSlowParticle;
    public ParticleSystem slowParticle;
    private bool slowParticlePlayed;

	public GameObject corpse; // the player exploding on thier death, assigned in editor
	private ParticleSystem playDeathParticle;
	public ParticleSystem deathParticle;
	bool particleSystemPlayed = false;

    [SerializeField]
    GameObject netManagerGameObject;
    Network_Manager networkManagerScript;

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            Network_GameManager.instance.SetSceneCameraActive(false);
        }

        CmdBroadCastNewPlayerSetup();
    }

    //void FixRotations()
    //{
    //    netManagerGameObject = GameObject.FindGameObjectWithTag("NetManager");
    //    networkManagerScript = netManagerGameObject.GetComponent<Network_Manager>();
    //    playerCharacterID = networkManagerScript.characterID;

    //    if (playerCharacterID == "ERNN")
    //    {
    //        Debug.Log("ROTATING!");
    //        playerModelTransform.Rotate(90, 0, 0);
    //    }
    //}

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


        ParticleSystem playHitParticle = (ParticleSystem)Instantiate(hitParticle, this.transform.position, this.transform.rotation);
        if (!hitParticleSystemPlayed)
        {
            {
                playHitParticle.Emit(1);
                hitParticleSystemPlayed = true;
                Debug.Log("Fly free my pretties");
            }
            if (hitParticleSystemPlayed == true)
            {
                Destroy(playHitParticle);
                Debug.Log("Bye felicia!");

            }
        }

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

        currentHealth += _amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    [ClientRpc]
    public void RpcUltimateCharging(float _amount)
    {
        // Debug.Log("Charging Ultimate!");

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

        deaths++;

        if (sourcePlayer != null)
        {
            sourcePlayer.killStats++;
            Network_GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        deathStats++;

        if (deaths == 10)
        {
            CmdMatchEnd();
        }

        DisablePlayer();
    }

    private void DieFromTrap(string _sourceID)
    {
        isDead = true;

        deaths++;
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

        if (deaths == 10)
        {
            CmdMatchEnd();
        }

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
        if (deaths != 10)
        {
            yield return new WaitForSeconds(Network_GameManager.instance.networkMatchSettings.respawnTime);

            Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;

            yield return new WaitForSeconds(0.1f);

            SetupPlayer();

            Debug.Log(transform.name + " respawned.");

        }
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

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
            if (!slowParticlePlayed)
            {
                {
                    playSlowParticle.Emit(1);
                    slowParticlePlayed = true;
                    Debug.Log("slowPlayed");
                }
                if (slowParticlePlayed == true)
                {
                    Destroy(playSlowParticle);
                    Debug.Log("slowDead");

                }
            }
        }

    }
}
