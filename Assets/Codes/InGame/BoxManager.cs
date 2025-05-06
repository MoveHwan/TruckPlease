using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;

    BoxCollider storageCollider;

    public Text boxCountUi;
    public Text weightUi;
    public Text inWeightUi;
    public Text gameEndCountUi;

    public GameObject[] box;
    float totalWeight;              // 총 박스 무게
    public int remainBoxCount;      // 남은 박스 수
    public float remainBoxWeight;   // 남은 박스 무게
    public float inBoxWeight;   // 들어가 있는 박스 무게
    public int count = 0;

    public List<GameObject> spawnedBoxes = new List<GameObject>();   // 스폰된 박스를 리스트에 넣어주기
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // 골인된 박스를 리스트에 넣어주기

    GameObject curBox;      // 현재 스폰되어 있는 박스

    bool gameEndBox;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        boxCountUi.text = remainBoxCount.ToString();
        weightUi.text = remainBoxWeight.ToString();
        inWeightUi.text = inBoxWeight.ToString();
    }

    public void NextBoxSpawn()
    {
        if (count == box.Length || GameManager.Instance.gameEnd)
            return;
        curBox = Instantiate(box[count], transform);
        float randomY = Random.Range(0f, 360f);
        curBox.transform.localRotation = Quaternion.Euler(-90f, randomY, 0f); // 부모 기준으로 Y축 회전
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

        if (count < box.Length)
        {
            count++;
        }
        if (count >= box.Length)
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
        if (spawnedBoxes.Count >= 1)
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
        remainBoxCount = box.Length - count;
    }

    // 현재 몇 kg이 생성됐는지
    public void CalcBoxCur()
    {
        float minusBoxWeight = 0f;

        foreach (GameObject boxObj in spawnedBoxes)
        {
            ThrowBox info = boxObj.GetComponent<ThrowBox>();
            if (info != null)
            {
                minusBoxWeight += info.weight;
            }
        }

        remainBoxWeight = totalWeight - minusBoxWeight; 
        
        if(remainBoxWeight + inBoxWeight < GameManager.Instance.firstStar)
        {
            StartCoroutine(BoxGameEnd());
        }
    }

    // 현재 몇 kg이 찼는지
    public void CalcBoxIn()
    {
        inBoxWeight = 0f;

        if (GoaledBoxes.Count > 0)
        {
            foreach (GameObject boxObj in GoaledBoxes)
            {
                ThrowBox info = boxObj.GetComponent<ThrowBox>();
                if (info != null)
                {
                    inBoxWeight += info.weight;
                }
            }
        }

        // 3성 이상이면 게임 끝내기
        if(inBoxWeight > GameManager.Instance.thirdStar)
        {
            StartCoroutine(BoxGameEnd());
        }
    }

    // 박스 관련 게임 끝내기
    IEnumerator BoxGameEnd()
    {
        if (gameEndBox)
            yield break;

        gameEndBox = true;

        float gameEndCount = 3f;
        gameEndCountUi.gameObject.SetActive(true);

        while (gameEndCount > 0)
        {
            if (inBoxWeight < GameManager.Instance.thirdStar && count < box.Length && remainBoxWeight + inBoxWeight >= GameManager.Instance.firstStar) // 조건이 깨졌는지 다시 확인
            {
                gameEndCountUi.gameObject.SetActive(false);
                gameEndBox = false;
                yield break;
            }

            if (gameEndCountUi != null)
                gameEndCountUi.text = gameEndCount.ToString("F1");

            yield return new WaitForSeconds(0.1f);
            gameEndCount -= 0.1f;
        }

        // 3성 이상이면 게임 끝내기
        if (inBoxWeight > GameManager.Instance.thirdStar || count >= box.Length || remainBoxWeight + inBoxWeight < GameManager.Instance.firstStar)
        {
            GameManager.Instance.GameEnd();
        }
    }
}
