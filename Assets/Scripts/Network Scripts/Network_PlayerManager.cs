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

    [SerializeField]
    private float maxHealth = 100;

    public GameObject healthBar;

    [SyncVar]
    private float currentHealth;

    public int killStats;
    public int deathStats;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool firstSetup = true;

    [SerializeField]
    private int deaths;

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

    //void Update()
    //{
    //    if (isLocalPlayer)
    //    {
    //        if (Input.GetKeyDown(KeyCode.K))
    //        {
    //            RpcTakeDamage(100);
    //            deaths--;
    //        }
    //    }
    //}

    [ClientRpc]
    public void RpcTakeDamage(float _amount, string _sourceID)
    {
        if (isDead)
            return;

        Debug.Log("Taken damage");

        currentHealth -= _amount;

        if (isLocalPlayer)
        {
            //calculates the players remaining health and updates health bar based on it
            //calculation will result in between 1 and 0 (eg. 80/100 = 0.6)

            float calculateHealth = currentHealth / maxHealth;
            SetHealthBar(calculateHealth);
        }

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die(_sourceID);         
        }
    }

    private void Die(string _sourceID)
    {
        isDead = true;

        deaths++;
        deathStats++;

        Network_PlayerManager sourcePlayer = Network_GameManager.GetPlayer(_sourceID);

        if (sourcePlayer != null)
        {
            sourcePlayer.killStats++;
        }

        if (deaths == 10)
        {
            CmdMatchEnd();
        }

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

            SetDefaults();
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

    public void SetHealthBar(float playerHealth)
    {
        //playerHealth value between 0-1
        //adjusts scale of player health bar between 0-1 based on above calculation
        healthBar.transform.localScale = new Vector3(playerHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    [Command]
    public void CmdMatchEnd()
    {
        Debug.Log("Match has finished");

        NetworkManager.Shutdown();

    }

}
