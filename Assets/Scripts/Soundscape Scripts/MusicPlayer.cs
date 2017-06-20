using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioMusic;

    private static MusicPlayer _instance;

    void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);

        audioMusic = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {   
            if (audioMusic.isPlaying)

                audioMusic.Stop();
            else
                audioMusic.Play();
        }
    }
}