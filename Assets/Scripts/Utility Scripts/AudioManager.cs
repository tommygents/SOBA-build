using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource[] audioSources;
    public AudioSource playerAudio;
    public AudioSource music;
    public AudioSource enemyAudio;
     

    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
        // Start is called before the first frame update
        void Start()
    {
        audioSources = GetComponents<AudioSource>();
        playerAudio = audioSources[0];
        music = audioSources[1];
        enemyAudio = audioSources[2];


    }



    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayerClip(AudioClip _clip)
    {
        if (playerAudio != null)
        {
            playerAudio.clip = _clip;
            playerAudio.Play();
        }
    }

    public void StopClip(AudioClip _clip)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (_clip == audioSource.clip)
                audioSource.Stop();
        }
    }

    public void PlaySoundAdHoc(AudioClip clip)
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.Play();
        Destroy(newSource, clip.length);  // Automatically destroy the AudioSource when done playing
    }

}
