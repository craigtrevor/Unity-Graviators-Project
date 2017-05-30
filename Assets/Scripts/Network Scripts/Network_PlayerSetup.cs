using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Network_PlayerStats))]
public class Network_PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    GameObject[] gameobjectsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;
    public NetworkAnimator netAnim;


    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }

        else
        {
            sceneCamera = Camera.main;

            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    //void RegisterPlayer()
    //{
    //    string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
    //    transform.name = _ID;
    //}

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }

        for (int i = 0; i < gameobjectsToDisable.Length; i++)
        {
            gameobjectsToDisable[i].SetActive(false);
        }
    }

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        Network_GameManager.UnRegisterPlayer(transform.name);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Network_PlayerStats _player = GetComponent<Network_PlayerStats>();

        Network_GameManager.RegisterPlayer(_netID, _player);
    }

    public override void PreStartClient()
    {
        for (int i = 0; i < 5; i++)
        {
            netAnim.SetParameterAutoSend(i, true);
        }
    }

    public override void OnStartLocalPlayer()
    {
        for (int i = 0; i < 5; i++)
        {
            netAnim.SetParameterAutoSend(i, true);
        }
    }
}
