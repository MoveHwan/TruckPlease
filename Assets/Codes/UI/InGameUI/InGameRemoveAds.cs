using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRemoveAds : MonoBehaviour
{
    public static InGameRemoveAds instance;

    public PurchaManager RemoveAds;
    public PurchaManager RemoveAds_Sale;

    void Awake()
    {
        instance = this;
    }

    public void RemoveAdsOn()
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

        if (PlayerPrefs.GetInt("InGameRemoveAdsPopUp", 0) == 0)
        {
            PlayerPrefs.SetInt("InGameRemoveAdsPopUp", 1);

            if (DateTime.Now > limitedTime)
                RemoveAds.RemoveADPopUp.SetActive(true);
            else
                RemoveAds_Sale.RemoveADPopUp.SetActive(true);
        }


    }
}
