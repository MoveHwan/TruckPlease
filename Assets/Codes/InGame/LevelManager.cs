using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int level;
    public int currentExp;

    void Awake()
    {
        instance = this;
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
        currentExp += expToAdd;

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // 예: 100 경험치마다 레벨업
        while (currentExp >= 100)
        {
            currentExp -= 100;
            level++;
            Debug.Log($"레벨업! 현재 레벨: {level}");
        }
    }
}
