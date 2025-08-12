using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    public static BottomPanel instance;

    BoxManager BoxManager;

    [SerializeField] int nowBoxIdx;

    public GameObject Count;

    [Header("[ UI ]")]
    public RectTransform LeftSide;
    public RectTransform RightSide;
    public RectTransform BottomWeight;

    [Header("[ Box ]")]
    public GameObject NextBox1;
    public GameObject NextBox2;
    public TextMeshProUGUI NowBoxWeightText;
    public TextMeshProUGUI NextBox1WeightText;
    public TextMeshProUGUI NextBox2WeightText;

    [Header("[ Infinite ]")]
    public GameObject NextCount;
    public Image NextMask;
    public Sprite infiniteMask;

    Vector2 defaultLeft, defaultRight, defaultBottom;

    int stage;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        BoxManager = BoxManager.Instance;

        nowBoxIdx = -1;

        if (StageManager.instance.EditorStageCheck())
            stage = PlayerPrefs.GetInt("Stage", 1);
        else
            stage = GameManager.Instance.stageSelect;

        if (stage == 999)
        {
            NextMask.sprite = infiniteMask;
            NextCount.SetActive(false);
        }

        defaultLeft = LeftSide.GetComponent<RectTransform>().anchoredPosition;
        defaultRight = LeftSide.GetComponent<RectTransform>().anchoredPosition;
        defaultBottom = LeftSide.GetComponent<RectTransform>().anchoredPosition;
    }

    
    void Update()
    {
        SetCargoBoxUI_Update();
    }


    void SetCargoBoxUI_Update()
    {
        if (nowBoxIdx == BoxManager.count || !BoxManager.boxReady /*|| (BoxManager.count != 6 && BoxManager.count + 1 != BoxManager.transform.childCount)*/) return;

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


    public void HideUI()
    {
        CanvasGroup canvasGroup;


        canvasGroup = LeftSide.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = LeftSide.AddComponent<CanvasGroup>();

        LeftSide.DOAnchorPosX(defaultLeft.x - LeftSide.rect.width, 0.5f).SetEase(Ease.InQuad);
        canvasGroup.DOFade(0f, 0.5f);


        canvasGroup = RightSide.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = RightSide.AddComponent<CanvasGroup>();

        RightSide.DOAnchorPosX(defaultRight.x + RightSide.rect.width, 0.5f).SetEase(Ease.InQuad);
        canvasGroup.DOFade(0f, 0.5f);


        canvasGroup = BottomWeight.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = BottomWeight.AddComponent<CanvasGroup>();

        BottomWeight.DOAnchorPosY(defaultBottom.y - LeftSide.rect.height, 0.5f).SetEase(Ease.InQuad);
        canvasGroup.DOFade(0f, 0.5f);
    }

    public void ShowUI()
    {
        CanvasGroup canvasGroup;


        canvasGroup = LeftSide.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = LeftSide.AddComponent<CanvasGroup>();

        LeftSide.DOAnchorPosX(defaultLeft.x, 0.5f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(1f, 0.5f);


        canvasGroup = RightSide.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = RightSide.AddComponent<CanvasGroup>();

        RightSide.DOAnchorPosX(defaultRight.x, 0.5f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(1f, 0.5f);


        canvasGroup = BottomWeight.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = BottomWeight.AddComponent<CanvasGroup>();

        BottomWeight.DOAnchorPosY(defaultBottom.y, 0.5f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(1f, 0.5f);
    }


}
