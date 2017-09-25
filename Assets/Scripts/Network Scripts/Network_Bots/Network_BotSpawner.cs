using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_BotSpawner : MonoBehaviour {

    [SerializeField]
    GameObject networkBot;

    [SerializeField]
    GameObject[] botSpawnPoints;

    [SerializeField]
    int numberToSpawn = 1;
    int botSpawnerindex;

    [SerializeField]
    float maxSpawnRateInSeconds = 10f;

    GameObject currentBotSpawnPoint;
    GameObject spawnedBot;

    void Start()
    {
        if (Network_SceneManager.instance.serverScene == "JoinPracticeRange_Scene")
        {
            SpawnInitialBot();
        }
    }

    void SpawnInitialBot()
    {
        spawnedBot = Instantiate(networkBot, botSpawnPoints[botSpawnerindex].transform.position, botSpawnPoints[botSpawnerindex].transform.rotation) as GameObject;
		NetworkServer.Spawn(spawnedBot.gameObject);
    }

    public void ScheduleNextEnemySpawn()
    {
        float spawnInSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }

        else
        {
            spawnInSeconds = 1f;
        }

        //numberToSpawn++;

        Invoke("SpawnBot", maxSpawnRateInSeconds);
    }

    void SpawnBot()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            botSpawnerindex = Random.Range(0, botSpawnPoints.Length);
            spawnedBot = Instantiate(networkBot, botSpawnPoints[botSpawnerindex].transform.position, botSpawnPoints[botSpawnerindex].transform.rotation) as GameObject;
        }
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
