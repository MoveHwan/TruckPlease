using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserFatigue : MonoBehaviour
{
    public TextMeshProUGUI FatigueText;
    public TextMeshProUGUI CoolTimeText;

    public bool isNoMore;

    int fatigue;
    string fatigueStr;


    void Update()
    {
        SetText();
    }

    void SetText()
    {
        fatigue = PlayerPrefs.GetInt("Fatigue", 0);

        if (isNoMore)
            fatigueStr = "<color=#FF9432>" + fatigue.ToString() + "</color>/10";
        else
            fatigueStr = fatigue.ToString() + "/10";

        if (FatigueText.text != fatigueStr)
            FatigueText.text = fatigueStr;

        FatigueManager.instance.UpdateUI(FatigueText, CoolTimeText);
    }


}
