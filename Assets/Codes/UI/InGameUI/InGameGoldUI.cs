using DG.Tweening;
using System.Linq;
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

    [SerializeField] int accGold;
    [SerializeField] int bonusGold;
    [SerializeField] int resultGold;

    bool isRefresh;
    int stage, chapter, starCount, prevBox;
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
        if (StageManager.instance.EditorStageCheck())
            stage = PlayerPrefs.GetInt("Stage", 1);
        else
            stage = GameManager.Instance.stageSelect;

        stageStarStr = "Stage" + stage + "_star";

        if (stage == 999)
        {
            bonusGold = 50;
        }
        else
        {
            chapter = stage / 6 + 1;

            if (stage % 6 == 0)
                chapter -= 1;

            bonusGold = 10 + (chapter - 1) * 5;
        }


        SetGoldEffect();
        SetGoldTextEffect();
    }

    void Update()
    {
        /*fireOn = VfxManager.instance.stack >= 2;

        if (fireOn && !FireAni.activeSelf)
        {
            FireAni.SetActive(true);
        }
        else if (!fireOn && FireAni.activeSelf)
        {
            FireAni.SetActive(false);
        }*/

        //RefreshresultGold();

        if (!isRefresh && accGold + resultGold != int.Parse(GoldText.text)) 
        {
            isRefresh = true;
            RefreshGold();
        }

        if (prevBox != BoxManager.Instance.GoaledBoxes.Count)
        {
            prevBox = BoxManager.Instance.GoaledBoxes.Count;

            //resultGold = GetTopBoxReward();
        }
    }

    public void GetGold()
    {
        //accGold += bonusGold;
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

            DOVirtual.Int(prevGold, accGold + resultGold, 0.4f, value =>
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

    void RefreshGold()
    {
        if (accGold > int.Parse(GoldText.text))
            PlayGoldEffect();

        PlayGoldTextEffect();
    }

    public int GetTotalRewardGold()
    {
        if (stage == 999)
        {
            //resultGold = GetTopBoxReward();
        }
        else
        {
            starCount = WeightSlider.instance.GetStarCount();
            resultGold = GetStageReward(stage, starCount, stageStarStr);
        }
        
        return accGold + resultGold;
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

    int GetTopBoxReward()
    {
        if (BoxManager.Instance.GoaledBoxes.Count == 0) return resultGold;

        float topBox = BoxManager.Instance.GoaledBoxes.Max(box => box.GetComponent<ThrowBox>().weight);

        int amount = 0;

        switch (topBox)
        {
            case 3000:
                amount = 30;
                break;
            case 1200:
                amount = 10;
                break;
            case 500:
                amount = 5;
                break;
            default:
                amount = 0;
                break;
        }

        return amount;
    }

    public void AddBoxGold(int boxGold)
    {
        if (stage == 999)
        {
            resultGold += boxGold;
        }
    }
}
