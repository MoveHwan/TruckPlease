using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageTruckCanvas : MonoBehaviour
{
    [Header("[ GameOgject ]")]
    public GameObject InGamePanel;
    public GameObject RetryButton;
    public GameObject ClearConfetti;
    public GameObject Count;
    public GameObject[] StarImages; // 3개의 별 이미지

    [Header("[ RectTransform ]")]
    public RectTransform TotalBox;
    public RectTransform TotalWeight;
    public RectTransform Reward;
    public RectTransform Buttons;
    public RectTransform StampImage;

    [Header("[ Text ]")]
    public TextMeshProUGUI LicensePlateText;
    public TextMeshProUGUI TotalBoxText;
    public TextMeshProUGUI TotalWeightText;
    public TextMeshProUGUI BoxCountText;
    public TextMeshProUGUI BoxWeightText;
    public TextMeshProUGUI CoinText;

    [Header("[ Sprite ]")]
    public Sprite Clear;
    public Sprite Fail;

    [Header("[ Other ]")]
    public CanvasGroup ResultCanvas;
    public Courier Courier;


    BoxManager BoxManager;
    GameManager GameManager;

    Sequence resultSeq;

    int starCount;
    bool isResult, isSetTotal;


    void Start()
    {
        string stageNum = PlayerPrefs.GetInt("Stage", 1).ToString();
        int size = stageNum.Length;

        for (int i = 0; i < 3 - size; i++)
            stageNum = "0" + stageNum;

        LicensePlateText.text = "STG - " + stageNum;

        BoxManager = BoxManager.Instance;
        GameManager = GameManager.Instance;

        for (int i = 0; i < StarImages.Length; i++)
            StarImages[i].SetActive(false);

        ResultCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isSetTotal)
        {
            isSetTotal = true;
            WaitReMainBoxInfo();
        }

        if (!isResult && GameManager.gameEnd)
        {
            isResult = true;
            StartCoroutine(ShowResultUIDelay());
        }
    }

    void WaitReMainBoxInfo()
    {
        TotalBoxText.text = "/" + (BoxManager.remainBoxCount + BoxManager.GoaledBoxes.Count);
        TotalWeightText.text = "/"+ GameManager.thirdStar;

    }


    IEnumerator ShowResultUIDelay ()
    {
        yield return new WaitForSeconds(3);

        SetResult();

        InGamePanel.SetActive(false);
        Count.SetActive(false);

        resultSeq.Play();

        yield break;
    }

    void SetResult()
    {
        starCount = WeightSlider.instance.GetStarCount();

        string str = "Stage" + PlayerPrefs.GetInt("Stage") + "_Star";

        if (starCount == 0)
        {
            StampImage.GetComponent<Image>().sprite = Fail;
            RetryButton.SetActive(true);
        }
        else if (PlayerPrefs.GetInt(str, 0) != 0)
        {
            if (starCount == 3)
                RetryButton.SetActive(false);
            else
                RetryButton.SetActive(true);
        }
        else
        {
            RetryButton.SetActive(false);
        }

        if (PlayerPrefs.GetInt(str, 0) < starCount) 
            PlayerPrefs.SetInt(str, starCount);

        if (starCount > 0 && PlayerPrefs.GetInt("TopRatingStage", 0) < PlayerPrefs.GetInt("Stage"))
            PlayerPrefs.SetInt("TopRatingStage", PlayerPrefs.GetInt("Stage"));
        

        resultSeq = DOTween.Sequence();

        ShowResultUI();

        ShowStars();

        LeftMoveAndNumbering(TotalBox, BoxCountText, BoxManager.GoaledBoxes.Count);
        LeftMoveAndNumbering(TotalWeight, BoxWeightText, (int)BoxManager.inBoxWeight);
        LeftMoveAndNumbering(Reward, CoinText, (int)BoxManager.inBoxWeight * 100);

        resultSeq.AppendInterval(0.4f);

        Stamp();

        ButtonsUpMove();

    }

    void ShowResultUI()
    {
        resultSeq.AppendCallback(() =>
        {
            ResultCanvas.gameObject.SetActive(true);
            ResultCanvas.alpha = 0;
            ResultCanvas.transform.localScale = Vector3.one * 0.8f;

            ResultCanvas.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
            ResultCanvas.DOFade(1f, 0.5f);
        }).AppendInterval(0.5f);
    }

    void ShowStars()
    {
        for (int i = 0; i < starCount; i++)
        {
            int idx = i;

            resultSeq.AppendCallback(() =>
            {
                StarImages[idx].SetActive(true);
                StarImages[idx].transform.localScale = Vector3.zero;
                StarImages[idx].transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);
            }).AppendInterval(0.25f);


        }

        resultSeq.AppendInterval(0.2f);
    }

    void LeftMoveAndNumbering(RectTransform targetUI, TextMeshProUGUI TMP, int targetValue)
    {
        // 원래 위치 저장
        Vector2 originalPos = targetUI.anchoredPosition;

        // 캔버스 그룹 없으면 추가
        CanvasGroup canvasGroup = targetUI.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = targetUI.gameObject.AddComponent<CanvasGroup>();

        // 시작 상태: 오른쪽 밖 + 투명
        targetUI.anchoredPosition = originalPos + Vector2.right * Screen.width; 
        canvasGroup.alpha = 0;

        // 이동 + 알파 동시에 트윈
        resultSeq.Append(targetUI.DOAnchorPos(originalPos, 0.5f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.5f))
            .AppendInterval(0.2f)
            .Append(DOVirtual.Int(0, targetValue, 0.5f, value => {
                TMP.text = value.ToString();
            }))
            .AppendInterval(0.1f);
    }

    void Stamp()
    {
        // 시작 상태: 작고 투명
        StampImage.localScale = Vector3.zero;

        CanvasGroup canvasGroup = StampImage.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = StampImage.gameObject.AddComponent<CanvasGroup>();

        StampImage.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        resultSeq.Append(canvasGroup.DOFade(1f, 0.05f))
           .Join(StampImage.DOScale(1.4f, 0.12f).SetEase(Ease.OutBack))
           .Append(StampImage.DOScale(1f, 0.08f).SetEase(Ease.InSine))
           .Append(StampImage.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1f))
           .AppendCallback(() =>
           {
               Courier.Reaction(starCount > 0);
               ClearConfetti.SetActive(starCount > 0);
               // 여기에 사운드 효과 넣는 것도 추천!
               // AudioManager.Play("stamp");
           }).AppendInterval(0.3f);


    }

    void ButtonsUpMove()
    {
        Vector2 originalPos = Buttons.anchoredPosition;

        CanvasGroup canvasGroup = Buttons.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = Buttons.gameObject.AddComponent<CanvasGroup>();

        // 시작 상태: 아래에서 대기 + 투명
        Buttons.anchoredPosition = originalPos - Vector2.up * Screen.height;
        canvasGroup.alpha = 0;

        // 이동 + 서서히 나타나기
        resultSeq.Append(Buttons.DOAnchorPos(originalPos, 0.8f).SetEase(Ease.OutCubic))
                 .Join(canvasGroup.DOFade(1f, 0.8f));
                 
    }





    public void StageRetry()
    {
        SceneManager.LoadScene("InGame");
    }

    public void NextStage()
    {
        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") + 1);

        SceneManager.LoadScene("InGame");
    }

}
