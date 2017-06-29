﻿using UnityEngine;
using System;

public class Network_DataTranslator : MonoBehaviour {

    private static string KILLS_SYMBOL = "[KILLS]";
    private static string DEATHS_SYMBOL = "[DEATHS]";

    public static string ValuesToData (int killStats, int deathStats)
    {
        return KILLS_SYMBOL + killStats + "/" + DEATHS_SYMBOL + deathStats;
    }

    public static int DataToKills (string data)
    {
        return int.Parse(DataToValue(data, KILLS_SYMBOL));
    }

    public static int DataToDeaths (string data)
    {
        return int.Parse(DataToValue(data, DEATHS_SYMBOL));
    }

    private static string DataToValue (string data, string symbol)
    {
        string[] pieces = data.Split('/');
        foreach (string piece in pieces)
        {
            if (piece.StartsWith(symbol))
            {
               return piece.Substring(symbol.Length);
            }
        }

        Debug.LogError(symbol + " not found in " + data);
        return "";
    }
}