using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] images; // UI 이미지 오브젝트들 (a, b, c)
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
        // 2초가 지난 뒤에만 입력을 받음
        if (canProceed && IsClickOrTouch())
        {
            canProceed = false;
            StartCoroutine(ShowNextImage());
        }
    }



    IEnumerator ShowNextImage()
    {
        // 이전 이미지 비활성화
        if (currentIndex > 0 && currentIndex - 1 < images.Length)
        {
            images[currentIndex - 1].SetActive(false);
        }
        if (currentIndex >= images.Length)
        {
            GameManager.Instance.gamePause = false;

            transform.gameObject.SetActive(false);
        }
        // 현재 인덱스가 범위 안에 있다면 이미지 보여주기
        if (currentIndex < images.Length)
        {
            images[currentIndex].SetActive(true);
            yield return new WaitForSeconds(3f); // 2초 대기
            canProceed = true;
            currentIndex++; // 다음 인덱스로 이동
        }
    }
    bool IsClickOrTouch()
    {
        // 마우스 클릭 또는 터치 입력 감지
        return Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}
