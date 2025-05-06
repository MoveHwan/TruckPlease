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
    float totalWeight;              // �� �ڽ� ����
    public int remainBoxCount;      // ���� �ڽ� ��
    public float remainBoxWeight;   // ���� �ڽ� ����
    public float inBoxWeight;   // �� �ִ� �ڽ� ����
    public int count = 0;

    public List<GameObject> spawnedBoxes = new List<GameObject>();   // ������ �ڽ��� ����Ʈ�� �־��ֱ�
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // ���ε� �ڽ��� ����Ʈ�� �־��ֱ�

    GameObject curBox;      // ���� �����Ǿ� �ִ� �ڽ�

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
        curBox.transform.localRotation = Quaternion.Euler(-90f, randomY, 0f); // �θ� �������� Y�� ȸ��
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

    // �ڽ��ǵ�����
    public void DeleteBox()
    {
        if (spawnedBoxes.Count >= 1)
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
        remainBoxCount = box.Length - count;
    }

    // ���� �� kg�� �����ƴ���
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

    // ���� �� kg�� á����
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

        // 3�� �̻��̸� ���� ������
        if(inBoxWeight > GameManager.Instance.thirdStar)
        {
            StartCoroutine(BoxGameEnd());
        }
    }

    // �ڽ� ���� ���� ������
    IEnumerator BoxGameEnd()
    {
        if (gameEndBox)
            yield break;

        gameEndBox = true;

        float gameEndCount = 3f;
        gameEndCountUi.gameObject.SetActive(true);

        while (gameEndCount > 0)
        {
            if (inBoxWeight < GameManager.Instance.thirdStar && count < box.Length && remainBoxWeight + inBoxWeight >= GameManager.Instance.firstStar) // ������ �������� �ٽ� Ȯ��
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

        // 3�� �̻��̸� ���� ������
        if (inBoxWeight > GameManager.Instance.thirdStar || count >= box.Length || remainBoxWeight + inBoxWeight < GameManager.Instance.firstStar)
        {
            GameManager.Instance.GameEnd();
        }
    }
}
