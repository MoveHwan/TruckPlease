using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;
using System;

public class DiscountRemoveAds : MonoBehaviour
{
    public TextMeshProUGUI[] Limited_Times;
    public TextMeshProUGUI[] Limited_Times_PopUp;
    public TextMeshProUGUI OriginalPrice;
    public TextMeshProUGUI Sale_Price;

    DateTime targetTime;
    TimeSpan subTime;

    Color red = new(0.87f, 0.369f, 0.235f);
    Color blue = new(0.302f, 0.651f, 1);

    void Start()
    {
        if (!PlayerPrefs.HasKey("Limited_Time_RemoveAds"))
            PlayerPrefs.SetString("Limited_Time_RemoveAds", DateTime.Now.AddDays(2).ToString());

        targetTime = DateTime.Parse(PlayerPrefs.GetString("Limited_Time_RemoveAds", DateTime.Now.AddDays(2).ToString()));

        StartCoroutine(WaitIAPManager());
    }

    
    void Update()
    {
        Limited_Time_Update();
    }

    IEnumerator WaitIAPManager()
    {
        yield return new WaitUntil(() => IAPManager.Instance != null && IAPManager.Instance.initializeEnd);

        Product product = IAPManager.Instance.GetProduct("removeads");

        OriginalPrice.text = product.metadata.localizedPriceString;
    }

    void Limited_Time_Update()
    {
        subTime = targetTime - DateTime.Now;

        int hours = subTime.Days * 24 + subTime.Hours;
        Color color = hours <= 0 ? red : blue;

        Limited_Times[0].text = Limited_Times_PopUp[0].text = $"{hours:D2}";
        Limited_Times[1].text = Limited_Times_PopUp[1].text = $"{subTime.Minutes:D2}";
        Limited_Times[2].text = Limited_Times_PopUp[2].text = $"{subTime.Seconds:D2}";



        for (int i = 0; i < Limited_Times.Length; i++)
        {
            if (Limited_Times[i].color == color) break;

            Limited_Times[i].color = color;
        }

        for (int i = 0;i < Limited_Times_PopUp.Length; i++)
        {
            if (Limited_Times_PopUp[i].color == color) break;

            Limited_Times_PopUp[i].color = color;
        }
    }

}
