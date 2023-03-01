using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGoogleAdmob : MonoBehaviour
{
    private InterstitialAd interstitial;

    void Start(){
        MobileAds.Initialize(initStatus => { });
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        //단일 OS일 경우 여기서 바로 스트링으로 꽂아줘도 가능
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
