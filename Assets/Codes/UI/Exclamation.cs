using UnityEngine;
using DG.Tweening;

public class Exclamation : MonoBehaviour
{
    public CanvasGroup[] exclamationMark;

    Sequence sequence;


    private void OnEnable()
    {
        ActivateCounter();
    }

    public void ActivateCounter()
    {
        if (sequence != null && sequence.IsActive())
            sequence.Kill();

        sequence = DOTween.Sequence();

        exclamationMark[0].alpha = 0;
        exclamationMark[1].alpha = 0;

        sequence.AppendInterval(0.5f)
            .Append(exclamationMark[0].DOFade(1f, 1))
            .Join(exclamationMark[1].DOFade(1f, 1))
            .AppendInterval(0.5f)
            .Append(exclamationMark[0].DOFade(0f, 1))
            .Join(exclamationMark[1].DOFade(0f, 1))
            .SetLoops(-1)
            .SetEase(Ease.InOutSine);

        sequence.Play();
    }
}
