using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlock : MonoBehaviour
{
    public List<GameObject> UnlockObjs;
    public RectTransform ItemList;

    bool unlock;

    void Start()
    {
        unlock = PlayerPrefs.GetInt("Stage" + PlayerPrefs.GetInt("Stage", 1) + "_Star", 0) == 0;
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

        // ū �ڽ� �ر�
        if (chapter == 1 && stage == 3 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Big Box");
        }

        // �� ���� �ر�
        if (chapter == 1 && stage == 6 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Long Box");
        }

        // ���̺� ������ �ر�
        if (chapter == 1 && stage == 9 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Save Item");
        }

        // �� ���� �ر�
        if (chapter == 2 && stage == 3 && starCount > 0)
        {
            for (int i = 0; i < UnlockObjs.Count; i++)
                UnlockObjs[i].SetActive(UnlockObjs[i].name == "Tall Box");
        }

        UnlockAniStart();
    }
    
    void UnlockAniStart()
    {
        // ���� ��ġ ����
        Vector2 originalPos = ItemList.anchoredPosition;

        // ĵ���� �׷� ������ �߰�
        CanvasGroup canvasGroup = ItemList.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = ItemList.gameObject.AddComponent<CanvasGroup>();

        // ���� ����: ������ �� + ����
        ItemList.anchoredPosition = originalPos + Vector2.right * Screen.width;
        canvasGroup.alpha = 0;

        // �̵� + ���� ���ÿ� Ʈ��
        ItemList.DOAnchorPos(originalPos, 0.3f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(1f, 0.3f);
    }
}
