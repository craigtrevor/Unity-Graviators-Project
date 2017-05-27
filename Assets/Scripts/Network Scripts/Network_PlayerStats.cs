using UnityEngine;
using UnityEngine.Networking;

public class Network_PlayerStats : NetworkBehaviour {

    public int maxHealth = 100; // changed to public so script can access it

    [SyncVar]
    private int currentHealth;

    void Awake()
    {
        SetDefaults();
    }

    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
