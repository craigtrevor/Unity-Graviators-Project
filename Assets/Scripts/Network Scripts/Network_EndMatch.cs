using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Network_EndMatch : MonoBehaviour {

    [SerializeField]
    GameObject winScreen;
    [SerializeField]
    Text winScreenUserName;

    [SerializeField]
    GameObject deathScreen;
    [SerializeField]
    Text deathScreenUserName;

    // Use this for initialization
    void Start()
    {
        if (Network_SceneManager.instance.wonMatch == true)
        {
            winScreen.SetActive(true);
            winScreenUserName.text = Network_SceneManager.instance.playerUsername;
        }

        else if (Network_SceneManager.instance.wonMatch == false)
        {
            deathScreen.SetActive(true);
            deathScreenUserName.text = Network_SceneManager.instance.playerUsername;
        }
    }
}
