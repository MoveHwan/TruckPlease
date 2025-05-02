using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;
    
    public GameObject[] box;
    public int count = 0;

    private List<GameObject> spawnedBoxes = new List<GameObject>();   // 스폰된 박스를 리스트에 넣어주기

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        NextBoxSpawn();
    }

    void Update()
    {
        
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
    }

    public void DeleteBox()
    {
        if (spawnedBoxes.Count >= 2)
        {
            int index = spawnedBoxes.Count - 2; // 뒤에서 두 번째
            GameObject objToDelete = spawnedBoxes[index];
            spawnedBoxes.RemoveAt(index);
            Destroy(objToDelete);
        }
    }
}
