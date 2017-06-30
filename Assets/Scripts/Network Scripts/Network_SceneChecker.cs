using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Network_SceneChecker : MonoBehaviour
{
    public static Network_SceneChecker instance;

    private Scene currentScene;
    public string sceneName;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        // Create a reference to the current scene.
        currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        sceneName = currentScene.name;
    }
}