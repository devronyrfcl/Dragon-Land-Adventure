using UnityEngine;

public class PlayFabLeaderBoard : MonoBehaviour
{
    private void Start()
    {
        // Access the KillCountManager and get the total kill count
        int totalKillCount = KillCountManager.Instance.GetTotalKillCount();
        
        // Check if PlayFabManager exists
        PlayFabManager playFabManager = FindObjectOfType<PlayFabManager>();
        if (playFabManager != null)
        {
            // Send the total kill count to PlayFab leaderboard if PlayFabManager is found
            playFabManager.SendLeaderboard(totalKillCount);
        }
        else
        {
            Debug.LogError("PlayFabManager not found in the scene.");
        }

        // Use the total kill count as needed
        //Debug.Log("Total Kill Count: " + totalKillCount);
    }
}
