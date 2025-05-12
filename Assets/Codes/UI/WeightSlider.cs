using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI TotalWeightText;

    public StarByWeight[] Stars;
    
    public float[] goalWeights;

    [SerializeField] float totalWeight;
    [SerializeField] int targetIdx;
    [SerializeField] float targetValue;

    bool setEnd;

    void Start()
    {
        StartCoroutine(WaitStarValue());
    }

    void Update()
    {
        if (setEnd)
        {
            totalWeight = BoxManager.Instance.inBoxWeight;

            SetTargetValue_Update();

            slider.value = Mathf.MoveTowards(slider.value, targetValue, Time.deltaTime * 0.8f);
        }
      
    }

    // 다음 슬라이더 값 추출 함수
    void SetTargetValue_Update()
    {
        TotalWeightText.text = totalWeight.ToString();


        if (goalWeights != null)
        {
            // 다음 목표 무게 찾기
            for (int i = 0; i < goalWeights.Length; i++)
            {
                if (i == goalWeights.Length - 1 && totalWeight >= goalWeights[i])
                {
                    targetIdx = i;
                    targetValue = 1;
                    return;
                }
                else if (totalWeight < goalWeights[i])
                {
                    targetIdx = i;
                    break;
                }
            }

            // 슬라이더 값 추출
            if (targetIdx == 0)
            {
                targetValue = totalWeight / goalWeights[targetIdx] * Stars[targetIdx].GoalSliderValue;
            }
            else
            {
                targetValue = Stars[targetIdx - 1].GoalSliderValue +
                    (Stars[targetIdx].GoalSliderValue - Stars[targetIdx - 1].GoalSliderValue) * 
                    (totalWeight - goalWeights[targetIdx-1]) / (goalWeights[targetIdx] - goalWeights[targetIdx - 1]);

            }


        }
    }

    // 목표무게들 세팅
    public void SetGoalWeights(float[] weights)
    {
        goalWeights = weights;

        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].SetGoal(goalWeights[i]);
        }
    }

    IEnumerator WaitStarValue()
    {
        while (GameManager.Instance.thirdStar == 0)
        {
            yield return null;
        }

        SetGoalWeights(new float[] { GameManager.Instance.firstStar, GameManager.Instance.secondStar, GameManager.Instance.thirdStar });

        setEnd = true;
    }
}
