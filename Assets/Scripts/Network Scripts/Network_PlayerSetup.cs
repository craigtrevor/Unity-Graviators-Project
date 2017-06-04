using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Network_PlayerManager))]
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

        GetComponent<Network_PlayerManager>().SetupPlayer();
    }

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
        Network_PlayerManager _player = GetComponent<Network_PlayerManager>();

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
