using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 네임스페이스


public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;

    BoxCollider storageCollider;

    public GameObject[] boxPool;        // 박스로 쓸 종류들
    public TextMeshProUGUI boxCountUi;
    public Text weightUi;
    public Text inWeightUi;
    public TextMeshProUGUI gameEndCountUi;

    public List<GameObject> box = new List<GameObject>();   // 쓸 박스들
    float totalWeight;              // 총 박스 무게
    public int remainBoxCount;      // 남은 박스 수
    public float remainBoxWeight;   // 남은 박스 무게
    public float inBoxWeight;   // 들어가 있는 박스 무게
    float keepBoxWeight;            // 킾 박스 웨이트
    public int count = 0;

    public List<GameObject> spawnedBoxes = new List<GameObject>();   // 스폰된 박스를 리스트에 넣어주기
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // 골인된 박스를 리스트에 넣어주기

    GameObject curBox;      // 현재 스폰되어 있는 박스

    public bool gameEndBox;
    public bool boxReady;          // 상자 준비 되면
    public bool boxWarn;                  // 상자 다던져도 안될때
    [Header("item")]
    public bool keepItem;
    public bool warnEnd;        // 남은 상자로 못 클리어하는 변수
    public bool itemBuy;        // 아이템을 살려고 하면 시간을 멈추게 하는변수

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        //boxCountUi.text = remainBoxCount.ToString();
        weightUi.text = remainBoxWeight.ToString();
        inWeightUi.text = inBoxWeight.ToString();
    }

    // boxPool에서 랜덤하게 count개 만큼 선택해서 box 리스트에 추가
    public void AddRandomBoxes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject randomBox = boxPool[Random.Range(0, boxPool.Length)];
            box.Add(randomBox);
        }
    }

    // boxPool에서 랜덤하게 하나 선택해서 box 리스트 맨 뒤에 추가
    public void AddOneRandomBox()
    {
        GameObject randomBox = boxPool[Random.Range(0, boxPool.Length)];
        box.Add(randomBox);
    }

    public void NextBoxSpawn()
    {
        if (count == box.Count || GameManager.Instance.gameEnd)
            return;
        Debug.Log("box.Count:" + box.Count);

        boxReady = true;
        curBox = Instantiate(box[count], transform);
        ThrowTouchPanel.Instance.controlBox = curBox.GetComponent<ThrowBox>();
        float randomY = Random.Range(0f, 360f);
        curBox.transform.localRotation = Quaternion.Euler(0f, randomY, randomY); // 부모 기준으로 Y축 회전
        AddOneRandomBox();
    }

    public void NextBoxSpawnWait()
    {
        StartCoroutine(NextBoxSpawnCor());
    }

    IEnumerator NextBoxSpawnCor()
    {
        yield return new WaitForSeconds(1.5f);
        NextBoxSpawn();
    }

    public void RemainBoxCal(GameObject throwBox)
    {
        spawnedBoxes.Add(throwBox);

        if (count < box.Count)
        {
            count++;
        }
        if (count >= box.Count)
        {
            StartCoroutine(BoxGameEnd());
            Debug.Log("GameEnd");
        }
        CalcBoxCur();
        CalcBoxCount();
    }

    // 박스되돌리기
    public void DeleteBox()
    {
        if (spawnedBoxes.Count >= 1 && boxReady)
        {
            int index = spawnedBoxes.Count - 1; // 뒤에서 첫 번째
            GameObject objToDelete = spawnedBoxes[index];
            spawnedBoxes.RemoveAt(index);
            GoaledBoxes.Remove(objToDelete);
            count--;
            Destroy(objToDelete);
            Destroy(curBox);
            NextBoxSpawn();
            CalcBoxCount();
            CalcBoxCur();
            CalcBoxIn();
        }
    }

    // 이번 라운드 총 무게
    public void CalcTotalWei()
    {
        foreach (GameObject obj in box)
        {
            ThrowBox script = obj.GetComponent<ThrowBox>();
            if (script != null)
            {
                totalWeight += script.boxData.Weight;
                Debug.Log("더해짐");
            }
        }
        remainBoxWeight = totalWeight;
    }

    //몇 번 남았는지
    public void CalcBoxCount()
    {
        remainBoxCount = box.Count - count;
    }

    // 현재 몇 kg이 생성됐는지
    public void CalcBoxCur()
    {
        float minusBoxWeight = keepBoxWeight;

        foreach (GameObject boxObj in spawnedBoxes)
        {
            ThrowBox info = boxObj.GetComponent<ThrowBox>();
            if (info != null)
            {
                minusBoxWeight += info.weight;
            }
        }

        remainBoxWeight = totalWeight - minusBoxWeight; 
        
    }

    // 현재 몇 kg이 생성됐는지 확인 후 게임 끝
    public void CalcBoxCurEnd()
    {
        float minusBoxWeight = keepBoxWeight;

        foreach (GameObject boxObj in spawnedBoxes)
        {
            ThrowBox info = boxObj.GetComponent<ThrowBox>();
            if (info != null)
            {
                minusBoxWeight += info.weight;
            }
        }

        remainBoxWeight = totalWeight - minusBoxWeight;

        if (remainBoxWeight + inBoxWeight < GameManager.Instance.firstStar)
        {
            StartCoroutine(BoxWarn());
        }
    }


    // 현재 몇 kg이 찼는지
    public void CalcBoxIn()
    {
        inBoxWeight = keepBoxWeight;

        if (GoaledBoxes.Count > 0)
        {
            foreach (GameObject boxObj in GoaledBoxes)
            {
                ThrowBox info = boxObj.GetComponent<ThrowBox>();
                if (info != null)
                {
                    inBoxWeight += info.weight;
                    Debug.Log(inBoxWeight);

                }
            }
        }

    }

    // 박스 관련 게임 끝내기
    IEnumerator BoxGameEnd()
    {
        if (gameEndBox)
            yield break;

        gameEndBox = true;
        GameManager.Instance.gamePause = true;

        float gameEndCount;

        if (count >= box.Count)
        {
            gameEndCount = 3f;
        }
        else
        {
            warnEnd = true;
            gameEndCount = 7f;
        }
        gameEndCountUi.transform.parent.gameObject.SetActive(true);

        while (gameEndCount >= 0)
        {
            if (inBoxWeight < GameManager.Instance.thirdStar && count < box.Count && remainBoxWeight + inBoxWeight >= GameManager.Instance.firstStar) // 조건이 깨졌는지 다시 확인
            {
                gameEndBox = false;
                GameManager.Instance.gamePause = false;
                gameEndCountUi.transform.parent.gameObject.SetActive(false);

                warnEnd = false;
                yield break;
            }

            if (gameEndCountUi != null)
                gameEndCountUi.text = gameEndCount.ToString("F1");

            yield return new WaitForSeconds(0.1f);
            if (!itemBuy)
            {
                gameEndCount -= 0.1f;
            }
        }
        gameEndCount = 0f;
        GameManager.Instance.GameEnd();
    }

    // 다 던져도 못넣을때 경고
    IEnumerator BoxWarn()
    {
        if (boxWarn)
            yield break;

        boxWarn = true;

        while (remainBoxWeight + inBoxWeight < GameManager.Instance.firstStar)
        {
            yield return new WaitForSeconds(1f);
        }
        boxWarn = false;
        yield break;
    }

    // 아이템 킾 발동
    public void ActiveKeepItem()
    {
        if (keepItem)
        {
            KeepItemBox();
        }
        keepItem = !keepItem;
    }

    // 아이템 킾
    public void KeepItemBox()
    {
        if (keepItem)
        {
            for (int i = GoaledBoxes.Count - 1; i >= 0; i--)
            {
                var thisBox = GoaledBoxes[i];
                TouchOutline touchOutline = thisBox.GetComponent<TouchOutline>();
                if (touchOutline != null && touchOutline.isOutlined)
                {
                    Debug.Log("킾됨");
                    keepBoxWeight += thisBox.GetComponent<ThrowBox>().weight;
                    box.Remove(thisBox);
                    spawnedBoxes.Remove(thisBox);
                    GoaledBoxes.RemoveAt(i); // 안전하게 제거 가능
                    thisBox.gameObject.SetActive(false);
                }
            }
            Debug.Log("box.Count:" + box.Count);

            CalcBoxIn();
        }
    }

    // 수박 게임 상자 생성
    public IEnumerator NextBigBoxCo(GameObject nextBox, Vector3 position)
    {
        yield return new WaitForSeconds(0.2f);
        Quaternion randomRotation = Quaternion.Euler(
        Random.Range(0f, 360f),
        Random.Range(0f, 360f),
        Random.Range(0f, 360f)
        );
        Instantiate(nextBox, position, randomRotation);
    }

    public void NextBigBox(GameObject nextBox, Vector3 position)
    {
        StartCoroutine(NextBigBoxCo(nextBox,position));
    }
}
