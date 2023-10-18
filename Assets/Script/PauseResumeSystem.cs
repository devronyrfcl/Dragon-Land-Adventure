using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseResumeSystem : MonoBehaviour
{
    private bool isPaused = false;

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            // Add pause-related logic here.
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            // Add resume-related logic here.
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Ensure time scale is reset before loading the main menu.
        SceneManager.LoadScene("MainMenu"); // Replace with your actual main menu scene name.
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure time scale is reset before restarting.
        // Reload the current scene to restart the game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
