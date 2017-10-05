using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour {

    public static bool IsOn = false;
    string Reset = "Reset";

    private NetworkManager networkManager;
    private NetworkDiscovery networkDiscovery;

    // Scripts
    Network_PlayerManager netPlayerManager;

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

        netPlayerManager = GetComponentInParent<Network_PlayerManager>();
    }

    public void LeaveRoom()
    {
        //networkManager.matchMaker

        if (Network_SceneManager.instance.serverScene == "Lobby_Scene")
        {
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.SendMessage("DestorySelf", true);
            networkManager.StopClient();
            SceneManager.LoadScene("Main_Menu");
            Debug.Log("Leaving");
        }

        else if (Network_SceneManager.instance.serverScene == "JoinLAN_Scene")
        {
            if (netPlayerManager.isPlayerServer)
            {
                networkManager.SendMessage("DestorySelf", true);
                networkManager.StopHost();
                networkDiscovery.StopBroadcast();
                networkDiscovery.SendMessage("DestorySelf", true);
                NetworkTransport.Shutdown();
                NetworkTransport.Init();
                SceneManager.LoadScene("Main_Menu");
                Debug.Log("Leaving");
            }
           
            else if (!netPlayerManager.isPlayerServer)
            {
                networkManager.SendMessage("DestorySelf", true);
                networkManager.StopClient();
                SceneManager.LoadScene("Main_Menu");
                Debug.Log("Leaving");
            }
        }

        else if (Network_SceneManager.instance.serverScene == "JoinPracticeRange_Scene")
        {
            networkManager.SendMessage("DestorySelf", true);
            networkManager.StopHost();
            networkDiscovery.SendMessage("DestorySelf", true);
            NetworkTransport.Shutdown();
            NetworkTransport.Init();
            SceneManager.LoadScene("Main_Menu");
            Debug.Log("Leaving");
        }
    }
}
