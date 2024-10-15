using UnityEngine;

public class BG_music_script : MonoBehaviour
{
    // Static instance to ensure only one instance of this script exists
    private static BG_music_script instance;

    // Audio clips for background music and boss fight music
    public AudioClip backgroundMusic;  // Assign this in the Inspector
    public AudioClip bossFightMusic;    // Assign this in the Inspector

    private AudioSource audioSource;

    void Awake() 
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If no instance exists, set this as the instance and make it persist between scenes
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading new scenes
            
            audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                // Start playing the background music
                audioSource.clip = backgroundMusic;
                audioSource.loop = true; // Loop the background music
                audioSource.Play();
            }
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    // Call this method to switch to the boss fight music
    public void StartBossFightMusic()
    {
        if (audioSource != null && bossFightMusic != null)
        {
            audioSource.Stop(); // Stop the current music
            audioSource.clip = bossFightMusic; // Change the audio clip
            audioSource.Play(); // Play the boss fight music
            Debug.Log("audioSource.clip.name");
        }
    }

    // Optional: Call this method to revert back to background music if needed
    public void RevertToBackgroundMusic()
    {
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.Stop();
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }
}