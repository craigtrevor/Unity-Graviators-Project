using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Network_GameManager : MonoBehaviour {

    public static Network_GameManager instance;

    public Network_MatchSettings networkMatchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

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

    #region Trap Tracking
    private const string TRAP_ID_PREFIX = "Trap ";
    private static Dictionary<string, Network_TrapManager> traps = new Dictionary<string, Network_TrapManager>();

    public static void RegisterTrap(string _netID, Network_TrapManager _trap)
    {
        string _trapID = TRAP_ID_PREFIX + _netID;
        traps.Add(_trapID, _trap);
        _trap.transform.name = _trapID;
    }

    public static void UnRegisterTrap(string _trapID)
    {
        traps.Remove(_trapID);
    }

    public static Network_TrapManager GetTrap(string _trapID)
    {
        return traps[_trapID];
    }

    public static Network_TrapManager[] GetAllTraps()
    {
        return traps.Values.ToArray();
    }

    #endregion

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


	#region Bot tracking

	private const string Bot_ID_PREFIX = "Bot ";

	private static Dictionary<string, Network_Bot> bots = new Dictionary<string, Network_Bot>();


	public static void RegisterBot (string _netID, Network_Bot _bot)
	{
		string _botID = PLAYER_ID_PREFIX + _netID;
		bots.Add(_botID, _bot);
		_bot.transform.name = _botID;
	}

	public static void UnRegisterBot(string _botID)
	{
		players.Remove(_botID);
	}

	public static Network_Bot GetBot (string _botID)
	{
		return bots[_botID];
	}

	public static Network_Bot[] GetAllBots()
	{
		return bots.Values.ToArray();
	}

	public static void KillBot (string _botID)
	{
		bots [_botID].health = 100;
		bots[_botID].transform.root.gameObject.SetActive(false);
		instance.StartCoroutine(instance.RespawnBot(_botID));
	}

	IEnumerator RespawnBot (string _botID) {
		yield return new WaitForSeconds(6f);
		bots[_botID].transform.root.gameObject.SetActive(true);
	}

	#endregion


	#region Aid Tracking

	private const string AID_ID_PREFIX = "Aid ";

	private static Dictionary<string, Network_AidManager> generators = new Dictionary<string, Network_AidManager>();

	public static void RegisterGenerator(string _netID, Network_AidManager _aid)
	{
		string _aidID = AID_ID_PREFIX + _netID;
		generators.Add(_aidID, _aid);
		_aid.transform.name = _aidID;
	}

	public static void UnRegisterGenerator(string _aidID)
	{
		generators.Remove(_aidID);
	}

	public static Network_AidManager GetGenerator(string _aidID)
	{
		return generators[_aidID];
	}

	public static Network_AidManager[] GetAllGenerators()
	{
		return generators.Values.ToArray();
	}
		#endregion
	}