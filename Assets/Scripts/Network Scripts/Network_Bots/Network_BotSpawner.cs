using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network_BotSpawner : NetworkBehaviour {

    [SerializeField]
    GameObject networkBot_noName;

	[SerializeField]
	GameObject networkBot_Sparkus;

	[SerializeField]
	GameObject networkBot_D1;

    [SerializeField]
    GameObject[] botSpawnPoints;

    [SerializeField]
    int numberToSpawn = 1;
    int botSpawnerindex;

    [SerializeField]
    float maxSpawnRateInSeconds = 10f;

	int randomizer = 0;

    GameObject currentBotSpawnPoint;
    GameObject spawnedBot;

	List <string> names = new List<string> ();

    void Start()
    {
		AddNames ();
		Network_Manager netManager = GameObject.FindGameObjectWithTag ("NetManager").GetComponent<Network_Manager> ();
		numberToSpawn = netManager.noOfCPUs;
		SpawnBot();
    }

	void AddNames() {
		names.Add ("CPU-Joe");
		names.Add ("CPU-Casey");
		names.Add ("CPU-Katie");
		names.Add ("CPU-Frank");
		names.Add ("CPU-Fred");
		names.Add ("CPU-Lucy");
	}

//    public void ScheduleNextEnemySpawn()
//    {
//        float spawnInSeconds;
//
//        if (maxSpawnRateInSeconds > 1f)
//        {
//            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
//        }
//
//        else
//        {
//            spawnInSeconds = 1f;
//        }
//
//        //numberToSpawn++;
//
//        Invoke("SpawnBot", maxSpawnRateInSeconds);
//    }

    void SpawnBot()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            botSpawnerindex = Random.Range(0, botSpawnPoints.Length);
			randomizer = Random.Range (1, 4);
			if (randomizer == 1) {
				spawnedBot = Instantiate (networkBot_noName, botSpawnPoints [botSpawnerindex].transform.position, botSpawnPoints [botSpawnerindex].transform.rotation) as GameObject;
			}
			if (randomizer == 2) {
				spawnedBot = Instantiate (networkBot_Sparkus, botSpawnPoints [botSpawnerindex].transform.position, botSpawnPoints [botSpawnerindex].transform.rotation) as GameObject;
			}
			if (randomizer == 3) {
				spawnedBot = Instantiate (networkBot_D1, botSpawnPoints [botSpawnerindex].transform.position, botSpawnPoints [botSpawnerindex].transform.rotation) as GameObject;
			}
			NetworkServer.Spawn(spawnedBot.gameObject);
			int tempInt = Random.Range (0, names.Count);
			spawnedBot.gameObject.GetComponent<Network_Bot>().username = names[tempInt];
			spawnedBot.gameObject.name = names [tempInt];
			names.Remove(names[tempInt]);
			Network_GameManager.RegisterBot(spawnedBot.gameObject.transform.name, spawnedBot.gameObject.GetComponent<Network_Bot>());

        }
    }

//    void IncreaseSpawnRate()
//    {
//        if (maxSpawnRateInSeconds > 1f)
//            maxSpawnRateInSeconds--;
//        if (maxSpawnRateInSeconds == 1f)
//            CancelInvoke("IncreaseSpawnRate");
//    }
//
//    //Funtion to stop enemy spawner
//    public void UnscheduleEnemySpawner()
//    {
//        CancelInvoke("SpawnEnemy");
//        CancelInvoke("IncreaseSpawnRate");
//        CancelInvoke("ScheduleEnemySpawner");
//    }
}
