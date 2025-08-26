using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsController : MonoBehaviour
{
    public static RemoveAdsController instance;

    public PurchaManager RemoveAds;
    public PurchaManager RemoveAds_Sale;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.GetInt("RemoveAdsControllerPopUp", 0) == 1)
        {
            DateTime limitedTime = DateTime.Parse(PlayerPrefs.GetString("Limited_Time_RemoveAds", DateTime.Now.AddDays(2).ToString()));

            if (DateTime.Now > limitedTime)
            {
                RemoveAds.gameObject.SetActive(true);
                RemoveAds_Sale.gameObject.SetActive(false);
            }
            else
            {
                RemoveAds.gameObject.SetActive(false);
                RemoveAds_Sale.gameObject.SetActive(true);
            }
        }
    }

    public void RemoveAdsOn()
    {
        if (PlayerPrefs.GetInt("RemoveAdsControllerPopUp", 0) == 1) return;

        PlayerPrefs.SetInt("RemoveAdsControllerPopUp", 1);

        DateTime limitedTime = DateTime.Parse(PlayerPrefs.GetString("Limited_Time_RemoveAds", DateTime.Now.AddDays(2).ToString()));

        if (DateTime.Now > limitedTime)
        {
            RemoveAds.gameObject.SetActive(true);
            RemoveAds_Sale.gameObject.SetActive(false);

            RemoveAds.RemoveADPopUp.SetActive(true);
        }
        else
        {
            RemoveAds.gameObject.SetActive(false);
            RemoveAds_Sale.gameObject.SetActive(true);

            RemoveAds_Sale.RemoveADPopUp.SetActive(true);
        }
    }
}
