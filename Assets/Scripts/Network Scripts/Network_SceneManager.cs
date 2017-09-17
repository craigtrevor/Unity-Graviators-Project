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
    public bool wonMatch;
    public string playerUsername;

    void Awake()
    {
        isPlaying = true;
        wonMatch = false;

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
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

            if (!isPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(Soundscape_AudioFade.FadeIn(musicSource, 2f));
                isPlaying = true;
            }
        }

        else if (sceneName == "Lobby_Scene")
        {
            serverScene = currentScene.name;

            if (!isPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(Soundscape_AudioFade.FadeIn(musicSource, 2f));
                isPlaying = true;
            }
        }

        if (sceneName == "Main_Menu")
        {
            if (!isPlaying)
            {
                StopAllCoroutines();
                StartCoroutine(Soundscape_AudioFade.FadeIn(musicSource, 2f));
                isPlaying = true;
            }
        }


        if (sceneName != "Online_Scene_ArenaV2" || sceneName != "Tutorial_Arena")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (sceneName == "Online_Scene_ArenaV2" || sceneName == "Tutorial_Arena")
        {
            StopAllCoroutines();
            StartCoroutine(Soundscape_AudioFade.FadeOut(musicSource, 2f));
            isPlaying = false;        
        }
    }
}