using System;
using System.Collections;
using System.Collections.Generic;
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
    public int remainBoxCount;      // ���� �ڽ� ��
    public float remainBoxWeight;   // ���� �ڽ� ����
    public float inBoxWeight;   // �� �ִ� �ڽ� ����
    public int count = 0;

    private List<GameObject> spawnedBoxes = new List<GameObject>();   // ������ �ڽ��� ����Ʈ�� �־��ֱ�
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // ���ε� �ڽ��� ����Ʈ�� �־��ֱ�

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
        GameObject newObj = Instantiate(box[count]);
        spawnedBoxes.Add(newObj); 
        
        if (count < box.Length - 1) 
        { 
            count++;
        }
        if(count == box.Length - 1)
        {
            GameManager.Instance.GameEnd();
            Debug.Log("GameEnd");
        }
        CalcBoxCount();
    }

    public void DeleteBox()
    {
        if (spawnedBoxes.Count >= 2)
        {
            int index = spawnedBoxes.Count - 2; // �ڿ��� �� ��°
            GameObject objToDelete = spawnedBoxes[index];
            spawnedBoxes.RemoveAt(index);
            count--;
            Destroy(objToDelete);
            CalcBoxCount();
            CalcBoxCur();
        }
    }

    void CalcBoxCount()
    {
        remainBoxCount = box.Length - count;
    }

    // ���� �� kg�� �����ƴ���
    public void CalcBoxCur()
    {
        remainBoxWeight = 0f;

        foreach (GameObject boxObj in spawnedBoxes)
        {
            ThrowBox info = boxObj.GetComponent<ThrowBox>();
            if (info != null)
            {
                remainBoxWeight += info.weight;
            }
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
    }
}
