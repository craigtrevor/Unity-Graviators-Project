using UnityEngine;
using System.Collections.Generic;

public class Network_GameManager : MonoBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Network_PlayerStats> players = new Dictionary<string, Network_PlayerStats>();

    public static void RegisterPlayer (string _netID, Network_PlayerStats _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Network_PlayerStats GetPlayer (string _playerID)
    {
        return players[_playerID];
    }

    void OnGUI()
    {
        //GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        //GUILayout.BeginVertical();

        //foreach (string _playerID in players.Keys)
        //{
        //    GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        //}

        //GUILayout.EndVertical();
        //GUILayout.EndArea();
    }

}
