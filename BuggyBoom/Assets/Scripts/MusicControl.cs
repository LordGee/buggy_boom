using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicControl : MonoBehaviour {

    public AudioClip[] musicSelection;
    public new AudioSource audio;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // audio = GetComponent<AudioSource>();
        // audioSource.volume = PlayerPrefsManager.GetMasterVolume();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        AudioClip thisSceneMusic = musicSelection[scene.buildIndex];
        if (thisSceneMusic)
        {
            audio.clip = thisSceneMusic;
            audio.loop = true;
            audio.Play();
        }
    }

    public void ChangeVolume(float newVolume)
    {
        audio.volume = newVolume;
    }
}
