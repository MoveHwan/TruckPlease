using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfiniteScore : MonoBehaviour
{
    public static InfiniteScore Instance;

    public TextMeshProUGUI ScoreText;

    float score;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (score != BoxManager.Instance.inBoxWeight)
        {
            score = BoxManager.Instance.inBoxWeight;
            ScoreText.text = score.ToString();
        }
    }

    public int GetScore() => (int)score;
}
