    using UnityEngine;
using UnityEngine.Networking;

public class Network_PlayerStats : NetworkBehaviour {

    [SerializeField]
    public float maxHealth = 100;

    [SyncVar]
    private float currentHealth;

    void Awake()
    {
        SetDefaults();
    }

    public void TakeDamage(float _amount)
    {
        Debug.Log("Taken damage");

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

}
