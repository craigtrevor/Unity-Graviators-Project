using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scoreboard : MonoBehaviour {

    [SerializeField]
    GameObject playerScoreboardItem;

    [SerializeField]
    Transform playerScoreboardList;

    void OnEnable()
    {
        Network_PlayerManager[] players = Network_GameManager.GetAllPlayers();

        foreach (Network_PlayerManager player in players)
        {
            GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
            UI_ScoreboardItem item = itemGO.GetComponent<UI_ScoreboardItem>();

            if (item != null)
            {
                item.Setup(player.username, player.killStats, player.deathStats);
            }
        }

		Network_Bot[] bots = Network_GameManager.GetAllBots();

		foreach (Network_Bot bot in bots)
		{
			GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
			UI_ScoreboardItem item = itemGO.GetComponent<UI_ScoreboardItem>();

			if (item != null)
			{
				item.Setup(bot.username, bot.killStats, bot.deathStats);
			}
		}
    }

    void OnDisable()
    {
        foreach (Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }
}
