using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Network_HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 8;

    [SerializeField]
    Text statusText;

    private string roomName;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        if (roomName != "" && roomName != null)
        {
            //Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
            Debug.Log("Player created internet match");

            statusText.color = Color.green;
            statusText.text = "Creating server...";

            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }

        else
        {
            statusText.color = Color.red;
            statusText.text = "Please enter a name for your server...";
        }
    }
}
