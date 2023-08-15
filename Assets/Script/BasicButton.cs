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
}
