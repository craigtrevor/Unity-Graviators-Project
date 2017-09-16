using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Network_PlayerManager))]
public class Network_PlayerScore : MonoBehaviour {

    int lastKills = 0;
    int lastDeaths = 0;

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
        if (networkPlayerManager.killStats <= lastKills && networkPlayerManager.deathStats <= lastDeaths)
            return;

        int killsSinceLast = networkPlayerManager.killStats - lastKills;
        int deathsSinceLast = networkPlayerManager.deathStats - lastDeaths;

        int killStats = Network_DataTranslator.DataToKills(data);
        int deathStats = Network_DataTranslator.DataToDeaths(data);

        int newKillsStats = killsSinceLast + killStats;
        int newDeathStats = deathsSinceLast + deathStats;

        string newData = Network_DataTranslator.ValuesToData(newKillsStats, newDeathStats);

        Debug.Log("Syncing: " + newData);

        lastKills = networkPlayerManager.killStats;
        lastDeaths = networkPlayerManager.deathStats;

        UI_UserAccountManager.instance.SendData(newData);
    }
}
