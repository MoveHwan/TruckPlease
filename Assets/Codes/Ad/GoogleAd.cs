using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleAd : MonoBehaviour
{
    public static GoogleAd instance;

    public bool buyAdDel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Initialize the Google Mobile Ads SDK.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string interAdUnitId = "ca-app-pub-1025011428285645/8575153655";
    private string bannerAdUnitId = "ca-app-pub-1025011428285645/3122123790";
    private string rewardAdUnitId = "ca-app-pub-1025011428285645/8661949414";

#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;
    private RewardedAd rewardedAd;
    private BannerView bannerView;

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    
    public void LoadAd()
    {
        // ±¤°íÁ¦°Å ±¸¸ÅÇßÀ» ½Ã
        if (PlayerPrefs.GetInt("RemoveAd") == 1)
        {
            buyAdDel = true;
        }

        if (!buyAdDel)
        {
            StartCoroutine(DelayLoad());
        }
    }

    IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(1f);
        LoadInterstitialAd();
        LoadBannerAd();
        LoadRewardedAd();

    }
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
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardAdUnitId, adRequest,
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

                rewardedAd = ad;
                RegisterReloadHandler(rewardedAd);
            });
    }

    public void ShowRewardedAd(DailyAdManager dailyAd)
    {
        //const string rewardMsg =
        //"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            ADBackOn();

            rewardedAd.Show((Reward reward) =>
            {
                PlayerPrefs.SetInt("isItemReward", 1);
                dailyAd.SubTicket();
                BuyItem_InGame.instance.AD_Gift();
            });

        }
        else
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                PlayerPrefs.SetInt("isItemReward", 1);
                dailyAd.SubTicket();
                BuyItem_InGame.instance.AD_Gift();
            }

            Debug.Log("No have RewardAd");
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


    public void ShowRewardedAdHeart()
    {
        //const string rewardMsg =
        //"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            ADBackOn();

            rewardedAd.Show((Reward reward) =>
            {
                // ¿©±â¼­ º¸»óÁà¾ßµÊ
            });

        }
        else
        {
        }
    }

    public void ShowRewardedAdCoin()
    {

        //const string rewardMsg =
        //"Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            ADBackOn();

            rewardedAd.Show((Reward reward) =>
            {
                PlayerPrefs.SetInt("isCoinReward", 1);
                StageTruckCanvas.Instance.AdReward();
            });

        }
        else
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                PlayerPrefs.SetInt("isCoinReward", 1);
                StageTruckCanvas.Instance.AdReward();
            }

        }
    }

    // ±¤°í µÞÆÇ ¶ç¿ì±â
    void ADBackOn()
    {
        GameManager.Instance.ShowAdBack();
        StartCoroutine(ADBackOffCoroutine());
    }

    IEnumerator ADBackOffCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        GameManager.Instance.HideAdBack();

        yield break;
    }
}