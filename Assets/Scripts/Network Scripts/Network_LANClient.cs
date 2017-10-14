using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Network_LANClient : NetworkDiscovery
{
    Text statusText;

    public void startClient()
    {
        Initialize();
        StartAsClient();
        StatusUpdate();
    }

    private void StatusUpdate()
    {
        statusText = GameObject.Find("StatusText").GetComponent<Text>();
        statusText.color = Color.green;
        statusText.text = "Joining server...";
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();
        Debug.Log("Player joined local server");
    }
}
