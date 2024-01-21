using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardedAds : MonoBehaviour
{
    public GameObject AdLoadedStatus;
    public string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Replace with your rewarded video ad unit id

    private RewardedAd rewardedAd;

    // Event to be triggered when the rewarded ad is closed
    public Action OnAdsClosed;

    // Event to be triggered when the player earns a reward
    public Action<int> OnEarnedReward; // Assuming the reward is an integer, modify the type accordingly

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadAd();
    }

    public void LoadAd()
    {
        if (rewardedAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading rewarded ad.");

        var adRequest = new AdRequest.Builder().Build();

        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error: " + error);
                return;
            }

            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            Debug.Log("Rewarded ad loaded with response: " + ad.GetResponseInfo());
            rewardedAd = ad;

            RegisterEventHandlers(ad);

            AdLoadedStatus?.SetActive(true);
        });
    }

    public void ShowAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            rewardedAd.Show((Reward reward) =>
            {
                int rewardAmount = Convert.ToInt32(reward.Amount); // Convert reward amount to an integer
                Debug.Log($"Rewarded ad granted a reward: {rewardAmount} {reward.Type}");

                // Trigger the OnEarnedReward event
                OnEarnedReward?.Invoke(rewardAmount);
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }

        AdLoadedStatus?.SetActive(false);
    }

    public void DestroyAd()
    {
        if (rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdLoadedStatus?.SetActive(false);
    }

    public void LogResponseInfo()
    {
        if (rewardedAd != null)
        {
            var responseInfo = rewardedAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}.");
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");

            // Trigger the OnAdsClosed event when the rewarded ad is closed
            OnAdsClosed?.Invoke();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error: " + error);
        };
    }
}
