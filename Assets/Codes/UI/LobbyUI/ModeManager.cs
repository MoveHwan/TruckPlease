using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    public GameObject InfiniteUI;
    public GameObject StageRankButton;
    public GameObject Block;

    public CanvasGroup Content;

    public Image[] BackImgs;
    public Image ModeButtonImg;
    public Sprite StageSprite;
    public Sprite InfiniteSprite;

    Sequence seq;

    void Start()
    {
        if (PlayerPrefs.GetInt("Mode", 0) == 0)
        {
            InfiniteUI.SetActive(true);
            ModeButtonImg.sprite = StageSprite;

            StageRankButton.SetActive(false);

            BackImgs[0].fillAmount = 0.5f;
            BackImgs[1].fillAmount = 0.5f;

            Content.transform.localScale = Vector3.one;
            Content.alpha = 1;
        }
        else
        {
            InfiniteUI.SetActive(false);
            ModeButtonImg.sprite = InfiniteSprite;

            StageRankButton.SetActive(true);

            BackImgs[0].fillAmount = 0;
            BackImgs[1].fillAmount = 0;

            Content.transform.localScale = Vector3.zero;
            Content.alpha = 0;
        }
        

    }

    public void SwitchMdoe()
    {
        Block.SetActive(true);

        if (PlayerPrefs.GetInt("Mode", 0) == 0)
        {
            PlayerPrefs.SetInt("Mode", 1);
            PlayerPrefs.Save();

            StageRankButton.SetActive(true);

            ModeButtonImg.sprite = InfiniteSprite;

            seq?.Kill();
            seq = DOTween.Sequence();

            seq.Append(Content.transform.DOScale(0, 0.3f).SetEase(Ease.InBack))
                .Join(Content.DOFade(0, 0.3f))
                .OnComplete(() => Content.gameObject.SetActive(false))

                .AppendInterval(0.2f)

                .Append(BackImgs[0].DOFillAmount(0, 0.4f).SetEase(Ease.InOutQuad))
                .Join(BackImgs[1].DOFillAmount(0, 0.4f).SetEase(Ease.InOutQuad))
                .Insert(0.6f, BackImgs[0].DOFade(0, 0.2f))
                .Join(BackImgs[1].DOFade(0, 0.2f))
                
                .OnComplete(() =>
                {
                    InfiniteUI.SetActive(false);
                    Block.SetActive(false);
                });
        }
        else
        {
            PlayerPrefs.SetInt("Mode", 0);
            PlayerPrefs.Save();

            InfiniteUI.SetActive(true);
            StageRankButton.SetActive(false);

            ModeButtonImg.sprite = StageSprite;

            seq?.Kill();
            seq = DOTween.Sequence();

            seq.Append(BackImgs[0].DOFillAmount(0.5f, 0.4f).SetEase(Ease.InOutQuad))
                .Join(BackImgs[1].DOFillAmount(0.5f, 0.4f).SetEase(Ease.InOutQuad))
                .Insert(0.1f, BackImgs[0].DOFade(1, 0.4f))
                .Join(BackImgs[1].DOFade(1, 0.4f))

                .AppendInterval(0.2f)

                .AppendCallback(() => 
                {
                    Content.transform.localScale = Vector3.one * 0.7f;
                    Content.gameObject.SetActive(true);
                })
                .Join(Content.transform.DOScale(1, 0.3f).SetEase(Ease.InBack))
                .Join(Content.DOFade(1, 0.3f))
                .OnComplete(() =>
                {
                    Block.SetActive(false);
                });

        }
    }
    
    public void LoadInfinite()
    {
        if (!FatigueManager.instance.CheckFatigue())
        {
            StageManager.instance.NoHeartPopUp.SetActive(true);
            return;
        }

        FatigueManager.instance.StageIn();

        PlayerPrefs.SetInt("Stage", 999);
        Debug.LogWarning("Stage: " + 999);

        AudioManager.instance.StopBGM();

        SceneManager.LoadScene("LoadingScene");
    }
}
