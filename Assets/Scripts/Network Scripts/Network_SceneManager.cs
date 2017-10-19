using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class Network_SceneManager : MonoBehaviour
{
    public static Network_SceneManager instance;
    public PostProcessingProfile ppProfile;

    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    AudioClip[] songs;

    bool isPlaying;

    private Scene currentScene;
    int sceneChange;

    public string sceneName;
    public string serverScene;
    public bool wonMatch;
    public string playerUsername;

    [SerializeField]
    int randomSong;

    [HideInInspector]
    public int narrationCount = 0;

    void Awake()
    {
        // Create a reference to the current scene.
        currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        sceneName = currentScene.name;

        isPlaying = true;
        wonMatch = false;

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (instance == null)
        {
            randomSong = Random.Range(0, 6);
            musicSource.clip = songs[randomSong];
            musicSource.Play();
        }

        instance = this;
        DontDestroyOnLoad(this);

        //copy current bloom settings from the profile into a temporary variable
        BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
        //change the intensity in the temporary settings variable
        bloomSettings.bloom.intensity = 0.1f;
        //set the bloom settings in the actual profile to the temp settings with the changed value
        ppProfile.bloom.settings = bloomSettings;
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

        else if (sceneName == "JoinOfflineMode_Scene")
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

        if (sceneName == "Title_Screen")
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

        if (sceneName == "Tutorial_Arena")
        {
            Debug.Log("Player joined tutorial");
        }
    }
}