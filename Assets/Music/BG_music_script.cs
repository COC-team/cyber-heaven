using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_music_script : MonoBehaviour
{
    // Static instance to ensure only one instance of this script exists
    private static BG_music_script instance;

    void Awake() 
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If no instance exists, set this as the instance and make it persist between scenes
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading new scenes

            // Optionally: start playing the background music if you have an AudioSource attached
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // Play the background music if not already playing
            }
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
}
