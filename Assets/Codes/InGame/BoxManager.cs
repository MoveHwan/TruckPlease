using System;
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

    public GameObject[] box;
    float totalWeight;              // �� �ڽ� ����
    public int remainBoxCount;      // ���� �ڽ� ��
    public float remainBoxWeight;   // ���� �ڽ� ����
    public float inBoxWeight;   // �� �ִ� �ڽ� ����
    public int count = 0;

    public List<GameObject> spawnedBoxes = new List<GameObject>();   // ������ �ڽ��� ����Ʈ�� �־��ֱ�
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // ���ε� �ڽ��� ����Ʈ�� �־��ֱ�

    GameObject curBox;      // ���� �����Ǿ� �ִ� �ڽ�

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
        curBox = Instantiate(box[count],transform);
    }

    public void RemainBoxCal(GameObject throwBox) 
    {
        spawnedBoxes.Add(throwBox);

        if (count < box.Length - 1)
        {
            count++;
        }
        if (count == box.Length - 1)
        {
            GameManager.Instance.GameEnd();
            Debug.Log("GameEnd");
        }
        CalcBoxCur();
        CalcBoxCount();
    }

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
    }
}
