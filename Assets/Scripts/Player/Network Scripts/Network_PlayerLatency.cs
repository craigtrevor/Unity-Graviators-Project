using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Network_PlayerLatency : NetworkBehaviour {

    NetworkClient nClient;
    private int latency;
    private Text latencyText;

    public override void OnStartLocalPlayer()
    {
        nClient = GameObject.Find("CustomNetworkManager").GetComponent<NetworkManager>().client;
        latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
    }

    void Update()
    {
        ShowLatency();
    }

    void ShowLatency()
    {
        if (isLocalPlayer)
        {
            latency = nClient.GetRTT();
            latencyText.text = latency.ToString();
        }
    }
}
