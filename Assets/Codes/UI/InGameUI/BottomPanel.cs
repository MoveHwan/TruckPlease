using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class BottomPanel : MonoBehaviour
{
    BoxManager BoxManager;

    [SerializeField] int nowBoxIdx;

    public GameObject Count;
    
    [Header("[ Cargo ]")]
    public TextMeshProUGUI CargoBoxCountText;
    public TextMeshProUGUI CargoBoxWeightText;
    public Image CargoBack;

    [Header("[ Box ]")]
    public GameObject NextBox1;
    public GameObject NextBox2;
    public TextMeshProUGUI NowBoxWeightText;
    public TextMeshProUGUI NextBox1WeightText;
    public TextMeshProUGUI NextBox2WeightText;

    Sequence seq;
    Color originalColor, targetColor;

    bool isWarning;

    void Start()
    {
        BoxManager = BoxManager.Instance;

        nowBoxIdx = -1;

        originalColor = CargoBack.color;

        targetColor = new(1, 0.5f, 0.5f, 1);
    }

    
    void Update()
    {
        CargoBoxCountText.text = BoxManager.remainBoxCount.ToString();
        CargoBoxWeightText.text = BoxManager.remainBoxWeight.ToString();

        SetCargoBoxUI_Update();

        if (!isWarning && BoxManager.Instance.boxWarn)
        {
            isWarning = true;
            SetWarningSeq();
        }

        if (!BoxManager.Instance.boxWarn && isWarning)
        {
            isWarning = false;

            seq.Kill();
            CargoBack.color = originalColor;
        }

    }

    void SetWarningSeq()
    {
        seq = DOTween.Sequence();
        seq.Pause();

        seq.Append(CargoBack.DOColor(targetColor, 0.8f));
        seq.AppendInterval(0.5f);
        seq.Append(CargoBack.DOColor(originalColor, 0.8f));
        seq.AppendInterval(0.8f);
        seq.SetLoops(-1);

        seq.Play();
    }

    void SetCargoBoxUI_Update()
    {
        if (nowBoxIdx == BoxManager.count || (BoxManager.count != 6 && BoxManager.count + 1 != BoxManager.transform.childCount)) return;

        nowBoxIdx = BoxManager.count;

        GameObject sourceBox;

        // 3번째 남은 박스
        if (BoxManager.remainBoxCount >= 3)
        {
            if (!NextBox2.activeSelf) NextBox2.SetActive(true);

            sourceBox = BoxManager.box[nowBoxIdx + 2];

            NextBox2.GetComponent<MeshFilter>().sharedMesh = sourceBox.GetComponent<MeshFilter>().sharedMesh;
            NextBox2.GetComponent<MeshRenderer>().sharedMaterials = sourceBox.GetComponent<MeshRenderer>().sharedMaterials;
            NextBox2.GetComponent<Transform>().localScale = sourceBox.GetComponent<Transform>().localScale;

            NextBox2.transform.localRotation = sourceBox.transform.localRotation;

            NextBox2WeightText.text = sourceBox.GetComponent<ThrowBox>().boxData.Weight.ToString();
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

            sourceBox = BoxManager.box[nowBoxIdx + 1];

            NextBox1.GetComponent<MeshFilter>().sharedMesh = sourceBox.GetComponent<MeshFilter>().sharedMesh;
            NextBox1.GetComponent<MeshRenderer>().sharedMaterials = sourceBox.GetComponent<MeshRenderer>().sharedMaterials;
            NextBox1.GetComponent<Transform>().localScale = sourceBox.GetComponent<Transform>().localScale;

            NextBox1.transform.localRotation = sourceBox.transform.localRotation;

            NextBox1WeightText.text = sourceBox.GetComponent<ThrowBox>().boxData.Weight.ToString();
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
