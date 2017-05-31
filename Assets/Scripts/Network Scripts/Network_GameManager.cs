using UnityEngine;
using System.Collections.Generic;

public class Network_GameManager : MonoBehaviour {

    public static Network_GameManager instance;

    public Network_MatchSettings networkMatchSettings;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Network GameManager in scene.");
        } else
        {
            instance = this;
        }
    }

    #region Player tracking

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

    #endregion
}
