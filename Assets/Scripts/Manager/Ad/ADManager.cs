using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class ADManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private InterstitialAd interstitial;
    private bool isRemovedAd = false;


    public void Awake()
    {
        MobileAds.Initialize(initStatus => { });
        
        RequestInterstitial();
        RequestRewards();

        IsometricManager.Instance.ShowRewardAD.AddListener(ShowReward);
        IsometricManager.Instance.ShowinterstitialAD.AddListener(ShowInterstitial);
    }

    public void RemoveAd()
    {
        PlayerPrefs.SetInt("isRemovedAd", 1);
    }

    public void Failed()
    {
        Debug.LogWarning("Faild to buy Removeads");
    }


    #region 전면광고

    public void ShowInterstitial()
    {
        int a = UnityEngine.Random.Range(0, 101);


        if (interstitial.IsLoaded() && a < 20 && PlayerPrefs.GetInt("isRemovedAd") == 0)
        {
            interstitial.Show();
            RequestInterstitial();
        }

        print(a);
    }


    public void RequestInterstitial()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3131514107827460/4392424279";
#elif UNITY_IPHONE
            string adUnitId = "unexpected_platform";
#else
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received");
        
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        print("HandleAdOpening event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    #endregion


    #region 리워드광고

    public void ShowReward()
    {
        if (rewardedAd.IsLoaded() && PlayerPrefs.GetInt("isRemovedAd") == 0)
        {
            rewardedAd.Show();
            RequestRewards();
        }
        else
        {
            IsometricManager.Instance.NoAdsEvent.Invoke();
        }
    }

    private void RequestRewards()
    {
        print("리워드 광고 요청");

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3131514107827460/3401504714";
#elif UNITY_IPHONE
            string adUnitId = "unexpected_platform";
#else
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif

        rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print(
             "HandleRewardedAdFailedToLoad event received with message: "
                              + args.LoadAdError.GetMessage());
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.AdError.GetMessage());
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

      

        if (type.ToLower().Contains("heart"))
        {
            IsometricManager.Instance.AddHearts.Invoke((int)amount);
        }
        else
        {
            print("type error :" + type);
        }
    }


    #endregion

}