using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // Singleton for global access
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);  // Keeps AudioManager alive across scenes
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}