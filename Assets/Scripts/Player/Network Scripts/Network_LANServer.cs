using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Network_LANServer : NetworkDiscovery
{
    Text statusText;

    void Start()
    {
        statusText = GameObject.Find("StatusText").GetComponent<Text>();
        Application.runInBackground = true;
    }

    // Call to create a server
    public void startServer()
    {
        int serverPort = createServer();

        if (serverPort != -1)
        {
            Debug.Log("Server created on port : " + serverPort);
            broadcastData = serverPort.ToString();
            Initialize();
            StartAsServer();
            NetworkManager.singleton.StartHost();
        }

        else
        {
            Debug.Log("Failed to create Server");
        }

    }

    int minPort = 1000;
    int maxPort = 10010;
    int defaultPort = 10000;

    // Creates a server then returns the port the server is created with. Returns -1 if server is not created
    private int createServer()
    {
        int serverPort = -1;

        // Connect to default port
        bool serverCreated = NetworkServer.Listen(defaultPort);

        if (serverCreated)
        {
            serverPort = defaultPort;

            statusText.color = Color.green;
            statusText.text = "Creating server...";
            Debug.Log("Server Created with default port");

        }

        else
        {
            Debug.Log("Failed to create with the default port");

            // Try to create servwer with other port from min to max except the default port which we tried already
            for (int temport = minPort; temport <= maxPort; temport++)
            {
                // Skip the default port since we already tried it

                if (temport != defaultPort)
                {
                    // Exit loop if successfully create a server

                    if (NetworkServer.Listen(temport))
                    {
                        serverPort = temport;
                        break;
                    }

                    // If this is the max port and server is not still created, show, failed to create server error
                    if (temport == maxPort)
                    {
                        Debug.LogError("Failed to create server");

                        statusText.color = Color.red;
                        statusText.text = "Failed to create server...";
                    }
                }
            }
        }
        return serverPort;
    }

    void DestorySelf(bool isDestroy)
    {
        Destroy(this.gameObject);
    }
}
