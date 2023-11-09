using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAds : MonoBehaviour
{
    public string adUnitId = "ca-app-pub-2542236422731393/2619611280";
    private BannerView bannerView;

    void Start()
    {
        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize(initStatus => { });

        // Request a banner ad
        RequestBannerAd();
    }

    private void RequestBannerAd()
    {
        // Create a banner view
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request
        bannerView.LoadAd(request);

        // Show the banner
        bannerView.Show();
    }

    void OnDestroy()
    {
        // Destroy the banner when the script is destroyed
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
