using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameItem : MonoBehaviour
{
    public TextMeshProUGUI ItemCountText;

    int count;

    void Start()
    {
        int count = 6;
        ItemCountText.text = count.ToString();
    }

    public void UseItem()
    {
        if (count <= 0) return;

        count -= 1;

        ItemCountText.text = count.ToString();
    }
}
