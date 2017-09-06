using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour {

    public static bool IsOn = false;
    string Reset = "Reset";

    private NetworkManager networkManager;
    private NetworkDiscovery networkDiscovery;

    void Start()
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
            networkDiscovery.StopBroadcast();
            networkDiscovery.SendMessage("DestorySelf", true);
            NetworkTransport.Shutdown();
            NetworkTransport.Init();
            SceneManager.LoadScene("JoinLAN_Scene");
            Debug.Log("Leaving");
        }
    }
}
