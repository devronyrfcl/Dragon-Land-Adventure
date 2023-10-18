using UnityEngine;

public class KillCountManager : MonoBehaviour
{
    private static KillCountManager instance; // Singleton instance
    private int currentSeasonKillCount = 0; // The current season's kill count.
    private int totalKillCount = 0; // The total kill count across all seasons.

    public static KillCountManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KillCountManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("KillCountManager");
                    instance = singletonObject.AddComponent<KillCountManager>();
                }
            }
            return instance;
        }
    }

    // This method should be called whenever an enemy is killed to update the kill count.
    public void EnemyKilled()
    {
        currentSeasonKillCount++;
        totalKillCount++;
        SaveKillCount();
    }

    // You can call this method to get the current season's kill count from other scripts.
    public int GetCurrentSeasonKillCount()
    {
        return currentSeasonKillCount;
    }

    // You can call this method to get the total kill count from other scripts.
    public int GetTotalKillCount()
    {
        return totalKillCount;
    }

    // You can call this method to reset the current season's kill count.
    public void ResetCurrentSeasonKillCount()
    {
        currentSeasonKillCount = 0;
        SaveKillCount();
    }

    // You can call this method to reset the total kill count.
    public void ResetTotalKillCount()
    {
        totalKillCount = 0;
        SaveKillCount();
    }

    // This method saves the total kill count using PlayerPrefs.
    private void SaveKillCount()
    {
        PlayerPrefs.SetInt("KillCount", totalKillCount);
        PlayerPrefs.Save();
    }

    // This method loads the total kill count from PlayerPrefs.
    private void LoadKillCount()
    {
        totalKillCount = PlayerPrefs.GetInt("KillCount", 0);
    }

    // Awake is called before Start and ensures there is only one instance of the script.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadKillCount();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update.
    private void Start()
    {
        LoadKillCount();
        currentSeasonKillCount = 0; // Reset current season's kill count to 0 on start
    }
}
