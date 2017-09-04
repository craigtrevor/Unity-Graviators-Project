using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Network_SceneManager : MonoBehaviour
{
    public static Network_SceneManager instance;

    [SerializeField]
    AudioSource musicSource;

    bool isPlaying;

    private Scene currentScene;
    int sceneChange;

    public string sceneName;
    public string serverScene;

    void Awake()
    {
        isPlaying = true;

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
        PlaySong();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Create a reference to the current scene.
        currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        sceneName = currentScene.name;

        if (sceneName == "JoinLAN_Scene")
        {
            serverScene = currentScene.name;
        }

        else if (sceneName == "Lobby_Scene")
        {
            serverScene = currentScene.name;
        }
    }

    void PlaySong()
    {
        if (sceneName == "Online_Scene" && isPlaying == true)
        {
            musicSource.Pause();
            isPlaying = false;
        }

        else if (sceneName == "Lobby_Scene" && !isPlaying)
        {
            musicSource.UnPause();
            isPlaying = true;
        }

        else if (sceneName == "Main_Menu" && !isPlaying)
        {
            musicSource.UnPause();
            isPlaying = true;
        }
    }
}