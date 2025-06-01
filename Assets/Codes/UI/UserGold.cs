using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserGold : MonoBehaviour
{
    public TextMeshProUGUI GoldText;

    int gold;
    string goldStr;

    void Update()
    {
        SetText_Update();
    }

    void SetText_Update()
    {
        gold = PlayerPrefs.GetInt("Gold", 0);

        goldStr = $"{gold:N0}";

        if (GoldText.text != goldStr)
            GoldText.text = goldStr;
    }
}
