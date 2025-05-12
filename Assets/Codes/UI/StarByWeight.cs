using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarByWeight : MonoBehaviour
{
    public Slider WeightSlider;

    public GameObject NoArrive;
    public GameObject Arrive;

    public TextMeshProUGUI[] Texts;

    public float GoalSliderValue;
    public float GoalWeight;

    
    void Start()
    {
        Arrive.SetActive(false);

        for (int i = 0; i < Texts.Length; i++) Texts[i].text = GoalWeight.ToString();
    }

    
    void Update()
    {
        if (WeightSlider.value >= GoalSliderValue && !Arrive.activeSelf)
            Arrive.SetActive(true);
        else if (WeightSlider.value < GoalSliderValue && Arrive.activeSelf)
            Arrive.SetActive(false);
    }
}