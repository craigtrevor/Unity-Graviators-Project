﻿using UnityEngine;
using UnityEngine.Networking;

public class Network_PlayerStats : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;

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
