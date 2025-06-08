using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public Image tutoPanel;

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

        // 현재 인덱스가 범위 안에 있다면 이미지 보여주기
        if (currentIndex <= 1)
        {
            images[currentIndex].SetActive(true);
            yield return new WaitForSeconds(3f); // 2초 대기
            canProceed = true;
            currentIndex++; // 다음 인덱스로 이동
        }
        else if (currentIndex == 2)
        {
            Color tuto = tutoPanel.color;
            tuto.a = 0f;
            tutoPanel.color = tuto;
            ThrowTouchPanel.Instance.controlBox.TutoThrow(new Vector3(0f, 2f, 2f));
            yield return new WaitForSeconds(2f); // 2초 대기
            tuto.a = 0.6f;
            tutoPanel.color = tuto;

            images[2].SetActive(true);
            canProceed = true;
            currentIndex++; // 다음 인덱스로 이동
        }
        else if (currentIndex == 3) 
        {
            Color tuto = tutoPanel.color;
            tuto.a = 0f;
            tutoPanel.color = tuto;
            ThrowTouchPanel.Instance.controlBox.TutoThrow(new Vector3(0f, 5f, 5f));
            yield return new WaitForSeconds(2f); // 2초 대기
            tuto.a = 0.6f;
            tutoPanel.color = tuto;

            images[3].SetActive(true);
            canProceed = true;
            currentIndex++; // 다음 인덱스로 이동

        }
        else if (currentIndex == 4)
        {
            Color tuto = tutoPanel.color;
            tuto.a = 0f;
            tutoPanel.color = tuto;
            ThrowTouchPanel.Instance.controlBox.TutoThrow(new Vector3(0f, 12f, 7f));
            yield return new WaitForSeconds(3f); // 2초 대기
            PlayerPrefs.SetInt("Tutorial", 1);
            PlayerPrefs.Save();
            tutoPanel.gameObject.SetActive(false);
        }
    }
    bool IsClickOrTouch()
    {
        // 마우스 클릭 또는 터치 입력 감지
        return Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}
