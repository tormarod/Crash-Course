using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private InterstitialAd interstitial;

    public static AdManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    public void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
            this.interstitial.Destroy();

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Inerstitial Ad is not ready yet");
        }
    }
}