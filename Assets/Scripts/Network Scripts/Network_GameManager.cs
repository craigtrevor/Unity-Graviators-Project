using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Network_GameManager : MonoBehaviour {

    public static Network_GameManager instance;

    public Network_MatchSettings networkMatchSettings;

    [SerializeField]
    private GameObject sceneCamera;

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

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(isActive);
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Network_PlayerManager> players = new Dictionary<string, Network_PlayerManager>();

    public static void RegisterPlayer (string _netID, Network_PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Network_PlayerManager GetPlayer (string _playerID)
    {
        return players[_playerID];
    }

    public static Network_PlayerManager[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    #endregion
}
