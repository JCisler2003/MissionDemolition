using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager Instance;

    // Reference to the Game Over UI panel
    public GameObject gameOverPanel;

    // Reference to the "Play Again" button
    public Button playAgainButton;

    // Failure counter and limit
    private int failureCount = 0;
    public int failureLimit = 3; // Set the failure limit to 3

    void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one GameManager exists
        }
    }

    void Start()
    {
        // Hide the Game Over panel at the start of the game
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Add a listener to the "Play Again" button
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(PlayAgain);
        }
    }

    // Call this method when the player fails
    public void RegisterFailure()
    {
        failureCount++; // Increment the failure counter

        // Check if the failure limit has been reached
        if (failureCount >= 3)
        {
            GameOver();
        }
    }

    // Call this method when the game is over
    public void GameOver()
    {
        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Optional: Pause the game
        Time.timeScale = 0;
    }

    // Call this method when the "Play Again" button is clicked
    void PlayAgain()
    {
        // Reset the failure counter
        failureCount = 0;

        // Resume the game (if it was paused)
        Time.timeScale = 1;

        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}