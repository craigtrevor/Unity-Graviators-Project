﻿using UnityEngine;
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

        if (sceneName != "Online_Scene" || sceneName != "Tutorial_Arena")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void PlaySong()
    {
        if (sceneName == "Online_Scene_ArenaV2" && isPlaying == true)
        {
            musicSource.Pause();
            isPlaying = false;
            Debug.Log("Pause");
        }

        else if (sceneName != "Online_Scene_ArenaV2" && !isPlaying)
        {
            musicSource.UnPause();
            Debug.Log("Unpause");
            isPlaying = true;
        }
    }
}