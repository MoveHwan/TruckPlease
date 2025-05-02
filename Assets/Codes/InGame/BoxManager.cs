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
    public int remainBoxCount;      // 남은 박스 수
    public float remainBoxWeight;   // 남은 박스 무게
    public float inBoxWeight;   // 들어가 있는 박스 무게
    public int count = 0;

    private List<GameObject> spawnedBoxes = new List<GameObject>();   // 스폰된 박스를 리스트에 넣어주기
    public List<GameObject> GoaledBoxes = new List<GameObject>();   // 골인된 박스를 리스트에 넣어주기

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
            int index = spawnedBoxes.Count - 2; // 뒤에서 두 번째
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

    // 현재 몇 kg이 생성됐는지
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
    }
}
