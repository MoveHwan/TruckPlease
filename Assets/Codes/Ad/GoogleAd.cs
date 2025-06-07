using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAd : MonoBehaviour
{
    public static GoogleAd instance;
    public GameObject ADBack;

    public bool buyAdDel;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ±¤°íÁ¦°Å ±¸¸ÅÇßÀ» ½Ã
        if(PlayerPrefs.GetInt("RemoveAd") == 1)
        {
            buyAdDel = true;
        }
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");

            if (!buyAdDel)
            {
                LoadInterstitialAd();
                LoadRewardedAd();
                LoadRewardedAdHeart();
                LoadRewardedAdCoin();
                LoadBannerAd();
            }
        });
    }


    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string interAdUnitId = "ca-app-pub-1025011428285645/8575153655";
    private string bannerAdUnitId = "ca-app-pub-1025011428285645/3122123790";
    private string rewardAdItemUnitId = "ca-app-pub-1025011428285645/8661949414";
    private string rewardAdHeartUnitId = "ca-app-pub-1025011428285645/7348867747";
    private string rewardAdCoinUnitId = "ca-app-pub-1025011428285645/9776554687";

#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;
    private RewardedAd rewardedAdItem;
    private RewardedAd rewardedAdHeart;
    private RewardedAd rewardedAdCoin;
    private BannerView bannerView;

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(interAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (!buyAdDel)
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                ADBackOn();

                Debug.Log("Showing interstitial ad.");
                _interstitialAd.Show();

                RegisterReloadHandler(_interstitialAd);
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAdItem != null)
        {
            rewardedAdItem.Destroy();
            rewardedAdItem = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardAdItemUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAdItem = ad;
            });
    }

    public void ShowRewardedAd()
    {
        //const string rewardMsg =
        //"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAdItem != null && rewardedAdItem.CanShowAd())
        {
            ADBackOn();

            rewardedAdItem.Show((Reward reward) =>
            {
                PlayerPrefs.SetInt("isItemReward", 1);
                BuyItem_InGame.instance.AD_Gift();
            });

        }
        else
        {
        }
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    public void LoadBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest adRequest = new AdRequest();

        bannerView.LoadAd(adRequest);

        Debug.Log("Banner ad loaded.");
    }

    public void ShowBanner()
    {
        bannerView?.Show();
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }

    public void LoadRewardedAdHeart()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAdHeart != null)
        {
            rewardedAdHeart.Destroy();
            rewardedAdHeart = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardAdHeartUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAdHeart = ad;
            });
    }

    public void ShowRewardedAdHeart()
    {
        //const string rewardMsg =
        //"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAdHeart != null && rewardedAdHeart.CanShowAd())
        {
            ADBackOn();

            rewardedAdHeart.Show((Reward reward) =>
            {
                // ¿©±â¼­ º¸»óÁà¾ßµÊ
            });

        }
        else
        {
        }
    }

    private void RegisterReloadHandlerHeart(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAdHeart();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAdHeart();
        };
    }

    public void LoadRewardedAdCoin()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAdCoin != null)
        {
            rewardedAdCoin.Destroy();
            rewardedAdCoin = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardAdCoinUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAdCoin = ad;
            });
    }

    public void ShowRewardedAdCoin()
    {

        //const string rewardMsg =
        //"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAdCoin != null && rewardedAdCoin.CanShowAd())
        {
            ADBackOn();

            rewardedAdCoin.Show((Reward reward) =>
            {
                PlayerPrefs.SetInt("isCoinReward", 1);
                StageTruckCanvas.Instance.AdReward();
            });

        }
        else
        {
        }
    }

    private void RegisterReloadHandlerCoin(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAdCoin();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAdCoin();
        };
    }


    // ±¤°í µÞÆÇ ¶ç¿ì±â
    void ADBackOn()
    {
        ADBack.SetActive(true);
        StartCoroutine(ADBackOffCoroutine());
    }
    IEnumerator ADBackOffCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        ADBack.SetActive(false);

        yield break;
    }
}