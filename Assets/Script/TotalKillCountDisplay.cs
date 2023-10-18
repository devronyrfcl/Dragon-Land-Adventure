using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class TotalKillCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI killCountText; // Reference to the TextMeshProUGUI component for displaying the kill count

    void Start()
    {
        // Load the kill count from PlayerPrefs
        int killCount = PlayerPrefs.GetInt("KillCount", 0);

        // Update the TextMeshProUGUI with the kill count
        if (killCountText != null)
        {
            killCountText.text = "Kills: " + killCount.ToString();
        }
    }
}
