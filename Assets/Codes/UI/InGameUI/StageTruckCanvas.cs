using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

public class StageTruckCanvas : MonoBehaviour
{
    public static StageTruckCanvas Instance;

    [Header("[ GameOgject ]")]
    public GameObject InGamePanel;
    public GameObject RetryButton;
    public GameObject ADButton;
    public GameObject ClearConfetti;
    public GameObject Count;
    public GameObject NoHeartPopUp;
    public GameObject ReviewPopUp;
    public GameObject StageMap;
    public GameObject[] StarImages; // 3개의 별 이미지

    [Header("[ RectTransform ]")]
    public RectTransform TotalBox;
    public RectTransform TotalWeight;
    public RectTransform Reward;
    public RectTransform Buttons;
    public RectTransform StampImage;
    public RectTransform LobbyButton;
    public RectTransform LostHeartPanel;

    [Header("[ Text ]")]
    public TextMeshProUGUI PauseStageText;
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
    public ItemUnlock ItemUnlock;
    //public NoHeart NoHeart;


    BoxManager BoxManager;
    GameManager GameManager;

    DG.Tweening.Sequence resultSeq;

    int starCount, rewardCoin, stageNum;
    bool isResult, isSetTotal, tuto;


    private void Awake()
    {
        Instance = this;

        if (!StageMap.activeSelf) StageMap.SetActive(true);
    }

    void Start()
    {
        tuto = PlayerPrefs.GetInt("Tutorial", 0) == 0;

        if (StageManager.instance.EditorStageCheck())
            stageNum = PlayerPrefs.GetInt("Stage", 1);
        else
            stageNum = GameManager.stageSelect;

        string str = "Stage - ";

        if (stageNum == 999)
            str += "∞";
        else
            str += stageNum;

        PauseStageText.text = str;
        LicensePlateText.text = str;

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
        TotalBoxText.text = "/" + (tuto ? "4" : (BoxManager.remainBoxCount + BoxManager.GoaledBoxes.Count));
        TotalWeightText.text = "/" + GameManager.thirdStar;

    }


    IEnumerator ShowResultUIDelay()
    {
        InGamePanel.SetActive(false);

        yield return new WaitForSeconds(3);

        SetResult();

        Count.SetActive(false);

        resultSeq.Play();

        yield break;
    }

    void SetResult()
    {
        starCount = WeightSlider.instance.GetStarCount();

        string str = "Stage" + PlayerPrefs.GetInt("Stage") + "_star";

        rewardCoin = InGameGoldUI.Instance.GetTotalRewardGold();

        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + rewardCoin);

        if (rewardCoin <= 0)
            ADButton.SetActive(false);

        // Retry버튼 활성화 여부
        if (starCount == 0)
        {
            FatigueManager.instance.SubFatigue();

            StampImage.GetComponent<Image>().sprite = Fail;
            RetryButton.SetActive(true);

        }
        else if (PlayerPrefs.GetInt(str, 0) != 0)
        {
            if (starCount == 3)
                RetryButton.SetActive(false);
            else
                RetryButton.SetActive(true);

            PlayerPrefs.SetInt("StageIn", 0);
        }
        else
        {
            RetryButton.SetActive(false);

            PlayerPrefs.SetInt("StageIn", 0);
        }


        if (PlayerPrefs.GetInt(str, 0) < starCount)
        {
            PlayerPrefs.SetInt(str, starCount);

            if (GameDatas.instance)
                GameDatas.instance.NewStageStar();
        }


        if (stageNum != 999 && stageNum > PlayerPrefs.GetInt("TopStage", 0))
            PlayerPrefs.SetInt("TopStage", stageNum);


        resultSeq = DOTween.Sequence();

        ShowResultUI();

        ShowStars();

        //LeftMoveAndNumbering(TotalBox, BoxCountText, BoxManager.GoaledBoxes.Count);

        if ((int)BoxManager.inBoxWeight != BoxManager.inBoxWeight)
            LeftMoveAndNumbering(TotalWeight, BoxWeightText, BoxManager.inBoxWeight);
        else
            LeftMoveAndNumbering(TotalWeight, BoxWeightText, (int)BoxManager.inBoxWeight);

        LeftMoveAndNumbering(Reward, CoinText, rewardCoin);

        resultSeq.AppendInterval(0.4f);

        Stamp();

        ShowMap();

        CourierActive();

        ButtonsUpMove();

        LeftMoveAndNumbering(LobbyButton, null, 0);

        resultSeq.AppendCallback(() => GameManager.StackIntAdClear())
            .AppendInterval(0.1f);

        //resultSeq.AppendCallback(() => ItemUnlock.UnlockCheck(starCount));

        resultSeq.AppendCallback(() =>
        {
            if (starCount <= 0) return;

            if (PlayerPrefs.GetInt("Review", 0) < 5)
            {
                PlayerPrefs.SetInt("Review", PlayerPrefs.GetInt("Review", 0) + 1);
            }

            if (PlayerPrefs.GetInt("ReviewOn", 0) != 1 && PlayerPrefs.GetInt("Review", 0) >= 5)
            {
                Debug.LogWarning("ReviewOn");

                ReviewPopUp.SetActive(true);
                PlayerPrefs.SetInt("ReviewOn", 1);
            }

        });

        PlayerPrefs.Save();


        if (Application.internetReachability != NetworkReachability.NotReachable && GameDatas.instance)
        {
            GameDatas.instance.CloudSave();
        }

    }

    void ShowResultUI()
    {
        resultSeq.AppendCallback(() =>
        {
            ResultCanvas.gameObject.SetActive(true);
            ResultCanvas.alpha = 0;
            ResultCanvas.transform.localScale = Vector3.one * 0.8f;

            ResultCanvas.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            ResultCanvas.DOFade(1f, 0.3f);
        }).AppendInterval(0.35f);
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
            })
            .AppendCallback(() =>
            {
                if (AudioManager.instance != null)
                {
                    switch (idx)
                    {
                        case 0:
                            AudioManager.instance.PlaySfx(AudioManager.Sfx.star1);
                            break;
                        case 1:
                            AudioManager.instance.PlaySfx(AudioManager.Sfx.star2);
                            break;
                        case 2:
                            AudioManager.instance.PlaySfx(AudioManager.Sfx.star3);
                            break;
                        default:
                            break;
                    }
                }
            })
            .AppendInterval(0.2f);


        }

    }

    void LeftMoveAndNumbering(RectTransform targetUI, TextMeshProUGUI TMP, int targetValue)
    {
        // 원래 위치 저장
        Vector2 originalPos = targetUI.anchoredPosition;

        // 캔버스 그룹 없으면 추가
        CanvasGroup canvasGroup = targetUI.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = targetUI.AddComponent<CanvasGroup>();
        }


        // 시작 상태: 오른쪽 밖 + 투명
        targetUI.anchoredPosition = originalPos + Vector2.right * Screen.width;
        canvasGroup.alpha = 0;

        // 이동 + 알파 동시에 트윈
        resultSeq.Append(targetUI.DOAnchorPos(originalPos, 0.3f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.3f))
            .AppendInterval(0.05f);

        if (targetValue != 0)
        {
            resultSeq.Append(DOVirtual.Int(0, targetValue, 0.2f, value =>
            {
                TMP.text = value.ToString();
            }));
        }

    }
    void LeftMoveAndNumbering(RectTransform targetUI, TextMeshProUGUI TMP, float targetValue)
    {
        // 원래 위치 저장
        Vector2 originalPos = targetUI.anchoredPosition;

        // 캔버스 그룹 없으면 추가
        CanvasGroup canvasGroup = targetUI.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = targetUI.AddComponent<CanvasGroup>();
        }

        // 시작 상태: 오른쪽 밖 + 투명
        targetUI.anchoredPosition = originalPos + Vector2.right * Screen.width;
        canvasGroup.alpha = 0;

        // 이동 + 알파 동시에 트윈
        resultSeq.Append(targetUI.DOAnchorPos(originalPos, 0.3f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.3f));

        if (targetValue != 0)
        {
            resultSeq.Append(DOVirtual.Float(0f, targetValue, 0.2f, value =>
            {
                TMP.text = (Mathf.Round(value * 100f) / 100f).ToString();
            }));
        }

    }

    void Stamp()
    {
        // 시작 상태: 작고 투명
        StampImage.localScale = Vector3.zero;

        CanvasGroup canvasGroup = StampImage.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = StampImage.AddComponent<CanvasGroup>();
        }


        StampImage.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        resultSeq.Append(canvasGroup.DOFade(1f, 0.05f))
           .Join(StampImage.DOScale(1.4f, 0.12f).SetEase(Ease.OutBack))
           .AppendCallback(() =>
           {
               if (AudioManager.instance != null)
               {
                   AudioManager.instance.PlaySfx(AudioManager.Sfx.stamp);
               }
           })
           .Append(StampImage.DOScale(1f, 0.08f).SetEase(Ease.InSine))
           .Append(StampImage.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1f))
           .AppendInterval(0.2f);

    }


    void ShowMap()
    {
        if (starCount <= 0) return;

        CanvasGroup canvasGroup = StageMap.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = StageMap.AddComponent<CanvasGroup>();
        }

        StageMap.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;

        resultSeq.AppendCallback(() => StageMap.SetActive(true))
            .Append(StageMap.transform.DOScale(1f, 0.4f).SetEase(Ease.OutCubic))
            .Join(canvasGroup.DOFade(1f, 0.4f))
            .AppendInterval(0.2f)
            .AppendCallback(() => 
            { 
                StageManager.instance.StageMapView(starCount);
                resultSeq.Pause();
            });
    }

    void CourierActive()
    {
        resultSeq.AppendCallback(() =>
        {
            Courier.Reaction(starCount > 0);
            ClearConfetti.SetActive(starCount > 0);

            StartCoroutine(CourierSfxDelay());
        }).AppendInterval(0.2f);
    }

    void ButtonsUpMove()
    {
        Vector2 originalPos = Buttons.anchoredPosition;

        CanvasGroup canvasGroup = Buttons.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = Buttons.AddComponent<CanvasGroup>();
            return;
        }

        // 시작 상태: 아래에서 대기 + 투명
        Buttons.anchoredPosition = originalPos - Vector2.up * Screen.height;
        canvasGroup.alpha = 0;

        // 이동 + 서서히 나타나기
        resultSeq.Append(Buttons.DOAnchorPos(originalPos, 0.4f).SetEase(Ease.OutCubic))
                 .Join(canvasGroup.DOFade(1f, 0.4f));

    }

    public void ResultSeqPlay()
    {
        resultSeq.Play();
    }



    public void AdReward()
    {
        if (PlayerPrefs.GetInt("isCoinReward", 0) == 0) return;

        PlayerPrefs.SetInt("isCoinReward", 0);
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) + rewardCoin * 3);

        PlayerPrefs.Save();

        ADButton.SetActive(false);

        resultSeq.Kill();
        resultSeq.Pause();

        resultSeq.AppendInterval(0.3f);
        resultSeq.Append(DOVirtual.Int(rewardCoin, rewardCoin * 4, 0.2f, value =>
        {
            CoinText.text = value.ToString();
        }));

        resultSeq.Play();
    }

    

    IEnumerator CourierSfxDelay()
    {
        yield return new WaitForSeconds(1);

        if (AudioManager.instance != null)
        {
            if (starCount > 0)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.winMan);
            else
                AudioManager.instance.PlaySfx(AudioManager.Sfx.loseMan);
        }
    }



    public void CheckPauseRetry()
    {
        if (!FatigueManager.instance.CheckRetryFatigue())
        {
            NoHeartPopUp.SetActive(true);
            return;
        }

        Button btn = LostHeartPanel.GetChild(4).GetComponent<Button>();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => StagePauseRetry());

        LostHeartPanel.gameObject.SetActive(true);
    }


    public void CheckStageRetry()
    {
        if (!FatigueManager.instance.CheckFatigue())
        {
            NoHeartPopUp.SetActive(true);
            return;
        }

        Button btn = LostHeartPanel.GetChild(4).GetComponent<Button>();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => StageRetry());

        LostHeartPanel.gameObject.SetActive(true);
    }

    public void CheckLobby()
    {
        Button btn = LostHeartPanel.GetChild(4).GetComponent<Button>();

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => Lobby());

        LostHeartPanel.gameObject.SetActive(true);
    }


    public void StagePauseRetry()
    {
        if (!FatigueManager.instance.SubFatigue())
        {
            NoHeartPopUp.SetActive(true);
            return;
        }

        DOTween.KillAll();

        GameManager.Instance.GameResume();

        FatigueManager.instance.StageIn();

        SceneManager.LoadScene("InGame");
    }

    public void StageRetry()
    {
        if (!FatigueManager.instance.CheckFatigue())
        {
            NoHeartPopUp.SetActive(true);
            return;
        }

        DOTween.KillAll();

        GameManager.Instance.GameResume();

        FatigueManager.instance.StageIn();

        SceneManager.LoadScene("InGame");
    }

    public void NextStage()
    {
        if (!FatigueManager.instance.CheckFatigue())
        {
            NoHeartPopUp.SetActive(true);
            return;
        }

        DOTween.KillAll();

        GameManager.Instance.GameResume();

        PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") + 1);
        FatigueManager.instance.StageIn();

        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScene");
    }

    public void Lobby()
    {
        GameManager.Instance.GameResume();

        DOTween.KillAll();

        SceneManager.LoadScene("Lobby");
    }


}
