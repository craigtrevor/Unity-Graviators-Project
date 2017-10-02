using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class Network_MatchEnd : NetworkBehaviour
{

    [Header("Network Components")]

    private NetworkManager networkManager;
    private NetworkDiscovery networkDiscovery;

    public bool hasMatchEnded;
    public bool hasWonMatch;

    [SerializeField]
    Network_PlayerManager[] players;

    [SerializeField]
    Network_Bot[] bots;

    [Header("End Match Components")]

    public int matchCount = 10;

    [SerializeField]
    GameObject endMatchCanvas;

    [SerializeField]
    Text endMatchText;

    [SerializeField]
    Text endMatchCountdownText;
    int endMatchCountdown = 10;

    void Awake()
    {
        networkManager = NetworkManager.singleton;
        SetNetworkDiscovery();
    }

    void SetNetworkDiscovery()
    {
        if (GameObject.Find("NetClient") != null)
        {
            networkDiscovery = GameObject.Find("NetClient").GetComponent<NetworkDiscovery>();
        }

        else if (GameObject.Find("NetServer") != null)
        {
            networkDiscovery = GameObject.Find("NetServer").GetComponent<NetworkDiscovery>();
        }
    }

    void Update()
    {
        CheckKillStats();
    }

    void CheckKillStats()
    {
        players = FindObjectsOfType(typeof(Network_PlayerManager)) as Network_PlayerManager[];
        bots = FindObjectsOfType(typeof(Network_Bot)) as Network_Bot[];

        if (players != null)
        {
            foreach (Network_PlayerManager playerManager in players)
            {
                if (playerManager.killStats == matchCount && !hasMatchEnded)
                {
                    CmdEndingMatch();
                }
            }
        }

        if (bots != null)
        {
            foreach (Network_Bot networkBots in bots)
            {
                if (networkBots.killStats == matchCount && !hasMatchEnded)
                {
                    CmdEndingMatch();
                }
            }
        }
    }

    [Command]
    public void CmdEndingMatch()
    {
        Debug.Log("Match has finished");

        RpcEndMatch();
    }

    [ClientRpc]
    public void RpcEndMatch()
    {
        hasMatchEnded = true;
        //hasWonMatch = true;
        endMatchCanvas.SetActive(true);

        endMatchText.text = "MATCH OVER!";
        endMatchText.color = Color.white;

        foreach (Network_PlayerManager playerManager in players)
        {
            playerManager.StopAllCoroutines();
            playerManager.DisablePlayerOnMatchEnd();
        }

        //if (hasWonMatch)
        //{
        //    endMatchText.text = "Match Over!";
        //    endMatchText.color = Color.white;
        //}

        //else if (!hasWonMatch)
        //{
        //    endMatchText.text = "DEFEAT";
        //    endMatchText.color = Color.red;
        //}

        StartCoroutine(ShutdownServer());
    }

    IEnumerator ShutdownServer()
    {
        endMatchCountdownText.text = endMatchCountdown.ToString();
        InvokeRepeating("ServerCountdown", 1, 1);
        yield return new WaitForSeconds(10);
        LeaveRoom();
    }

    void ServerCountdown()
    {
        endMatchCountdown -= 1;
        endMatchCountdownText.text = endMatchCountdown.ToString();
    }

    public void LeaveRoom()
    {
        //networkManager.matchMaker

        if (Network_SceneManager.instance.serverScene == "Lobby_Scene")
        {
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopClient();
            SceneManager.LoadScene("Lobby_Scene");
            Debug.Log("Leaving");
        }

        else if (Network_SceneManager.instance.serverScene == "JoinLAN_Scene")
        {
            networkManager.StopHost();

            if (networkDiscovery != null)
            {
                networkDiscovery.StopBroadcast();
                networkDiscovery.SendMessage("DestorySelf", true);
            }

            NetworkTransport.Shutdown();
            NetworkTransport.Init();
            SceneManager.LoadScene("JoinLAN_Scene");
            Debug.Log("Leaving");
        }

        else if (Network_SceneManager.instance.serverScene == "JoinPracticeRange_Scene")
        {
            networkManager.StopHost();

            if (networkDiscovery != null)
            {
                networkDiscovery.StopBroadcast();
                networkDiscovery.SendMessage("DestorySelf", true);
            }

            NetworkTransport.Shutdown();
            NetworkTransport.Init();
            SceneManager.LoadScene("JoinPracticeRange_Scene");
            Debug.Log("Leaving");
        }
    }
}

