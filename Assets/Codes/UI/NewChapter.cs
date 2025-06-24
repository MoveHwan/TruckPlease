using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChapter : MonoBehaviour
{
    public GameObject Chapter;
    public CanvasGroup Lock;

    public int idx;

    [SerializeField] bool isNew;

    Sequence UnlockSeq;


    private void OnEnable()
    {
        if (isNew)
        {
            idx = int.Parse(Chapter.name.Split(" ")[1]) - 1;

            UnlockSeq = DOTween.Sequence();

            UnlockSeq.Pause();

            UnlockSeq.AppendCallback(() =>
            {
                Lock.gameObject.SetActive(true);
                Lock.alpha = 1;
            });

            UnlockSeq
                .Append(Lock.transform.DOScale(0.8f, 0.3f).SetEase(Ease.OutBack))
                .Append(Lock.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
                .Join(Lock.DOFade(0, 0.5f));

            UnlockSeq.AppendCallback(() => 
            {
                string str = Chapter.name + "_new";

                PlayerPrefs.SetInt(str, 0);

                Lock.gameObject.SetActive(false);

                DOTween.Kill(UnlockSeq);
            });


            ScrollHelper.instance.NewChapter = this;
        }


    }


    public void PlayUnlockSeq()
    {
        UnlockSeq.Play();
    }

    public void NewOn()
    {
        isNew = PlayerPrefs.GetInt(Chapter.name + "_new", 0) == 1;

        gameObject.SetActive(isNew);

        Lock.gameObject.SetActive(isNew);
    }

}
