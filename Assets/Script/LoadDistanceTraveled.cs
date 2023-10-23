using UnityEngine;

public class LoadDistanceTraveled : MonoBehaviour
{
    public float loadedDistanceTraveled;
    public int loadedDistanceTraveledInt;
    public PlayFabManager playFabManager; // Reference to the PlayFabManager script

    void Start()
    {
        SendValueToLB();
    }

    public void SendValueToLB()
    {
        loadedDistanceTraveled = PlayerPrefs.GetFloat("DistanceTraveled", 0f);
        loadedDistanceTraveledInt = Mathf.RoundToInt(loadedDistanceTraveled);

        // Find the PlayFabManager script in the scene
        playFabManager = FindObjectOfType<PlayFabManager>();

        // Check if the PlayFabManager script was found
        if (playFabManager != null)
        {
            playFabManager.SendLeaderboard(loadedDistanceTraveledInt);
            Debug.Log("Loaded DistanceTraveled (int): " + loadedDistanceTraveledInt);
        }
        else
        {
            Debug.LogError("PlayFabManager not found in the scene.");
        }
    }
}
