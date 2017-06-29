using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Network_PlayerManager))]
public class Network_PlayerScore : MonoBehaviour {

    Network_PlayerManager networkPlayerManager;

    void Start()
    {
        networkPlayerManager = GetComponent<Network_PlayerManager>();
        StartCoroutine(SyncScoreLoop());
    }

    void OnDestroy()
    {
        if (networkPlayerManager != null)
            SyncNow();
    }

    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            SyncNow();
        }
    }

    void SyncNow()
    {
        if (UI_UserAccountManager.IsLoggedIn)
        {
            UI_UserAccountManager.instance.GetData(OnDataRecieved);
        }
    }

    void OnDataRecieved(string data)
    {
        if (networkPlayerManager.killStats == 0 && networkPlayerManager.deathStats == 0)
            return;

        int killStats = Network_DataTranslator.DataToKills(data);
        int deathStats = Network_DataTranslator.DataToDeaths(data);

        int newKillsStats = networkPlayerManager.killStats + killStats;
        int newDeathStats = networkPlayerManager.deathStats + deathStats;

        string newData = Network_DataTranslator.ValuesToData(newKillsStats, newDeathStats);

        Debug.Log("Syncing: " + newData);

        networkPlayerManager.killStats = 0;
        networkPlayerManager.deathStats = 0;

        UI_UserAccountManager.instance.SendData(newData);
    }
}
