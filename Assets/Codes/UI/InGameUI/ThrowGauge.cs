using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowGauge : MonoBehaviour
{
    public Slider PowerSlider;
    public Image Fill;


    void Update()
    {
        Fill.fillAmount = PowerSlider.value;
    }

}
