using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Slider LevelSlider;
    public Slider BackLevelSlider;
    public TextMeshProUGUI LevelText;

    public int level;
    public int currentExp;
    
    const int maxExp = 100;

    bool isDelay;

    void Awake()
    {
        instance = this;

        currentExp = PlayerPrefs.GetInt("UserExp", 0);

        level = currentExp / maxExp;
        LevelText.text = level.ToString();

        LevelSlider.maxValue = maxExp;
        BackLevelSlider.maxValue = maxExp;

        currentExp %= maxExp;

        LevelSlider.value = currentExp;
        BackLevelSlider.value = currentExp;
    }

    void Update()
    {
        if (!isDelay && LevelSlider.value < currentExp)
        {
            if (LevelSlider.value >= currentExp - 0.005f)
            {
                LevelSlider.value = currentExp;
                return;
            }

            LevelSlider.value = Mathf.Lerp(LevelSlider.value, currentExp, Time.deltaTime * 9);
        }
    }

    // 색깔에 따라 경험치 추가
    public void AddExpFromBox(StickyBlock sticky)
    {
        int expToAdd = 0;

        switch (sticky.blockColor)
        {
            case BlockColor.Red:
                expToAdd = 1;
                break;
            case BlockColor.Orange:
                expToAdd = 2;
                break;
            case BlockColor.Yellow:
                expToAdd = 4;
                break;
            case BlockColor.Green:
                expToAdd = 6;
                break;
            case BlockColor.Blue:
                expToAdd = 10;
                break;
            case BlockColor.Pink:
                expToAdd = 15;
                break;
            case BlockColor.Skyblue:
                expToAdd = 20;
                break;
            case BlockColor.Mint:
                expToAdd = 40;
                break;
            case BlockColor.Purple:
                expToAdd = 60;
                break;
        }

        PlayerPrefs.SetInt("UserExp", PlayerPrefs.GetInt("UserExp", 0) + expToAdd);

        currentExp += expToAdd;
        BackLevelSlider.value = currentExp;

        StartCoroutine(FillExpWaitDelay());

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            level++;

            LevelText.text = level.ToString();

            LevelSlider.value = currentExp;
            BackLevelSlider.value = currentExp;

            LevelUpPopUp.Instance.PopUpOn(level);

            Debug.Log($"레벨업! 현재 레벨: {level}");
        }

    }

    IEnumerator FillExpWaitDelay()
    {
        if (isDelay) yield break;

        isDelay = true;

        yield return new WaitForSeconds(0.3f);

        isDelay = false;

        CheckLevelUp();
    }
}
