using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUnlock : MonoBehaviour
{
    public GameObject TotalPopUp;
    public RectTransform PopUp;
    public List<GameObject> UnlockObjs;
    public RectTransform ItemList;

    public RectTransform NextUnlock;
    public Image NextUnlockIcon;
    public TextMeshProUGUI NextUnlockStage;

    int stage, chapter;

    [SerializeField] bool unlock;

    void Start()
    {
        unlock = PlayerPrefs.GetInt("Stage" + PlayerPrefs.GetInt("Stage", 1) + "_Star", 0) == 0;

        TotalPopUp.SetActive(false);

        stage = PlayerPrefs.GetInt("Stage", 1);

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

        ViewUnlockCheck();
    }


    public void UnlockCheck(int starCount)
    {
        if (!unlock) return;

        // ū �ڽ� �ر�
        if (chapter == 1 && stage == 2 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Big Box");
        }

        // �� ���� �ر�
        else if (chapter == 1 && stage == 5 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Long Box");
        }

        // ���̺� ������ �ر�
        else if (chapter == 1 && stage == 9 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Save Item");
        }

        // ���� ���� �ر�
        else if (chapter == 2 && stage == 3 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Tall Box");
        }

        else
            return;

        UnlockAniStart();
    }
    
    void UnlockAniStart()
    {
        TotalPopUp.SetActive(true);

        CanvasGroup canvasGroup = PopUp.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            return;
        }
            
        canvasGroup.gameObject.SetActive(true);

        canvasGroup.alpha = 0;
        canvasGroup.transform.localScale = Vector3.one * 0.8f;

        canvasGroup.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, 0.5f);



        Vector2 originalPos = ItemList.anchoredPosition;
        canvasGroup = ItemList.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup.DOKill();
            return;
        }

        ItemList.anchoredPosition = originalPos + Vector2.right * Screen.width;
        canvasGroup.alpha = 0;

        ItemList.DOAnchorPos(originalPos, 0.8f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(1f, 0.8f);
        
    }


    void ViewUnlockCheck()
    {
        string[] strArr = PlayerPrefs.GetString("TopRatingStage", "1_0").Split("_");

        float topStage = float.Parse(strArr[0]) + float.Parse(strArr[1]) / 10;

        Debug.LogWarning("UnlockCheck Stage: " + topStage);

        // ū �ڽ� �ر�
        if (topStage < 1.2f)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
            {
                if (UnlockObjs[i].name == "Big Box")
                {
                    NextUnlockIcon.sprite = UnlockObjs[i].transform.GetChild(1).GetComponent<Image>().sprite;
                }
            }

            NextUnlockStage.text = "Ch 1-3";
        }

        // �� ���� �ر�
        else if (topStage < 1.5f)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
            {
                if (UnlockObjs[i].name == "Long Box")
                {
                    NextUnlockIcon.sprite = UnlockObjs[i].transform.GetChild(1).GetComponent<Image>().sprite;
                }
            }

            NextUnlockStage.text = "Ch 1-6";
        }

        // ���̺� ������ �ر�
        else if (topStage < 1.9f)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
            {
                if (UnlockObjs[i].name == "Save Item")
                {
                    NextUnlockIcon.sprite = UnlockObjs[i].transform.GetChild(1).GetComponent<Image>().sprite;
                }
            }

            NextUnlockStage.text = "Ch 2-1";
        }

        // ���� ���� �ر�
        else if (topStage < 2.3f)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
            {
                if (UnlockObjs[i].name == "Tall Box")
                {
                    NextUnlockIcon.sprite = UnlockObjs[i].transform.GetChild(1).GetComponent<Image>().sprite;
                }
            }

            NextUnlockStage.text = "Ch 2-4";
        }

        else
        {
            NextUnlock.gameObject.SetActive(false);
            return;
        }

        NextUnlockAni();
    }


    void NextUnlockAni()
    {
        Sequence seq = DOTween.Sequence();

        // ���� ��ġ ����
        Vector2 originalPos = NextUnlock.anchoredPosition;

        // ĵ���� �׷� ������ �߰�
        CanvasGroup canvasGroup = NextUnlock.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = NextUnlock.gameObject.AddComponent<CanvasGroup>();
        }

        // ���� ����: ������ �� + ����
        NextUnlock.anchoredPosition = originalPos + Vector2.right * Screen.width;
        canvasGroup.alpha = 0;

        // �̵� + ���� ���ÿ� Ʈ��
        seq.AppendInterval(0.3f)
            .Append(NextUnlock.DOAnchorPos(originalPos, 0.3f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.3f))
            .AppendInterval(0.05f);
    }

}
