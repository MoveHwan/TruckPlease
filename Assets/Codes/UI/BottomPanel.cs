using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BottomPanel : MonoBehaviour
{
    BoxManager BoxManager;

    [SerializeField] int nowBoxIdx;
    
    [Header("[ Cargo ]")]
    public TextMeshProUGUI CargoBoxCountText;
    public TextMeshProUGUI CargoBoxWeightText;

    [Header("[ Box ]")]
    public GameObject NextBox1;
    public GameObject NextBox2;
    public TextMeshProUGUI NowBoxWeightText;
    public TextMeshProUGUI NextBox1WeightText;
    public TextMeshProUGUI NextBox2WeightText;


    void Start()
    {
        BoxManager = BoxManager.Instance;

        nowBoxIdx = -1;
    }

    
    void Update()
    {
        CargoBoxCountText.text = BoxManager.remainBoxCount.ToString();
        CargoBoxWeightText.text = BoxManager.remainBoxWeight.ToString();

        SetCargoBoxUI_Update();
    }


    void SetCargoBoxUI_Update()
    {
        if (nowBoxIdx == BoxManager.count || (BoxManager.count != 6 && BoxManager.count + 1 != BoxManager.transform.childCount)) return;

        nowBoxIdx = BoxManager.count;

        // 3번째 남은 박스
        if (BoxManager.remainBoxCount >= 3)
        {
            if (!NextBox2.activeSelf) NextBox2.SetActive(true);

            NextBox2.GetComponent<Transform>().localScale = BoxManager.box[nowBoxIdx + 2].GetComponent<Transform>().localScale;
            NextBox2WeightText.text = BoxManager.box[nowBoxIdx + 2].GetComponent<ThrowBox>().boxData.Weight.ToString();
        }
        else
        {
            if (NextBox2.activeSelf) NextBox2.SetActive(false);
            if (NextBox2WeightText.text != "-") NextBox2WeightText.text = "-";
        }

        // 2번째 남은 박스
        if (BoxManager.remainBoxCount >= 2)
        {
            if (!NextBox1.activeSelf) NextBox1.SetActive(true);

            NextBox1.GetComponent<Transform>().localScale = BoxManager.box[nowBoxIdx + 1].GetComponent<Transform>().localScale;
            NextBox1WeightText.text = BoxManager.box[nowBoxIdx + 1].GetComponent<ThrowBox>().boxData.Weight.ToString();
        }
        else
        {
            if (NextBox1.activeSelf) NextBox1.SetActive(false);
            if (NextBox1WeightText.text != "-") NextBox1WeightText.text = "-";
        }

        if (BoxManager.remainBoxCount >= 1) 
        {
            NowBoxWeightText.text = BoxManager.box[nowBoxIdx].GetComponent<ThrowBox>().boxData.Weight.ToString();
        }
        else
        {
            if (NowBoxWeightText.text != "-") NowBoxWeightText.text = "-";
        }

    }
}
