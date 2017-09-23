using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_BotSpawner : MonoBehaviour {

    [SerializeField]
    GameObject networkBot;

    [SerializeField]
    Transform botSpawnPoint;

    GameObject spawnedBot;

    float maxSpawnRateInSeconds = 5f;

    [SerializeField]
    int spawnAmount;

    void Start()
    {
        if (Network_SceneManager.instance.serverScene == "JoinPracticeRange_Scene")
        {
            ScheduleNextEnemySpawn();
        }
    }

    void SpawnBot()
    {
        spawnedBot = Instantiate(networkBot, botSpawnPoint.position, botSpawnPoint.rotation) as GameObject;
        ScheduleNextEnemySpawn();
    }

    void ScheduleNextEnemySpawn()
    {
        //float spawnInSeconds;

        //if (maxSpawnRateInSeconds > 1f)
        //{
        //    spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        //}

        //else
        //{
        //    spawnInSeconds = 1f;
        //}

        Invoke("SpawnBot", maxSpawnRateInSeconds);
        maxSpawnRateInSeconds = 10f;
    }

    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSeconds > 1f)
            maxSpawnRateInSeconds--;
        if (maxSpawnRateInSeconds == 1f)
            CancelInvoke("IncreaseSpawnRate");
    }

    //Funtion to stop enemy spawner
    public void UnscheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
        CancelInvoke("ScheduleEnemySpawner");
    }
}
