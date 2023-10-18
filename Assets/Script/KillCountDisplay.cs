using UnityEngine;
using TMPro;

public class KillCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI killCountText; // Reference to the TextMeshPro Text component.

    // Update is called once per frame.
    private void Update()
    {
        // Check if the KillCountManager instance exists.
        if (KillCountManager.Instance != null)
        {
            // Get the current season's kill count from the KillCountManager.
            int currentSeasonKillCount = KillCountManager.Instance.GetCurrentSeasonKillCount();

            // Update the TextMeshPro text to display the current season's kill count.
            if (killCountText != null)
            {
                killCountText.text = currentSeasonKillCount.ToString();
            }
        }
    }
}
