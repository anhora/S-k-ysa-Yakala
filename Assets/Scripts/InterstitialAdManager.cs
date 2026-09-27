
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;

#if UNITY_IOS
    private const string InterstitialAdUnitId =
        "ca-app-pub-3940256099942544/4411468910";
#elif UNITY_ANDROID
    private const string InterstitialAdUnitId =
        "ca-app-pub-3940256099942544/1033173712";
#else
    private const string InterstitialAdUnitId = "";
#endif

    private Action onAdClosed;

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            LoadInterstitialAd();
        });
    }

    void LoadInterstitialAd()
    {
        if (string.IsNullOrEmpty(InterstitialAdUnitId))
        {
            Debug.LogWarning("Bu platform için reklam ID'si tanımlı değil.");
            return;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(
            InterstitialAdUnitId,
            request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError(
                        "Interstitial reklam yüklenemedi: " + error
                    );

                    return;
                }

                interstitialAd = ad;

                Debug.Log(
                    "Interstitial reklam başarıyla yüklendi."
                );

                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log(
                        "Interstitial reklam kapatıldı."
                    );

                    interstitialAd.Destroy();
                    interstitialAd = null;

                    LoadInterstitialAd();

                    if (onAdClosed != null)
                    {
                        Action callback = onAdClosed;
                        onAdClosed = null;
                        callback.Invoke();
                    }
                };

                interstitialAd.OnAdFullScreenContentFailed +=
                    (AdError adError) =>
                    {
                        Debug.LogError(
                            "Interstitial reklam gösterilemedi: " +
                            adError
                        );

                        if (interstitialAd != null)
                        {
                            interstitialAd.Destroy();
                        }

                        interstitialAd = null;

                        LoadInterstitialAd();

                        if (onAdClosed != null)
                        {
                            Action callback = onAdClosed;
                            onAdClosed = null;
                            callback.Invoke();
                        }
                    };
            }
        );
    }

    public void ShowInterstitialAd(Action afterAdClosed)
    {
        onAdClosed = afterAdClosed;

        if (interstitialAd != null &&
            interstitialAd.CanShowAd())
        {
            Debug.Log(
                "Interstitial reklam gösteriliyor."
            );

            interstitialAd.Show();
        }
        else
        {
            Debug.Log(
                "Interstitial reklam henüz hazır değil."
            );

            onAdClosed = null;

            if (afterAdClosed != null)
            {
                afterAdClosed.Invoke();
            }

            LoadInterstitialAd();
        }
    }
}
