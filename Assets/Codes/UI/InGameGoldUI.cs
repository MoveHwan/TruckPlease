using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameGoldUI : MonoBehaviour
{
    public static InGameGoldUI Instance;

    public RectTransform GoldIcon;
    public TextMeshProUGUI GoldText;
    public GameObject FireAni;

    public bool fireOn;

    Sequence GoldSeq, GoldTextSeq;

    [SerializeField] int gold;
    [SerializeField] int bonusGold;
    [SerializeField] int starGold;

    bool isRefresh;
    int stage, chapter, starCount;
    string stageStarStr;

    int[] stageRewards = {
        40, 40, 60, 40, 40, 60, 40, 40, 60,
        50, 50, 75, 50, 50, 75, 50, 50, 75,
        50, 50, 75, 60, 60, 90, 60, 60, 90,
        60, 60, 90, 60, 60, 90
    };

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        stage = PlayerPrefs.GetInt("Stage", 1);

        stageStarStr = "Stage" + stage + "_Star";

        if (stage < 10)
        {
            chapter = (stage - 1) / 9 + 1;
            stage = stage % 9 == 0 ? 9 : stage % 9;
        }
        else
        {
            stage -= 9;

            chapter = (stage - 1) / 12 + 2;
            stage = stage % 12 == 0 ? 12 : stage % 12;
        }

        bonusGold = 10 + (chapter - 1) * 5;

        SetGoldEffect();
        SetGoldTextEffect();
    }

    void Update()
    {
        fireOn = VfxManager.instance.stack >= 2;

        if (fireOn && !FireAni.activeSelf)
        {
            FireAni.SetActive(true);
        }
        else if (!fireOn && FireAni.activeSelf)
        {
            FireAni.SetActive(false);
        }

        RefreshStarGold();

        if (!isRefresh && gold + starGold != int.Parse(GoldText.text)) 
        {
            isRefresh = true;
            RefreshGold();
        }
    }

    public void GetGold()
    {
        gold += bonusGold;
    }


    void SetGoldEffect()
    {
        GoldSeq = DOTween.Sequence();
        GoldSeq.Pause();

        GoldSeq.SetAutoKill(false);

        GoldSeq.Append(GoldIcon.DOScale(1.3f, 0.15f).SetEase(Ease.OutBack))  // 커지기
            .Append(GoldIcon.DOScale(1.0f, 0.1f).SetEase(Ease.InQuad))   // 원래대로
            .Join(GoldIcon.DORotate(new Vector3(0, 0, 10f), 0.05f))      // 살짝 흔들
            .Append(GoldIcon.DORotate(Vector3.zero, 0.05f));            // 복귀
    }

    void SetGoldTextEffect()
    {
        GoldTextSeq = DOTween.Sequence();
        GoldTextSeq.Pause();

        GoldTextSeq.SetAutoKill(false);

        GoldTextSeq.AppendCallback(() =>
        {
            int prevGold = int.Parse(GoldText.text);

            DOVirtual.Int(prevGold, gold + starGold, 0.4f, value =>
            {
                GoldText.text = (Mathf.Round(value * 100f) / 100f).ToString();
            }).OnComplete(() =>
            {
                isRefresh = false;
            });
        });
    }

    public void PlayGoldEffect()
    {
        GoldIcon.DOKill();
        GoldIcon.localScale = Vector3.one;

        if (GoldSeq == null || !GoldSeq.IsActive())
        {
            SetGoldEffect(); // 혹시 seq가 죽었거나 없으면 다시 만듦
        }

        GoldSeq.Restart(); // 무조건 처음부터 재생
    }

    public void PlayGoldTextEffect()
    {
        if (GoldTextSeq == null || !GoldTextSeq.IsActive())
        {
            SetGoldTextEffect(); // 혹시 seq가 죽었거나 없으면 다시 만듦
        }

        GoldTextSeq.Restart(); // 무조건 처음부터 재생
    }

    void RefreshStarGold()
    {
        starCount = WeightSlider.instance.GetStarCount();

        starGold = GetStageReward(GameManager.Instance.stage, starCount, stageStarStr);
    }

    void RefreshGold()
    {
        if (gold + starGold > int.Parse(GoldText.text))
            PlayGoldEffect();

        PlayGoldTextEffect();
    }

    public int GetTotalRewardGold()
    {
        return gold + starGold;
    }


    int GetStageReward(int stage, int starCount, string stageStar)
    {
        if (starCount <= 0) return 0;

        int accStarCount = PlayerPrefs.GetInt(stageStar, 0);

        int amount = stageRewards[stage - 1] / 3;

        int[] rewards = { amount, amount, amount + stageRewards[stage - 1] % 3 };

        amount = 0;

        for (int i = 0; i < rewards.Length; i++)
        {
            if (accStarCount > 0)
            {
                rewards[i] = 0;
            }
            else if (starCount <= 0)
            {
                rewards[i] = 0;
            }

            accStarCount -= 1;
            starCount -= 1;


            amount += rewards[i];
        }


        return amount;
    }
}
