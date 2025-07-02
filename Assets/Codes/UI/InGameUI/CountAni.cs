using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountAni : MonoBehaviour
{
    public TextMeshProUGUI countText;

    Color blinkColor = Color.red;
    Color originalColor;
    Sequence seq;

    [SerializeField] bool effectOn;

    void Start()
    {
        originalColor = countText.color;
        seq = DOTween.Sequence();
        seq.Pause();

        seq.Append(countText.DOColor(blinkColor, 0.8f));
        seq.AppendInterval(0.5f);
        seq.Append(countText.DOColor(originalColor, 0.8f));
        seq.AppendInterval(0.8f);
        seq.SetLoops(-1);
    }


    void Update()
    {
        if (!effectOn && int.Parse(countText.text) <= 10)
        {
            effectOn = true;
            seq.Play();
        }

        if (GameManager.Instance.gameEnd)
            seq.Kill();
    }

    
}
