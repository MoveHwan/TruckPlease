using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserFatigue : MonoBehaviour
{
    public TextMeshProUGUI FatigueText;
    public TextMeshProUGUI CoolTimeText;

    int fatigue;
    string fatigueStr;

    void Update()
    {
        SetText();
    }

    void SetText()
    {
        fatigue = PlayerPrefs.GetInt("Fatigue", 0);

        fatigueStr = fatigue.ToString() + "/10";

        if (FatigueText.text != fatigueStr)
            FatigueText.text = fatigueStr;

        FatigueManager.instance.UpdateUI(FatigueText, CoolTimeText);
    }


}
