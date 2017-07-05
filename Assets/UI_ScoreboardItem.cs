﻿using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreboardItem : MonoBehaviour
{
    [SerializeField]
    Text usernameText;

    [SerializeField]
    Text killsText;

    [SerializeField]
    Text deathsText;

    public void Setup (string username, int kills, int deaths)
    {
        usernameText.text = username;
        killsText.text = "Kills: " + kills;
        deathsText.text = "Deaths: " + deaths;
    }
}


