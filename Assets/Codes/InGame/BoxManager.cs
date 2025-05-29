using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ���ӽ����̽�


public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;

    BoxCollider storageCollider;

    public TextMeshProUGUI boxCountUi;
    public Text weightUi;
    public Text inWeightUi;
    public TextMeshProUGUI gameEndCountUi;

    public List<GameObject> box = new List<GameObject>();
    float totalWeight;              // �� �ڽ� ����
    public int remainBoxCount;      // ���� �ڽ� ��
    public float remainBoxWeight;   // ���� �ڽ� ����
    public float inBoxWeight;   // �� �ִ� �ڽ� ����
    float keepBoxWeight;            // �h �ڽ� ����Ʈ
    public int count = 0;

    public List<GameObject> spawnedBoxes = new List<GameObject>();   // ������ �ڽ��� ����Ʈ�� �־��ֱ�
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // ���ε� �ڽ��� ����Ʈ�� �־��ֱ�

    GameObject curBox;      // ���� �����Ǿ� �ִ� �ڽ�

    public bool gameEndBox;
    public bool boxReady;          // ���� �غ� �Ǹ�

    [Header("item")]
    public bool keepItem;
    public bool warnEnd;        // ���� ���ڷ� �� Ŭ�����ϴ� ����

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

    public void NextBoxSpawn()
    {
        if (count == box.Count || GameManager.Instance.gameEnd)
            return;
        Debug.Log("box.Count:" + box.Count);

        boxReady = true;
        curBox = Instantiate(box[count], transform);
        float randomY = Random.Range(0f, 360f);
        curBox.transform.localRotation = Quaternion.Euler(0f, randomY, randomY); // �θ� �������� Y�� ȸ��
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

    // �ڽ��ǵ�����
    public void DeleteBox()
    {
        if (spawnedBoxes.Count >= 1 && boxReady)
        {
            int index = spawnedBoxes.Count - 1; // �ڿ��� ù ��°
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

    // �̹� ���� �� ����
    public void CalcTotalWei()
    {
        foreach (GameObject obj in box)
        {
            ThrowBox script = obj.GetComponent<ThrowBox>();
            if (script != null)
            {
                totalWeight += script.boxData.Weight;
                Debug.Log("������");
            }
        }
        remainBoxWeight = totalWeight;
    }

    //�� �� ���Ҵ���
    public void CalcBoxCount()
    {
        remainBoxCount = box.Count - count;
    }

    // ���� �� kg�� �����ƴ���
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

    // ���� �� kg�� �����ƴ��� Ȯ�� �� ���� ��
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
            StartCoroutine(BoxGameEnd());
        }
    }


    // ���� �� kg�� á����
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

    // �ڽ� ���� ���� ������
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
            gameEndCount = 10f;
        }
        gameEndCountUi.transform.parent.gameObject.SetActive(true);

        while (gameEndCount >= 0)
        {
            if (inBoxWeight < GameManager.Instance.thirdStar && count < box.Count && remainBoxWeight + inBoxWeight >= GameManager.Instance.firstStar) // ������ �������� �ٽ� Ȯ��
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
            gameEndCount -= 0.1f;
        }
        gameEndCount = 0f;
        GameManager.Instance.GameEnd();
    }

    // ������ �h �ߵ�
    public void ActiveKeepItem()
    {
        if (keepItem)
        {
            KeepItemBox();
        }
        keepItem = !keepItem;
    }

    // ������ �h
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
                    Debug.Log("�h��");
                    keepBoxWeight += thisBox.GetComponent<ThrowBox>().weight;
                    box.Remove(thisBox);
                    spawnedBoxes.Remove(thisBox);
                    GoaledBoxes.RemoveAt(i); // �����ϰ� ���� ����
                    thisBox.gameObject.SetActive(false);
                }
            }
            Debug.Log("box.Count:" + box.Count);

            CalcBoxIn();
        }
    }
}
