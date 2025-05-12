using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowGauge : MonoBehaviour
{
    public static ThrowGauge Instance;

    public Slider PowerSlider;

    public bool isTouch;

    [SerializeField] float value;
    [SerializeField] float targetValue;
    [SerializeField] float speed = 2;


    void Awake()
    {
        Instance = this;

        PowerSlider.value = 0.5f;

        targetValue = Random.Range(0, 2);

    }

    void Update()
    {
        if (isTouch)
        {
            GaugeRandom_Update();
        }
    }

    void GaugeRandom_Update()
    {
        if (targetValue == 0 && PowerSlider.value <= targetValue)
        {
            targetValue = 1;
        }
        else if (targetValue == 1 && PowerSlider.value >= targetValue) 
        {
            targetValue = 0;
        }

        PowerSlider.value = Mathf.MoveTowards(PowerSlider.value, targetValue, Time.deltaTime * speed);

    }

    public void SetGaugeRan()
    {
        isTouch = true;

        PowerSlider.value = 0.5f;

        targetValue = Random.Range(0, 2);
    }

    public float GetGaugeNum()
    {
        isTouch = false;

        value = PowerSlider.value;

        return value;
    }
}
