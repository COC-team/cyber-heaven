using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the game or changing scenes
using UnityEngine.UI; // To handle UI elements

public class Game_over : MonoBehaviour
{
    public GameObject gameOverScreen; // The Game Over UI canvas
    public Entity playerEntity; // Reference to the player entity
    public BG_music_script bgMusicScript;

    private bool gameOverShown = false; // Prevent showing Game Over multiple times

    void Start()
    {
        // Ensure the Game Over screen is initially hidden
        gameOverScreen.SetActive(false);
    }

    void Update()
    {
        // Check if the player is dead and the Game Over screen hasn't already been shown
        if (!playerEntity.isAlive && !gameOverShown)
        {
            ShowGameOverScreen();
        }
    }

    void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true); // Show the Game Over screen
        bgMusicScript.RevertToBackgroundMusic();
        // Time.timeScale = 0f; // Pause the game
        gameOverShown = true; // Mark that the Game Over screen has been shown
    }

    // Function for the "Restart" button in the UI
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("GameplayScene"); // Reload the current scene
    }

    // Function for the "Main Menu" button in the UI
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("Main menu 2");
    }
}