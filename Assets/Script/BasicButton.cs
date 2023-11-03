using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicButton : MonoBehaviour
{
    public string mainLevelSceneName = "MainLevel"; // Name of the MainLevel scene

    public void OnButtonClicked()
    {
        // Load the MainLevel scene
        SceneManager.LoadScene(mainLevelSceneName);
    }

    public void ExitGame()
    {
        // Quit the application (only works in standalone builds, not in the Unity editor)
        Application.Quit();
    }
}
