using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlock : MonoBehaviour
{
    public GameObject TotalPopUp;
    public RectTransform PopUp;
    public List<GameObject> UnlockObjs;
    public RectTransform ItemList;


    [SerializeField] bool unlock;

    void Start()
    {
        unlock = PlayerPrefs.GetInt("Stage" + PlayerPrefs.GetInt("Stage", 1) + "_Star", 0) == 0;

        TotalPopUp.SetActive(false);
    }

    public void UnlockCheck(int starCount)
    {
        if (!unlock) return;

        int stage = PlayerPrefs.GetInt("Stage", 1);
        int chapter;

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


        // 큰 박스 해금
        if (chapter == 1 && stage == 2 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Big Box");
        }

        // 긴 상자 해금
        else if (chapter == 1 && stage == 5 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Long Box");
        }

        // 세이브 아이템 해금
        else if (chapter == 1 && stage == 9 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Save Item");
        }

        // 긴 상자 해금
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
}
