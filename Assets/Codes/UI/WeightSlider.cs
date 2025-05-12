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

    // ���� �����̴� �� ���� �Լ�
    void SetTargetValue_Update()
    {
        TotalWeightText.text = totalWeight.ToString();


        if (goalWeights != null)
        {
            // ���� ��ǥ ���� ã��
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

            // �����̴� �� ����
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

    // ��ǥ���Ե� ����
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
