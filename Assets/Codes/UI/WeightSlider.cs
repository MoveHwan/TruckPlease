using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightSlider : MonoBehaviour
{
    public static WeightSlider instance;

    public Slider slider;
    public TextMeshProUGUI TotalWeightText;
    public GameObject Count;
    public Image Fill;

    public StarByWeight[] Stars;
    
    public float[] goalWeights;

    [SerializeField] float totalWeight;
    [SerializeField] int targetIdx;
    [SerializeField] float targetValue;

    Sequence seq;
    Color originalColor;

    bool setEnd;

    void Awake()
    {
        instance = this;

        originalColor = Fill.color;

        seq = DOTween.Sequence();
        seq.Pause();

        seq.Append(Fill.DOColor(Color.red, 0.8f));
        seq.AppendInterval(0.5f);
        seq.Append(Fill.DOColor(originalColor, 0.8f));
        seq.AppendInterval(0.8f);
        seq.SetLoops(-1);
    }

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

        if (Count.activeSelf)
            seq.Play();
        else
        {
            seq.Pause();
            Fill.color = originalColor;
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
                targetValue = totalWeight / goalWeights[targetIdx] * Stars[targetIdx].goalSliderValue;
            }
            else
            {
                targetValue = Stars[targetIdx - 1].goalSliderValue +
                    (Stars[targetIdx].goalSliderValue - Stars[targetIdx - 1].goalSliderValue) * 
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

    public int GetStarCount()
    {
        int count = 0;

        for (int i = 0; i < Stars.Length; i++)
        {
            if (targetValue < Stars[i].goalSliderValue)
                break;

            count++;
        }

        return count;
    }
}
