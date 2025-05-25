using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] images; // UI �̹��� ������Ʈ�� (a, b, c)
    private int currentIndex = 0;
    private bool canProceed = false;

    [Header("DragZone")]
    public GameObject Dragzone;

    public GameObject throwPowerImage;
    public GameObject throwHeightImage;


    void Start()
    {
        GameManager.Instance.gamePause = true;

        StartCoroutine(ShowNextImage());
    }

    void Update()
    {
        NextPhase();
    }

    IEnumerator WaitDragzone()
    {
        yield return new WaitForSeconds(2f);
    }

    void NextPhase()
    {
        // 2�ʰ� ���� �ڿ��� �Է��� ����
        if (canProceed && IsClickOrTouch())
        {
            canProceed = false;
            StartCoroutine(ShowNextImage());
        }
    }



    IEnumerator ShowNextImage()
    {
        // ���� �̹��� ��Ȱ��ȭ
        if (currentIndex > 0 && currentIndex - 1 < images.Length)
        {
            images[currentIndex - 1].SetActive(false);
        }
        if (currentIndex >= images.Length)
        {
            GameManager.Instance.gamePause = false;

            transform.gameObject.SetActive(false);
        }
        // ���� �ε����� ���� �ȿ� �ִٸ� �̹��� �����ֱ�
        if (currentIndex < images.Length)
        {
            images[currentIndex].SetActive(true);
            yield return new WaitForSeconds(3f); // 2�� ���
            canProceed = true;
            currentIndex++; // ���� �ε����� �̵�
        }
    }
    bool IsClickOrTouch()
    {
        // ���콺 Ŭ�� �Ǵ� ��ġ �Է� ����
        return Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}
