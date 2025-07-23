using UnityEngine;
using TMPro; // TextMeshPro를 사용하는 경우
using DG.Tweening;

public class ComboEffect : MonoBehaviour
{
    public TextMeshProUGUI comboText; // UI 텍스트 연결
    public float scaleUp = 1.5f;
    public float animDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    private void OnEnable()
    {
        comboText.transform.localScale = Vector3.zero;
        comboText.alpha = 1f;

        Sequence comboSeq = DOTween.Sequence();

        comboSeq.Append(comboText.transform.DOScale(scaleUp, animDuration).SetEase(Ease.OutBack))
                .Append(comboText.transform.DOScale(1f, animDuration).SetEase(Ease.InBack))
                .AppendInterval(0.3f)
                .Append(comboText.DOFade(0f, fadeOutDuration))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false); // 비활성화 또는 Destroy(gameObject);
                });
    }

    // 콤보 숫자 갱신하고 싶을 때
    public void SetComboText(string text)
    {
        comboText.text = text;
    }
}
