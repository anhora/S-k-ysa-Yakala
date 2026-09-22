using UnityEngine;
using GoogleMobileAds.Api;

public class RewardedAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    // GameManager referansı
    public GameManager gameManager;

    // Google'ın TEST Rewarded reklam ID'si
    private const string AndroidTestAdUnitId =
        "ca-app-pub-3940256099942544/5224354917";

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            LoadRewardedAd();
        });
    }

    void LoadRewardedAd()
    {
        AdRequest request = new AdRequest();

        RewardedAd.Load(
            AndroidTestAdUnitId,
            request,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError(
                        "Rewarded reklam yüklenemedi: " + error
                    );
                    return;
                }

                rewardedAd = ad;

                Debug.Log(
                    "Rewarded reklam başarıyla yüklendi."
                );

                // Reklam kapatıldığında yeni reklam yükle
                rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    rewardedAd = null;
                    LoadRewardedAd();
                };

                // Reklam gösterilirken hata olursa
                rewardedAd.OnAdFullScreenContentFailed +=
                    (AdError adError) =>
                    {
                        Debug.LogError(
                            "Rewarded reklam gösterilemedi: " + adError
                        );

                        rewardedAd = null;
                        LoadRewardedAd();
                    };
            }
        );
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(
                    "REKLAM TAMAMLANDI! Ödül: " +
                    reward.Amount
                );

                // Reklam tamamen izlendi.
                // Oyuncuya 1 can ver ve oyuna devam ettir.
                if (gameManager != null)
                {
                    gameManager.ContinueAfterReward();
                }
                else
                {
                    Debug.LogError(
                        "GameManager referansı atanmadı!"
                    );
                }
            });
        }
        else
        {
            Debug.Log(
                "Rewarded reklam henüz hazır değil."
            );

            LoadRewardedAd();
        }
    }
}