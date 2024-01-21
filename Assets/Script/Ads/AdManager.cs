using UnityEngine;

public class AdManager : MonoBehaviour
{
    public RewardedAds rewardedAds;

    public float loadAdInterval = 5f; // Load ad every 5 seconds
    private float timeSinceLastLoad;

    void Start()
    {
        // Load rewarded ads
        if (rewardedAds != null)
        {
            rewardedAds.LoadAd();
        }
        else
        {
            Debug.LogError("RewardedAds script reference is missing!");
        }

        // Initialize the timer
        timeSinceLastLoad = loadAdInterval;
    }

    void Update()
    {
        // Continuously check and reload ads based on the timer
        timeSinceLastLoad -= Time.deltaTime;
        if (timeSinceLastLoad <= 0)
        {
            // Load rewarded ads
            if (rewardedAds != null)
            {
                rewardedAds.LoadAd();
            }

            // Reset the timer
            timeSinceLastLoad = loadAdInterval;
        }
    }
}
